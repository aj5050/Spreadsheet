/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      1-11-2024
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
///     This file contains the FormulaEvaluator library, as well as the evaluator class and evaluator method along with it's private helper methods
/// 
/// </summary>
namespace FormulaEvaluator
{
    using System.Data.SqlTypes;
    using System.Numerics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This class contains methods for evaluating mathematical expressions that are in the form of strings. It uses 2 stacks: an operator stack for the
    /// operators and parentheses, and a value stack for all of the numbers. It also contains a delegate method that handles variables in the expression.
    /// </summary>
    public static class Evaluator
    {
        private static Stack<String> operatorStack = new Stack<string>();
        private static Stack<int> valueStack = new Stack<int>();
        /// <summary>
        /// This delegate finds the value associated with the given variable string.
        /// </summary>
        /// <param name="variable_name"> the variable that is being passed in in exchange for a integer value </param>
        /// <returns> this method returns the given integer associated with the variable if the variable exists, and if not it throws an exception 
        /// </returns>
        public delegate int Lookup(String variable_name);
        /// <summary>
        /// This method takes in an expression and returns the resulting value. It uses a for-each loop to process every token in the expression, as long as
        /// the token is acceptable (no words, variables are characters with numbers attached to the end). It also follows the order of operations.
        /// </summary>
        /// <param name="expression"> the string expression that is being passed in to be evaluated </param>
        /// <param name="variableEvaluator"> the delegate that is being passed in to find values associated with a given variable </param>
        /// <exception cref="ArgumentException"> This exception stops the computer from diving by zero </exception>
        /// <returns> the overall value of the expression (if there is one) </returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            operatorStack = new Stack<string>();
            valueStack = new Stack<int>();
            string[] substrings = Regex.Split(expression.Trim(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (string substring in substrings)
            {
                if (substring == "")
                {

                }
                // this if statement asserts if the substring is an integer or not, and if it is, it calls the valPush helper method. 
                else if (int.TryParse(substring, out int result))
                {
                    valPush(result);
                }
                // this else if statement will assert that the given string is a variable, as the initial if statement shows that it is not an integer,
                // and all variables require a length of at least 2. 
                else if (isVariable(substring))
                {
                    valPush(variableEvaluator(substring));
                }
                // this else if statement asserts that the substring is an operator, and if it is, then it either calls the opPop helper method
                // (for the + and - operators) or pushes the operator onto the operatorStack
                else if (substring == "+" || substring == "-" || substring == "*" || substring == "/")
                {
                    if (substring == "+" || substring == "-")
                    {
                        opPop();
                    }
                    operatorStack.Push(substring);
                }
                // this else if statement asserts that the substring is a parentheses, and then pushes it onto the operator stack if its the opening (
                // or works out the rest of the expression that's inside the parentheses if the token is the closing )   
                else if (substring == "(" || substring == ")")
                {
                    if (substring == "(")
                    {
                        operatorStack.Push(substring);
                    }
                    else
                    {
                        //step one
                        opPop();
                        //step 2
                        if(operatorStack.Count > 0)
                        {
                            operatorStack.Pop();
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Expression");
                        }
                        
                        //step 3
                        if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            int temp1 = valueStack.Pop();
                            if (operatorStack.Pop() == "*")
                            {
                                valueStack.Push(valueStack.Pop() * temp1);
                            }
                            else if (temp1 == 0)
                            {
                                throw new ArgumentException("Cannot Divide By 0");
                            }
                            else
                            {
                                valueStack.Push(valueStack.Pop() / temp1);
                            }
                        }
                    }
                }
            }
            if (operatorStack.Count != 0 && valueStack.Count >= 2)
            {
                if (operatorStack.Pop() == "+")
                {
                    valueStack.Push(valueStack.Pop() + valueStack.Pop());
                }
                else
                {
                    int temp1 = valueStack.Pop();
                    valueStack.Push(valueStack.Pop() - temp1);
                }
            }else if( valueStack.Count < 2 && operatorStack.Count!=0 )
            {
                throw new ArgumentException("Invalid Expression");
            }
            if( operatorStack.Count == 0 &&  valueStack.Count >1 ) {
                throw new ArgumentException("Invalid Expression");
            }
            //else
            //{
            //    throw new ArgumentException("Invalid Expression");
            //}
            //throw exception for empty valueStack
            if (valueStack.Count == 0)
            {
                throw new ArgumentException("the final result should be in the stack");
            }
            // end result
            return valueStack.Pop();
        }
        /// <summary>
        /// This method is used twice in the Evaluate method, so it was put in a private helper method to make the code shorter.
        /// </summary>
        /// <param name="input"> an integer value that is being operated on or pushed into the value stack </param>
        /// <exception cref="ArgumentException"> This exception stops the computer from diving by zero </exception>
        private static void valPush(int input)
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
        private static void opPop()
        {
            if (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && valueStack.Count >= 2)
            {
                if (operatorStack.Pop() == "+")
                {
                    valueStack.Push(valueStack.Pop() + valueStack.Pop());
                }
                else
                {
                    int temp1 = valueStack.Pop();
                    valueStack.Push(valueStack.Pop() - temp1);
                }
            }
            

        }
        /// <summary>
        /// This method checks if the given string qualifies as a variable
        /// </summary>
        /// <param name="potentialVar"> The string that is being evaluated as a variable </param>
        /// <returns> A true/false statement based on whether the string qualifies as a variable </returns>
        private static bool isVariable(string potentialVar)
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
