/*FIT VUT ICP 2017/18
**Samuel Bohovic xbohov01
**Jakub Crkoň xcrkon00
*/
/**
* @file
* @brief This is a header file for BlockController class
*
* @author Samuel Bohovic xbohov01
* @author Jakub Crkoň xcrkon00
*/
#pragma once
#include "block.h"

/**
 * @class BlockController
 * @brief Class for BlockController
 * @details Defines the properties and methods of BlockController. 
 * This class provides oversight, controll and verification options for ArithmBlocks inside shemas.
 */
class BlockController
{
    private:
        /** @brief Counter for block identificatiors*/
        int blockCounter;
    public:
        
		~BlockController();

        BlockController();

        int NewBlockId();

	    void ConnectBlocks(Block *destination, Block *source, int port);

		void ConnectConstant(Block *destination, ConstBlock *source, int port);

        void DisconnectBlockByPort(Block *toDisconnect, int port);

        void DisconnectAll(Block *block);
};
