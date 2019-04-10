/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a source file with SchemaSaver implementation
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/

#include "SchemaSaver.h"

using namespace std;

/**
* @brief Constructor
*/
SchemaSaver::SchemaSaver()
{

}

SchemaSaver::~SchemaSaver()
{

}

/**
* @brief Parses input nodes
* @details Creates a string to be written into save file from the current input nodes.
*/
string SchemaSaver::InputParser(Block *block)
{
    string output = "";
    for (int i = 0; i < 2; i++)
    {
        //block is a constant
        if (block->blockType == "const")
        {
            output.append(to_string(block->result));
        }
        //in not connected
        else if (block->inputs[i].connectedTo == NULL && block->inputs[i].isConnected == false)
        {
            output.append("null");
        }
        //in has block
        else if (block->inputs[i].connectedTo != NULL && block->inputs[i].isConnected == true)
        {
            output.append(to_string(block->inputs[i].connectedTo->blockId));
        }
        if (i == 0)
        {
            output.append(",");
        }
    }

    return output;
}

/**
* @brief Creates schema save file
* @details Parses current schema into a .csv file
*/
void SchemaSaver::SaveSchema(string path, Schema *schema)
{
    ofstream outFile;
    try
    {
        outFile.open(path);
    }
    catch (exception exc)
    {
        throw exc;
    }

    int blockCount = schema->blocks.size();

    outFile << blockCount << "\n";

    Block *current;

    for (int i = 0; i < blockCount; i++)
    {
        current = schema->blocks[i];
        outFile << current->blockId << ",";
        outFile << current->blockType << ",";
        outFile << current->x << ",";
        outFile << current->y << ",";
        outFile << this->InputParser(current);
        outFile << "\n";
    }

    outFile.close();
}
