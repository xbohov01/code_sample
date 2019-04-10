/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a source file with BlockController implementation
* @details Contains all BlockController methods
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/

#include "blockcontroller.h"
#include "custom_exceptions.h"
#include <algorithm>

/**
* @brief Default constructor
* @details Sets default values for a newly created BlockController
*/
BlockController::BlockController()
{
    blockCounter = 0;
}


BlockController::~BlockController()
{
    blockCounter = 0;
}

/**
* @brief Generates new identificator
* @details Assigns given block a new unique identification
*/
int BlockController::NewBlockId()
{
    return blockCounter++;
}

/**
* @brief Connects two blocks
* @details Creates a connection between two blocks on given port
* @exception PortOccuptiedException
* @exception LoopFoundException
*/
void BlockController::ConnectBlocks(Block *destination, Block *source, int port)
{
    //CHECK IF DEST HAS FREE PORTS
	if (destination->inputs[port].isConnected == true)
	{
		throw new PortOccupiedException();
	}
	//Check if trying to connect to itself
	if (destination == source)
	{
		throw new LoopFoundException();
	}
	if (destination->blockType == "const")
	{
		//TODO EXCEPTION
	}
	//connect dest
	destination->inputs[port].isConnected = true;
	destination->inputs[port].connectedTo = source;
	//connect src
	source->outputs.insert(source->outputs.end(), &destination->inputs[port]);
}

/**
* @brief Connects block to a constant
* @exception PortOccuptiedException
*/
void BlockController::ConnectConstant(Block *destination, ConstBlock *source, int port)
{
	if (destination->inputs[port].isConnected == true)
	{
		throw new PortOccupiedException();
	}
	destination->inputs[port].isConnected = true;
	destination->inputs[port].connectedTo = source;
	source->outputs.insert(source->outputs.end(), &destination->inputs[port]);
	source->PassResult();
}

/**
* @brief Disconnect blocks
* @details Disconnects given port from it's connection and erases the possible input
*/
void BlockController::DisconnectBlockByPort(Block *toDisconnect, int port)
{
	Block *upwards = toDisconnect->inputs[port].connectedTo;
	if (upwards != NULL)
	{
		upwards->outputs.erase(remove(upwards->outputs.begin(), upwards->outputs.end(), &toDisconnect->inputs[port]));
	}
	toDisconnect->inputs[port].connectedTo = NULL;
	toDisconnect->inputs[port].isConnected = false;
	toDisconnect->inputs[port].value = numeric_limits<double>::infinity();
} 

/**
* @brief Disconnects a given block completely
*/
void BlockController::DisconnectAll(Block *block)
{
    Block *upwards = block->inputs[0].connectedTo;
    if (upwards != NULL)
    {
        upwards->outputs.erase(remove(upwards->outputs.begin(), upwards->outputs.end(), &block->inputs[0]));
    }
    block->inputs[0].connectedTo = NULL;
    block->inputs[0].isConnected = false;
    block->inputs[0].value = numeric_limits<double>::infinity();

    upwards = block->inputs[1].connectedTo;
    if (upwards != NULL)
    {
        upwards->outputs.erase(remove(upwards->outputs.begin(), upwards->outputs.end(), &block->inputs[1]));
    }
    block->inputs[1].connectedTo = NULL;
    block->inputs[1].isConnected = false;
    block->inputs[1].value = numeric_limits<double>::infinity();

    for (size_t i = 0; i < block->outputs.size(); i++)
    {
        InputNode *node = block->outputs[i];
        for (int j = 0; j < 2; j++)
        {
            node->connectedTo = NULL;
            node->isConnected = false;
            node->value = numeric_limits<double>::infinity();
        }
    }
}
