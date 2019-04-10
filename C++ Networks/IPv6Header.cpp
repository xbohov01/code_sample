//ISA 2018/19
//Class to represent IPv4 frame in packet
//Samuel Bohovic
//xbohov01

#include "IPv6Header.h"

IPv6Header::IPv6Header(const u_char *packet){
    char ipChar[INET6_ADDRSTRLEN];
    memset(&ipChar, '\0' ,INET6_ADDRSTRLEN);

    //Parse IP
    //Could be done just with pointers but direct approach makes it easier to format
    sprintf(ipChar, "%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x", 
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+1],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+2],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+3],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+4],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+5],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+6],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+7],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+8],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+9],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+10],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+11],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+12],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+13],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+14],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+15]);

    memcpy(this->sourceIP, ipChar, INET6_ADDRSTRLEN);

    memset(&ipChar, '\0' ,INET6_ADDRSTRLEN);
    sprintf(ipChar, "%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x:%02x%02x", 
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+16],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+17],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+18],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+19],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+20],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+21],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+22],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+23],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+24],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+25],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+26],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+27],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+28],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+29],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+30],
        packet[ETHER_HEADER_LEN+IP6_HEADER_LEN+31]);

    memcpy(this->destinationIP, ipChar, INET6_ADDRSTRLEN);

    return;
}