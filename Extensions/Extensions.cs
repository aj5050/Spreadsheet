using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      1-23-2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Austin January - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Austin January, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///    This file contains private helper methods for the projects in the "Spreadsheet" solution
/// 
/// </summary>
namespace Extensions
{
    /// <summary>
    /// This class will contain private helper methods for the projects in the "Spreadsheet" solution.
    /// </summary>
    public class Extensions
    {
        /// <summary>
        /// This method is used twice in the Evaluate method, so it was put in a private helper method to make the code shorter.
        /// </summary>
        /// <param name="input"> an integer value that is being operated on or pushed into the value stack </param>
        /// <exception cref="ArgumentException"> This exception stops the computer from diving by zero </exception>
        public static void valPush(double input, Stack<string> operatorStack, Stack<double> valueStack)
        {
            if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                if (operatorStack.Pop() == "*")
                {
                    valueStack.Push(valueStack.Pop() * input);
                }
                else
                {
                    valueStack.Push(valueStack.Pop() / input);
                }
            }
            else
            {
                valueStack.Push(input);
            }
        }
        /// <summary>
        /// This method pops the operator stack and uses that operator to add or subtract the top two integers of the value stack.
        /// </summary>
        public static void opPop(Stack<string> operatorStack, Stack<double> valueStack)
        {
            if (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && valueStack.Count >= 2)
            {
                if (operatorStack.Pop() == "+")
                {
                    valueStack.Push(valueStack.Pop() + valueStack.Pop());
                }
                else
                {
                    double temp1 = valueStack.Pop();
                    valueStack.Push(valueStack.Pop() - temp1);
                }
            }


        }

        /// <summary>
        /// This method checks if the given token qualifies as a variable
        /// </summary>
        /// <param name="token"> The token that is being evaluated as a variable </param>
        /// <returns>A true/false statement based on whether the token qualifies as a variable </returns>
        public static bool isTokenVariable(string token)
        {
            if (!double.TryParse(token, out double value) && Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method checks if the given token qualifies as an operator
        /// </summary>
        /// <param name="token"> the token that is being evaluated as an operator </param>
        /// <returns> A true/false statement based on whether or not the token qualifies as an operator </returns>
        public static bool isTokenOperator(string token)
        {
            if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
            {
                return true;
            }
            return false;
        }
        public static bool isValidCell(string name)
        {
            if(Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
            {
                return true;
            }
            return false;
        }
    }
}
