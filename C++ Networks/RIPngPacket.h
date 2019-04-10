//ISA 2018/19
//Class to represent RIPng packet
//Samuel Bohovic
//xbohov01

#include <netinet/in.h>
#include <arpa/inet.h>
#include <string>
#include <stdlib.h>
#include <ctype.h>
#include <stdio.h>
#include <unistd.h>
#include "string.h"
#include <string>
#include <iostream>

#define RIPNG_HEADER_LEN 4
#define RIPNG_RT_ENTRY_LEN 20
#define RIPNG_NH_ENTRY_LEN 20

class RIPngPacket{    
    public:
        char *packet;
        int len;

        //Constructs header and Route table entry
        RIPngPacket(char prefix[32], char routeTag[2], char *prefixLen, char *metric, char nextHop[32]);

        //Hexadecimal converter
        int HexCharToInteger(char ch);
};