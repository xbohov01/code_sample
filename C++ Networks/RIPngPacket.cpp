//ISA 2018/19
//Class to represent RIPng packet
//Samuel Bohovic
//xbohov01

#include "RIPngPacket.h"

using namespace std;

int RIPngPacket::HexCharToInteger(char ch)
{
    if (ch >= '0' && ch <= '9')
        return ch - '0';
    if (ch >= 'A' && ch <= 'F')
        return ch - 'A' + 10;
    if (ch >= 'a' && ch <= 'f')
        return ch - 'a' + 10;
    return -1;
}

RIPngPacket::RIPngPacket(char prefix[32], char routeTag[2], char *prefixLen, char *metric, char nexthop[32]){
    bool nextHopFlag = false;

    //Allocate proper amount of space
    if (strcmp(nexthop, "::") == 0){
        this->packet = (char*)malloc(RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN);
        memset(this->packet, '\0', RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN);
        this->len = RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN;
    } else {
        printf("here\n");
        this->packet = (char*)malloc(RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN+RIPNG_NH_ENTRY_LEN);
        memset(this->packet, '\0', RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN+RIPNG_NH_ENTRY_LEN);
        nextHopFlag = true;
        this->len = RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN+RIPNG_NH_ENTRY_LEN;
    }

    //Create header
    //Command
    memset(&packet[0], 2, 1);
    //Version
    memset(&packet[1], 1, 1);
    //Reserved MBZ
    memset(&packet[2], 0, 1);
    memset(&packet[3], 0, 1);

    //Create RTE
    //Prefix 
    int destCounter = 0;
    for(int c = 0; c < 32; c+=2){
        char digit[3];
        digit[0]=prefix[c];
        digit[1]=prefix[c+1];
        digit[2]='\0';
        int digitInt = stoi(digit, NULL, 16);
        unsigned short digitShort = (unsigned short)digitInt;
        digitShort = abs(digitShort);
        packet[4+destCounter] = digitShort;
        destCounter++;
    }

    //Route Tag
    int routerTag = strtol(routeTag, NULL, 10);
    short routerTagShort = (short)routerTag;
    memcpy(&packet[20], &routerTagShort, 2);
    
    //Prefix len
    int prefLen = strtol(prefixLen, NULL, 10);
    short prefLenShort = (short)prefLen;
    memcpy(&packet[22], &prefLenShort, 1);
    
    //Metric
    int metricInt = strtol(metric, NULL, 10);
    short metricShort = (short)metricInt;
    memcpy(&packet[23], &metricShort, 1);

    //Append next hop if needed
    if (nextHopFlag == true){
        //Address
        int destCounter = 0;
        for(int c = 0; c < 32; c+=2){
            char digit[3];
            digit[0]=nexthop[c];
            digit[1]=nexthop[c+1];
            digit[2]='\0';
            int digitInt = stoi(digit, NULL, 16);
            unsigned short digitShort = (unsigned short)digitInt;
            digitShort = abs(digitShort);
            packet[RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN+destCounter] = digitShort;
            destCounter++;
        }
        //Metric field
        packet[RIPNG_HEADER_LEN+RIPNG_RT_ENTRY_LEN+RIPNG_NH_ENTRY_LEN-1] = 0xff;
    }

    return;
}
