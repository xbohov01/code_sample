/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a header file with SchemaSaver declarations
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/

#pragma once
#include <string>
#include "schema.h"
#include "block.h"
#include <stdio.h>
#include <iostream>
#include <fstream>

using namespace std;

/**
* @class SchemaSaver
* @brief Class for saving schemas
*/
class SchemaSaver
{
public:
	SchemaSaver();

	~SchemaSaver();
	
	string InputParser(Block *block);

	void SaveSchema(string path, Schema *schema);
};

