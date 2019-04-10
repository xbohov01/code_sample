//ISA 2018/19
//Class to represent IPv4 frame in packet
//Samuel Bohovic
//xbohov01

#include "IPv4Header.h"

IPv4Header::IPv4Header(const u_char *packet){
    char ipChar[INET_ADDRSTRLEN];
    memset(&ipChar, '\0', INET_ADDRSTRLEN);

    //Parse IP addresses
    sprintf(ipChar, "%d.%d.%d.%d", packet[ETHER_HEADER_LEN+12], packet[ETHER_HEADER_LEN+13], packet[ETHER_HEADER_LEN+14], packet[ETHER_HEADER_LEN+15]);
    memcpy(this->sourceIP, ipChar, INET_ADDRSTRLEN);

    memset(&ipChar, '\0', INET_ADDRSTRLEN);
    sprintf(ipChar, "%d.%d.%d.%d", packet[ETHER_HEADER_LEN+16], packet[ETHER_HEADER_LEN+17], packet[ETHER_HEADER_LEN+18], packet[ETHER_HEADER_LEN+19]);
    memcpy(this->destinationIP, ipChar, INET_ADDRSTRLEN);
}