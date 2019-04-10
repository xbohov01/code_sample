/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoň xcrkon00
*/
/**
 * @file
 * @brief This is a header file with Block declarations
 * @details This file has declaration of Block base class
 * and its derived classes
 *
 * @author Samuel Bohovic xbohov01
 * @author Jakub Crkoň xcrkon00
 */


#ifndef BLOCK_H
#define BLOCK_H


#pragma once
#include <vector>
#include <string>

using namespace std;

class Block;
class BlockController;

/**
 * @class InputNode
 * @brief Class for Block input node
 * @details Defines the properties of a Block's connection port to other Blocks
 */
class InputNode
{
public:
    /** @brief Tells whether this port is connected*/
    bool isConnected = false;
    /** @brief Links connection*/
    Block *connectedTo;
    /** @brief Input value*/
    double value;
};

/**
 * @class Block
 * @brief Class for Block
 * @details Defines the properties and methods of Block base class;
 */
class Block
{
    public:
    /** @brief Houses input nodes of this block*/
    InputNode inputs[2];
    /** @brief Holds pointers to all connected blocks ports*/
    vector<InputNode*> outputs;
    /** @brief Shows whether this block was already calculated*/
    bool wasCalculated;
    /** @brief Holds calculation result for reference*/
    double result;
    /** @brief Holds this blocks identification for verification*/
    int blockId;
    /** @brief Holds this blocks type for reconstruction*/
    string blockType;
    /** @brief Holds this blocks user-made name*/
    string blockName;
    /** @brief Holds x coordinate for UI reconstruction*/
    int x;
    /** @brief Holds y coordinate for UI reconstruction*/
    int y;

    Block();
    virtual ~Block();

    int CheckInputs();

    int CanCalculate();

    virtual void Calculate();

    /** @brief Only implemented in ConstBlock */
    virtual void UpdateValue(double value);

    void PassResult();
};

/**
* @class AddBlock
* @brief Class for Addition block
* @details Blocks that adds two numbers given on inputs.
*/
class AddBlock : public Block
{
public:

    AddBlock(BlockController *ctrl);

    void Calculate();
};

/**
* @class SubBlock
* @brief Class for Substraction block
* @details Blocks that substracts two numbers given on inputs.
*/
class SubBlock : public Block
{
public:

    SubBlock(BlockController *ctrl);

    void Calculate();
};

/**
* @class MulBlock
* @brief Class for Multiplication block
* @details Blocks that multiplies two numbers given on inputs.
*/
class MulBlock : public Block
{
public:
    MulBlock(BlockController *ctrl);

    void Calculate();
};

/**
* @class DivBlock
* @brief Class for division block
* @details Blocks that divides two numbers given on inputs.
*/
class DivBlock : public Block
{
public:
    DivBlock(BlockController *ctrl);

    void Calculate();
};

/**
* @class ConstBlock
* @brief Class for a constant
* @details Set result to desired value
*/
class ConstBlock : public Block
{
public:
    ConstBlock(BlockController *ctrl);
    int CanCalculate();
    void UpdateValue(double newValue);
    void Calculate();
};

#endif // BLOCK_H
