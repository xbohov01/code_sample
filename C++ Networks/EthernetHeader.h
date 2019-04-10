//ISA 2018/19
//Class to represent ethernet frame in packet
//Samuel Bohovic
//xbohov01

#pragma once
#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#define ETHER_ADDR_LEN	6
#define ETHER_HEADER_LEN 14

class EthernetHeader{
    public:
        u_char destinationHost[ETHER_ADDR_LEN];
        u_char sourceHost[ETHER_ADDR_LEN];
        u_char type[2];

        EthernetHeader(const u_char *packet);
};