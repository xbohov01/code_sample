//ISA 2018/19
//Class to represent ethernet frame in packet
//Samuel Bohovic
//xbohov01

#include "EthernetHeader.h"

using namespace std;

EthernetHeader::EthernetHeader(const u_char *packet){
    //Fill fields
    memcpy(this->destinationHost, packet, ETHER_ADDR_LEN);
    memcpy(this->sourceHost, packet+ETHER_ADDR_LEN, ETHER_ADDR_LEN);
    memcpy(this->type, packet+(2*ETHER_ADDR_LEN), 2);
}



