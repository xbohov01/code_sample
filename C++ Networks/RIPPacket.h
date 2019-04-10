//ISA 2018/19
//Class to represent ethernet RIP part of packet
//Samuel Bohovic
//xbohov01

#include "RIPPayload.h"
#include <list>

#define RIP_HEADER_LEN 4
#define RIP_PAYLOAD_LEN 20

using namespace std;

class RIPPacket{
    public:
        int version;
        char password[16];
        list<RIPPayload*> payloads;
};