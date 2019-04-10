/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoò xcrkon00
*/
/**
* @file
* @brief This is a source file with SchemaLoader implementation
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoò xcrkon00
*/

#include "SchemaLoader.h"

/**
* @brief Constructor
*/
SchemaLoader::SchemaLoader()
{

};

/**
* @brief Destructor
*/
SchemaLoader::~SchemaLoader()
{

};

/**
* @brief Constructs connection info
*/
ConnectionInfo::ConnectionInfo(int destination, int source, int port)
{
	this->destination = destination;
	this->source = source;
	this->port = port;
}

/**
* @brief Splits string by delimiter
*/
string SchemaLoader::SplitLine(string line, int order)
{
	string res = "";
	int index = 1;
	for (string::size_type i = 0; i < line.size(); ++i) {
		if (line[i] == ',' && index == order)
		{
			return res;
		}
		else if (line[i] == ',')
		{
			index++;
			res = "";
		}
		else
		{
			res.append(1, line[i]);
		}
	}
	return res;
}

/**
* @brief Loads schema
* @details Reads given file and creates a schema based on it's contents
*/

Schema * SchemaLoader::LoadSchema(string path, BlockController *controller)
{
	ifstream inFile;
    int blockCount, x, y;
	string line;
    string split;
    Schema *loadedSchema = new Schema();
	Block *block;
    vector<ConnectionInfo*> connections;
    ConnectionInfo *connection;

	//open file
	try
	{
        inFile.open(path);
	}
	catch (exception exc)
	{
		throw exc;
	}

	//first line with number of blocks
	getline(inFile, line);

	//check if number is valid
	try
	{
		blockCount = stoi(line);
	}
	catch (exception exc)
	{
		throw exc;
	}

	for (int i = 0; i < blockCount; i++)
	{
		//get block info
		getline(inFile, line);
        int id = stoi(this->SplitLine(line, 1));
        string operation = this->SplitLine(line, 2);
        x = stoi(this->SplitLine(line, 3));
        y = stoi(this->SplitLine(line, 4));

		//set block info
		if (operation == "add")
		{
            block = new AddBlock(controller);
		}
		else if (operation == "sub")
		{
            block = new SubBlock(controller);
		}
		else if (operation == "mul")
		{
            block = new MulBlock(controller);
		}
		else if (operation == "div")
		{
            block = new DivBlock(controller);
		}
		else if (operation == "const")
		{
            block = new ConstBlock(controller);
            block->result = stod(this->SplitLine(line, 5));
		}
		else
		{
			//TODO throw exception
            return NULL;
		}
		
        block->blockId = id;
		block->blockType = operation;
        block->x = x;
        block->y = y;

		//add block to schema
        loadedSchema->AddBlockToSchema(block);

        //log connections to connect later
        string in1Id = this->SplitLine(line, 5);
        string in2Id = this->SplitLine(line, 6);

        if (in1Id != "null" && operation != "const")
        {
            connection = new ConnectionInfo(id,stoi(in1Id), 0);
            connections.insert(connections.end(), connection);
        }
        if (in2Id != "null" && operation != "const")
        {
            connection = new ConnectionInfo(id,stoi(in2Id), 1);
            connections.insert(connections.end(), connection);
        }
		
	}

	//create connections
	for (size_t i = 0; i < connections.size(); i++)
	{
        Block *destination = loadedSchema->FindBlockById(connections[i]->destination);
        Block *source = loadedSchema->FindBlockById(connections[i]->source);
        controller->ConnectBlocks(destination, source, connections[i]->port);
    }

    return loadedSchema;
}

