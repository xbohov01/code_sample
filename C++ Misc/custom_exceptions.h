/*FIT VUT ICP 2017/18 
**Samuel Bohovic xbohov01
**Jakub Crkoň xcrkon00
*/
/**
 * @file
 * @brief This is a header file for including custom exceptions
 *
 * @author Samuel Bohovic xbohov01 
 * @author Jakub Crkoň xcrkon00
 */
#pragma once
#ifndef EXCEPTIONS
#define EXCEPTIONS
#include <exception>
#endif


 /// \addtogroup Exceptions
 /// Documentation for exceptions
 ///  @{

using namespace std;

/**
 * @class PortOccupiedException
 * @brief Custom exception for occupied port
 * @details This exception is throw by Block connector when given port is already occupied by another connection
 */
class PortOccupiedException : public exception
{
    const char* what() const throw()
    {
        return "Exception: Given destination port is already occupied by a connection";
    }
};

/**
 * @class LoopFoundException
 * @brief Custom exception for loop in schema
 * @details This exception is throw by Block connector when a loop is created in the schema
 */
class LoopFoundException : public exception
{
    const char* what() const throw()
    {
        return "Exception: This connection creates a loop in the schema";
    }
};

/**
 * @class NoExitPointDefinedException
 * @brief Custom exception for no schema result configuration
 * @details This exception is thrown by Schema at the start of schema calculation if 
 * result node is not connected to any block.
 */
class NoExitPointDefinedException : public exception
{
    const char* what() const throw()
    {
        return "Exception: Your schema has no defined exit point";
    }
};

/**
* @class ZeroInputException
* @brief Custom exception for when division by zero is attempted
*/
class ZeroInputException : public exception
{
	const char* what() const throw()
	{
		return "Exception: Division by zero detected";
	}
};



/// @}