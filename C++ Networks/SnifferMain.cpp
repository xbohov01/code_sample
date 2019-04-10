//ISA 2018/19
//Samuel Bohovic
//xbohov01

#include "EthernetHeader.h"
#include "IPv4Header.h"
#include "IPv6Header.h"
#include "RIPPacket.h"
#include "RIPPayload.h"
#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pcap.h>
#include <string>
#include <csignal>
#include <string>

#define UDP_HEADER_LEN 8

using namespace std;

//Holds global pointer to session handle for alarms
pcap_t *sessionHandle;

void alarmHander(int sig){
    printf("60 seconds over!\n");
    pcap_breakloop(sessionHandle);
}

//Parsing RIP packet payloads(route table entries)
void ParseRIPPayloads(const u_char *data, RIPPacket *packet){
    int offset = RIP_HEADER_LEN;

    char afi[2], routeTag[2];
/*
    for (int i = 0; i < 10; i++){
        printf("%d: %02x\n", i, data[offset+i]);
    }*/

    if(packet->version == 2){
        //Expect first payload to be auth
        if(data[offset] == 255){
            //Check type of authentication
            int authType;
            authType = (int)data[offset+3];

            if (authType == 2){
                //Simple password authentication
                memcpy(packet->password, data+8, 16);
                offset += RIP_HEADER_LEN;
            } else if (authType == 3){
                //MD5 authentication
                strcpy(packet->password, "MD5");
            } else {
                strcpy(packet->password, "Other");
            }
            
        }
    }

    //Parse payloads
    //This part can be added, but I don't think route table items are needed

    return;

}

void PrintCharArrayAsHex(u_char *array, int len){

    if (len == 0){
            for(int i = 0; i < sizeof (array); i++){
            printf("%02x", array[i]);
            if (i < sizeof array - 1){
                printf(" ");
            }
        }
    } else {
        for(int i = 0; i < len; i++){
            printf("%02x", array[i]);
            if (i < len - 1){
                printf(" ");
            }
        }
    }

    printf("\n");

    return;
}

//Callback function to handle RIPng packets
void GotRIPngPacketCallback(u_char *args, const struct pcap_pkthdr *header, const u_char *packet){
    static int order = 0;

    //Create new Ethernet header
    EthernetHeader *ethernetHeader = new EthernetHeader(packet);

    //Create IPv6 header
    IPv6Header *ipv6Header = new IPv6Header(packet);

    //No authentication so no need to proccess RIP payloads

    //Print relevant data
    printf("Relevant packet caught (%d):\n", order++);
    printf("    Source IP       : %s\n", ipv6Header->sourceIP);
    printf("    Destination IP  : %s\n", ipv6Header->destinationIP);

    printf("=================================================\n");

    return;
}

//Callback function to handle RIPv1 RIPv2 packets reception
void GotRIPPacketCallback(u_char *args, const struct pcap_pkthdr *header, const u_char *packet){
    static int order = 0;

    //Create new Ethernet header
    EthernetHeader *ethernetHeader = new EthernetHeader(packet);

    //Create IP header
    IPv4Header *ipv4Header = new IPv4Header(packet);

    //Create RIP packet
    RIPPacket *ripPacket = new RIPPacket();

    //Detect type and version of RIP part
    if(packet[ETHER_HEADER_LEN+IP4_HEADER_LEN+UDP_HEADER_LEN] == 2){        
        //Detect version
        if(packet[ETHER_HEADER_LEN+IP4_HEADER_LEN+UDP_HEADER_LEN+1] == 1){
            //RIPv1            
            ripPacket->version = 1;
            strcpy(ripPacket->password, "NA");
        } else {
            //RIPv2
            ripPacket->version = 2;
        }
        //Parse payload
        ParseRIPPayloads(packet+ETHER_HEADER_LEN+IP4_HEADER_LEN+UDP_HEADER_LEN, ripPacket);
    } else {
        //Not a Response
        return;
    }

    //Print data for user
    printf("Relevant packet caught (%d):\n", order++);
    printf("    Source IP     : %s\n", ipv4Header->sourceIP);
    printf("    Destination IP: %s\n", ipv4Header->destinationIP);
    printf("    Version       : RIPv%d\n", ripPacket->version);
    printf("    Password      : %s\n", ripPacket->password);

    printf("=================================================\n");

    return;
}

void CompileAndSetRIPFilter(pcap_t* handle, bpf_u_int32 deviceIp){
    struct bpf_program compiledFilter; //Compiled filter expression

    //Compile filter
    if (pcap_compile(handle, &compiledFilter, "port 520", 0, deviceIp) == -1){
        fprintf(stderr, "Couldn't parse filter: %s\n", pcap_geterr(handle));
		exit(1);
    }

    //Set filter
    if (pcap_setfilter(handle, &compiledFilter) == -1) {
		fprintf(stderr, "Couldn't install filter: %s\n", pcap_geterr(handle));
		exit(1);
	}

    return;
}

void CompileAndSetRIPngFilter(pcap_t* handle, bpf_u_int32 deviceIp){
    struct bpf_program compiledFilter; //Compiled filter expression

    //Compile filter
    if (pcap_compile(handle, &compiledFilter, "port 521", 0, deviceIp) == -1){
        fprintf(stderr, "Couldn't parse filter: %s\n", pcap_geterr(handle));
		exit(1);
    }

    //Set filter
    if (pcap_setfilter(handle, &compiledFilter) == -1) {
		fprintf(stderr, "Couldn't install filter: %s\n", pcap_geterr(handle));
		exit(1);
	}

    return;
}

int main(int argc, char *argv[])
{
    char *interface; //Interface to use
    char errBuf[PCAP_ERRBUF_SIZE]; //Error string
    pcap_t* handle; //Session handle
 
    char filterExpressionRIP[] = "port 520"; //Filter expression for RIPv1 and RIPv2
    char filterExpressionRIPng[] = "port 521"; //Filter expression for RIPng

    bpf_u_int32 deviceMask;
    bpf_u_int32 deviceIp;
    struct pcap_pkthdr header; 
    const u_char *packet;

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

    printf("Setting up sniffer on interface: %s\nRIP payloads will not be processed\n", interface);

    //Get mask and IP
    if (pcap_lookupnet(interface, &deviceIp, &deviceMask, errBuf) == -1) {
		 fprintf(stderr, "Can't get mask and IP for interface %s\n", interface);
		 deviceIp = 0;
		 deviceMask = 0;
	}

    //Create session
    handle = pcap_open_live(interface, BUFSIZ, 1, 1000, errBuf);
    if (handle == NULL){
        fprintf(stderr, "Couldn't open session on interface %s: %s\n", interface, errBuf);
		return(1);
    }
    sessionHandle = handle;

    //Configure RIPv1 RIPv2 filter
    CompileAndSetRIPFilter(handle, deviceIp);

    //Star sniffing
    printf("Sniffing for RIPv1 and RIPv2 packets for 60 seconds:\n");

    //Set alarm
    alarm(60);
    signal(SIGALRM, alarmHander);

    pcap_loop(handle, 16, GotRIPPacketCallback, NULL);

    //Configure RIPng filter
    CompileAndSetRIPngFilter(handle, deviceIp);

    //Start sniffing
    printf("Sniffing for RIPng packets for 60 seconds:\n");

    //Set alarm
    alarm(60);

    pcap_loop(handle, 16, GotRIPngPacketCallback, NULL);

    pcap_close(handle);

    return 0;
}
