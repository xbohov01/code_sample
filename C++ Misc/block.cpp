/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoň xcrkon00
*/
/**
 * @file
 * @brief This is a source file with ArtihmBlock implementation
 * @details This file has implementation of Block base class
 * and its derived classes
 *
 * @author Samuel Bohovic xbohov01
 * @author Jakub Crkoň xcrkon00
 */

#include "custom_exceptions.h"

#ifndef ARITHM
    #define ARITHM
    #include "block.h"
#endif
#include <limits>

#ifndef CONTROLLER
    #define CONTROLLER
    #include "blockcontroller.h"
#endif // !CONTROLLER

/**
 * @brief Default constructor
 * @details Sets default values for a newly created Block
 */
Block::Block()
{
    wasCalculated = false;
    inputs[0].value = numeric_limits<double>::infinity();
    inputs[1].value = numeric_limits<double>::infinity();
    this->inputs[0].connectedTo = NULL;
    this->inputs[1].connectedTo = NULL;
}

/**
* @brief Default destructor
*/
Block::~Block()
{

}

/**
 * @brief Method for checking input connections
 * @details Checks whether both inputs are connected to something.
 * This method DOES NOT verify values
 */
int Block::CheckInputs()
{
    if (inputs[0].isConnected == true && inputs[1].isConnected == true)
    {
        return 0;
    }
    else
    {
        return 1;
    }
}

/**
 * @brief Method for verifying input values
 * @details This method checks whether a calculation on this block can be done.
 * First it checks if both inputs are connected using CheckInputs
 * @see {CheckInputs}
 */
int Block::CanCalculate()
{
    if (this->CheckInputs() != 0)
    {
        return 1;
    }
    else
    {
        if (inputs[0].value != numeric_limits<double>::infinity() && inputs[1].value != numeric_limits<double>::infinity())
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}

/** @brief Overloaded in ConstBlock */
void Block::UpdateValue(double value)
{
    return;
}

/**
 * @brief Method for calculating result
 * @details This method is a place holder for overloaded calculations for derived classes.
 * It does not execute any further checks as it is expected that all required values are correct.
 */
void Block::Calculate()
{

}

/**
 * @brief Method for passing the result.
 * @details This method passes the result to all blocks connected to this one downwards.
 * It iterates over the connected blocks and sets the connected port to desired value.
 */
void Block::PassResult()
{
    //EXCEPTION
    if (outputs.size() == 0)
    {
        return;
    }
    //iterate over connections
    for (size_t i = 0; i < outputs.size(); i++)
    {
        this->outputs[i]->value = result;
    }
}


/**
* @brief AddBlock constructor
* @details Sets default values for a newly created AddBlock
* @param ctrl Current block controller to generate new block id
*/
AddBlock::AddBlock(BlockController *ctrl) : Block()
{
    this->blockId = ctrl->NewBlockId();
    this->blockType = "add";
}

/**
* @brief Overloaded method for addtion
*/
void AddBlock::Calculate()
{
    result = this->inputs[0].value + this->inputs[1].value;
    this->wasCalculated = true;
    this->PassResult();
}

/**
* @brief SubBlock constructor
* @details Sets default values for a newly created SubBlock
* @param ctrl Current block controller to generate new block id
*/
SubBlock::SubBlock(BlockController *ctrl) : Block()
{
    this->blockId = ctrl->NewBlockId();
    this->blockType = "sub";
}

/**
* @brief Overloaded method for substraction
*/
void SubBlock::Calculate()
{
    result = this->inputs[0].value - this->inputs[1].value;
    this->wasCalculated = true;
    this->PassResult();
}

/**
* @brief MulBlock constructor
* @details Sets default values for a newly created MulBlock
* @param ctrl Current block controller to generate new block id
*/
MulBlock::MulBlock(BlockController *ctrl) : Block()
{
    this->blockId = ctrl->NewBlockId();
    this->blockType = "mul";
}

/**
* @brief Overloaded method for multiplication
*/
void MulBlock::Calculate()
{
    result = this->inputs[0].value * this->inputs[1].value;
    this->wasCalculated = true;
    this->PassResult();
}

/**
* @brief DivBlock constructor
* @details Sets default values for a newly created DivBlock
* @param ctrl Current block controller to generate new block id
*/
DivBlock::DivBlock(BlockController *ctrl) : Block()
{
    this->blockId = ctrl->NewBlockId();
    this->blockType = "div";
}

/**
* @brief Overloaded method for multiplication
* @exception ZeroInputException
*/
void DivBlock::Calculate()
{
    if (this->inputs[1].value == 0)
    {
        throw new ZeroInputException();
    }
    result = this->inputs[0].value / this->inputs[1].value;
    this->wasCalculated = true;
    this->PassResult();
}

/**
* @brief ConstBlock constructor
*/
ConstBlock::ConstBlock(BlockController *ctrl)
{
    this->blockId = ctrl->NewBlockId();
    this->inputs[0].isConnected = true;
    this->inputs[0].connectedTo = NULL;
    this->inputs[0].value = 0;
    this->inputs[1].isConnected = true;
    this->inputs[1].connectedTo = NULL;
    this->inputs[1].value = 0;
    this->blockType = "const";
    this->result = 0.f;
}

/**
* @brief Overloaded check, constant can always be "calculated"
*/
int ConstBlock::CanCalculate()
{
    return 0;
}

/**
* @brief Overloaded method, only passes given value
*/
void ConstBlock::Calculate()
{
    this->wasCalculated = true;
    this->PassResult();
}

/**
* @brief Method that updates the value of a constant value block
*/
void ConstBlock::UpdateValue(double newValue)
{
    this->result = newValue;
    this->PassResult();
}
