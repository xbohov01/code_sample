/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a header file with Schema declarations
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/
#pragma once
#include "block.h"
#include "blockcontroller.h"
#include "custom_exceptions.h"
using namespace std;

/**
* @class Schema
* @brief Class for entire block schema
* @details Derived from Block this class implements calculation for adding two numbers
*/
class Schema
{
public:
	/** @brief Vector of all blocks*/
	vector<Block*>blocks;
	/** @brief Connection for results*/
	InputNode schemaResult;
    /** @brief Steps taken when stepping through a schema*/
    int stepsDone;

	Schema();

	~Schema();

	void AddBlockToSchema(Block *block);

	void CheckSchemaOutput();

	void ConnectSchemaOutput(Block *block);

	void CalculateSchema();

    Block* StepThroughSchema();

	void RemoveFromSchema(Block *block);

    Block* FindBlockById(int id);

    void ResetSchema();
};
