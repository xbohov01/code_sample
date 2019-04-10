//ISA 2018/19
//Main file for myriprequest
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

int main(int argc, char *argv[])
{
    char *interface; //Interface to use
    char *metric = (char*)"16";
    char *nextHop = (char*)"::";
    char *routeTag = (char*)"0";
    char *ip = (char*)"00000000000000000000000000000000";
    char *mask = (char*)"0";
    int clientSocket;
    struct sockaddr_in6 destination;
    struct sockaddr_in6 source;

    //Process arguments
    if (argc == 1){
        fprintf(stderr, "Not enough arguments.\n");
        return 1;
    }
    int opt;
    while((opt = getopt(argc, argv, "i:")) != -1){
        switch (opt){
            case 'i':
                interface = optarg;
                break;
            case '?':
                fprintf(stderr, "Incorrect argument.\n");
                return 1;
            default:
                exit(1);
        }
    }
    if (interface == NULL){
        fprintf(stderr, "Incorrect argument.\n");
        return 1;
    }

    printf("Seinding fake request on interface: %s\n", interface);

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
    RIPngPacket *ripngPacket = new RIPngPacket(ip, routeTag, mask, metric, nextHop);

    //Change Response to Request
    ripngPacket->packet[0] = 1;

    //Send packet
    if (sendto(clientSocket, ripngPacket->packet, ripngPacket->len, 0, (struct sockaddr*)&destination, sizeof(destination)) < 0){
        perror("Failed to send message");
        return 1;
    }

    return 0;
}