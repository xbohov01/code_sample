/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoň xcrkon00
*/
/**
* @file
* @brief This is a header file with SchemaLoader declarations
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoň xcrkon00
*/
#pragma once
#include "schema.h"
#include <string>
#include "block.h"
#include "blockcontroller.h"
#include "schema.h"
#include <stdio.h>
#include <iostream>
#include <fstream>

using namespace std;

/**
* @class ConnectionInfo
* @brief Class to store connection information during loading
* @details Stored information about connection between blocks (destination, source)
*/
class ConnectionInfo
{
public:
    /** @brief Destination block ID*/
    int destination;
    /** @brief Source block ID*/
    int source;
    /** @brief Port that is connected*/
    int port;

    ConnectionInfo(int destination, int source, int port);
};

/**
* @class SchemaLoader
* @brief Class for loading schemas
*/
class SchemaLoader
{
public:

    SchemaLoader();

    ~SchemaLoader();

    string SplitLine(string line, int order);

    Schema * LoadSchema(string path, BlockController *controller);
};
