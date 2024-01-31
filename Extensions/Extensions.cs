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
                else if (input == 0)
                {
                    throw new ArgumentException("Cannot Divide By 0");
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
        /// This method checks if the given string qualifies as a variable
        /// </summary>
        /// <param name="potentialVar"> The string that is being evaluated as a variable </param>
        /// <returns> A true/false statement based on whether the string qualifies as a variable </returns>
        public static bool isVariable(string potentialVar)
        {
            if (potentialVar.Length > 1)
            {
                Char varStart = potentialVar.ToCharArray()[0];
                bool intExists = int.TryParse(potentialVar.ToCharArray().Last().ToString(), out int varTypeEnd);
                if (char.IsLetter(varStart) && intExists)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
