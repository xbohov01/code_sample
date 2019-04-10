//ISA 2018/19
//Main file for myripresponse
//Samuel Bohovic
//xbohov01

#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pcap.h>
#include <string>
#include <csignal>
#include <netinet/in.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <string.h>
#include <ifaddrs.h>
#include <sys/ioctl.h>
#include <net/if.h>
#include <algorithm>
#include "RIPngPacket.h"

#define RIPNGPORT 521

using namespace std;

bool IsIPValid(char *ip){
    
}

//Return IP address from argument without delimiters
char* ExtractAddress(char *ipAndMask){
    string str(ipAndMask);
    str.erase(std::remove(str.begin(), str.end(), ':'), str.end());
       
    string ip = str;
    /*if (str.find('/') != string::npos){
        ip = str.substr(0, str.find('/'));
    }*/

    return (char*)ip.c_str();
}

char* ExtractMask(char *ipAndMask){
    char *split = strtok(ipAndMask, "/");
    split = strtok(NULL, "/");
    return split;
}

int main(int argc, char *argv[])
{
    char *interface;
    char *ipAndMask;
    char *metric = (char*)"1";
    char *nextHop = (char*)"::";
    char *routeTag = (char*)"0";
    char *ip; //This is the fake IP
    char *mask;
    int clientSocket;
    struct sockaddr_in6 destination;
    struct sockaddr_in6 source;

    //Process arguments
    if (argc < 3){
        fprintf(stderr, "Not enough arguments.\n");
        return 1;
    }
    int opt;
    while((opt = getopt(argc, argv, "i:r:n:m:t:")) != -1){
        switch(opt){
            case 'i':
                interface = optarg;
                break;
            case 'r':
                ipAndMask = optarg;
                if (strlen(ipAndMask) < 42){
                    fprintf(stderr, "Prefix is too short.\n");
                    return 1;
                }
                break;
            case 'n':
                nextHop = ExtractAddress(optarg);
                if (strlen(nextHop) < 39){
                    fprintf(stderr, "Next hop is too short.\n");
                    return 1;
                }
                break;
            case 'm':
                metric = optarg;
                break;
            case 't':
                routeTag = optarg;
                break;
            case '?':
                fprintf(stderr, "Incorrect argument.\n");
                return 1;
            default:
                exit(1);
        }
    }

    printf("Interface: %s\n", interface);
    printf("Metric: %s\n", metric);
    printf("Next hop address: %s\n", nextHop);
    printf("Route tag: %s\n", routeTag);

    ip = ExtractAddress(ipAndMask);
    printf("Fake network address: %s\n", ipAndMask);

    //Prep to send
    //Prep source address
    memset(&source, 0, sizeof(source));
    source.sin6_family = AF_INET6;
    source.sin6_port = htons(RIPNGPORT);
    source.sin6_addr = in6addr_any;

    //Prep destination address
    memset(&destination, 0, sizeof(destination));
    destination.sin6_family = AF_INET6;
    destination.sin6_port = htons(RIPNGPORT);
    inet_pton(AF_INET6, "ff02::9", &destination.sin6_addr); //Send to RIP multicast address

    //Create socket
    clientSocket = socket(AF_INET6, SOCK_DGRAM, IPPROTO_UDP);
    if (clientSocket < 0){
        fprintf(stderr, "Failed to create a socket.\n");
        exit(1);
    }

    //Bind socket to port
    if (bind(clientSocket, (struct sockaddr *) &source, sizeof(source)) < 0) {
        perror("bind");
        exit(1);
    }

    //Set socket
    if (setsockopt(clientSocket, SOL_SOCKET, SO_BINDTODEVICE, interface, strlen(interface))){
        perror("setsockopt"); 
        return 1;
    }
    
    //Build RIPng packet
    mask = ExtractMask(ipAndMask);
    RIPngPacket *ripngPacket = new RIPngPacket(ip, routeTag, mask, metric, nextHop);
   
    //Send packet
    if (sendto(clientSocket, ripngPacket->packet, ripngPacket->len, 0, (struct sockaddr*)&destination, sizeof(destination)) < 0){
        perror("Failed to send message");
        return 1;
    }

    return 0;
}
