//ISA 2018/19
//Class to represent IPv6 frame in packet
//Samuel Bohovic
//xbohov01

#include <netinet/in.h>
#include <arpa/inet.h>
#include <string>
#include <stdlib.h>
#include "EthernetHeader.h"

#define IP6_HEADER_LEN 8

class IPv6Header{
    public:
        char sourceIP[INET6_ADDRSTRLEN], destinationIP[INET6_ADDRSTRLEN];

        IPv6Header(const u_char *packet);
};