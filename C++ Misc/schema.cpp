/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a source file with Schema implementation
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/
#include "schema.h"

using namespace std;

/**
* @brief Constructor for Schema
*/
Schema::Schema()
{

}


/**
* @brief Destructor for Schema
*/
Schema::~Schema()
{
    for (size_t i = 0; i < this->blocks.size(); i++)
    {
        delete blocks[i];
    }
}

/**
* @brief Adds given block to the schema
* @details Adds given block to the schema, meaning that it will be calculated in the next calculation.
*/
void Schema::AddBlockToSchema(Block *block)
{
    if (block != NULL)
    {
        this->blocks.insert(blocks.end(), block);
        this->ResetSchema();
    }
}
/**
* @brief Checks if schema has a valid result connection
* @exception NoExitPointDefined
* @deprecated
*/
void Schema::CheckSchemaOutput()
{
    if (this->schemaResult.isConnected == false)
    {
        throw new NoExitPointDefinedException();
    }
}
/**
* @brief Connects block to result node
* @exception PortOccuptiedException
* @deprecated
*/
void Schema::ConnectSchemaOutput(Block *block)
{
    if (this->schemaResult.isConnected == true || this->schemaResult.connectedTo != NULL)
    {
        throw new PortOccupiedException();
    }
    else
    {
        this->schemaResult.isConnected = true;
        this->schemaResult.connectedTo = block;
        block->outputs.insert(block->outputs.end(), &this->schemaResult);
    }
}

/**
* @brief Calculates given schema
* @details Resolves entire schema at once.
* @exception LoopFoundException
*/
void Schema::CalculateSchema()
{
    this->ResetSchema();
    int maxOperations = blocks.size();
    int operationsDone = 0;

    bool didOperation = false;
    while (operationsDone < maxOperations)
    {
        for (size_t i = 0; i < this->blocks.size(); i++)
        {
            try
            {
                //try to calculate blocks
                if (blocks[i]->wasCalculated == false && blocks[i]->CanCalculate() == 0)
                {
                    operationsDone++;
                    didOperation = true;
                    blocks[i]->Calculate();
                }
            }
            catch (exception exc)
            {
                throw exc;
            }
        }
        if (didOperation == false)
        {
            throw new LoopFoundException();
        }
        else
        {
            didOperation = false;
        }
    }
    //Reset blocks
    for (size_t i = 0; i < blocks.size(); i++)
    {
        blocks[i]->wasCalculated = false;
    }
}

/**
* @brief Returns pointer to a block with given id
*/
Block* Schema::FindBlockById(int id)
{
    for (size_t i = 0; i < this->blocks.size(); i++)
    {
        if (this->blocks[i]->blockId == id)
        {
            return this->blocks[i];
        }
    }
    return NULL;
}

/**
* @brief Calculates one step of a schema
* @details Calculates one block. It being the first block that can be calculated.
* @exception LoopFoundException
*/
Block* Schema::StepThroughSchema()
{
    int maxOperations = blocks.size();
    Block *ret = NULL;

    bool didOperation = false;
    if (stepsDone < maxOperations)
    {
        for (size_t i = 0; i < this->blocks.size(); i++)
        {
            try
            {
                //try to calculate blocks
                if (blocks[i]->wasCalculated == false && blocks[i]->CanCalculate() == 0)
                {
                    stepsDone++;
                    didOperation = true;
                    blocks[i]->Calculate();
                    ret = blocks[i];
                }
            }
            catch (exception exc)
            {
                throw exc;
            }
            if (didOperation == true)
            {
                return ret;
            }
        }
        if (didOperation == false)
        {
            throw new LoopFoundException();
        }
        else
        {
            return ret;
        }
    }
    if (stepsDone == maxOperations)
    {
        this->ResetSchema();
    }
    return NULL;
}

/**
* @brief Removes given block from schema
*/
void Schema::RemoveFromSchema(Block *block)
{
    for (size_t i = 0; i < this->blocks.size(); i++)
    {
        if (this->blocks[i] == block)
        {
            blocks.erase(blocks.begin()+i);
            BlockController *ctrl = new BlockController();
            ctrl->DisconnectAll(block);
            delete ctrl;
        }
    }
}

/**
* @brief Resets all blocks for repeated calculation
*/
void Schema::ResetSchema()
{
    for (size_t i = 0; i < blocks.size(); i++)
    {
        blocks[i]->wasCalculated = false;
    }
    stepsDone = 0;
}
