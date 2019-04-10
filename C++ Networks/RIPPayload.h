//ISA 2018/19
//Class to represent ethernet RIP payload slot
//Samuel Bohovic
//xbohov01

#pragma once
#include <netinet/in.h>
#include <arpa/inet.h>

class RIPPayload{
    public:
        int addressFamily;
        int routeTag;
        char address[INET_ADDRSTRLEN];
        char netmask[INET_ADDRSTRLEN];
        char nexthop[INET_ADDRSTRLEN];
        int metric;
};