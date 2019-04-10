//ISA 2018/19
//Class to represent IPv4 frame in packet
//Samuel Bohovic
//xbohov01

#include <netinet/in.h>
#include <arpa/inet.h>
#include <string>
#include <stdlib.h>
#include "EthernetHeader.h"

#define IP4_HEADER_LEN 20

class IPv4Header{
    public:
        char sourceIP[INET_ADDRSTRLEN], destinationIP[INET_ADDRSTRLEN];

        IPv4Header(const u_char *packet);
};