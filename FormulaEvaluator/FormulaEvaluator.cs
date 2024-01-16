namespace FormulaEvaluator
{
    using System.Numerics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 
    /// </summary>
    public static class Evaluator
    {
        private static Stack<String> operatorStack = new Stack<string>();
        private static Stack<int> valueStack = new Stack<int>();
        public delegate int Lookup(String variable_name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression.Trim(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (string substring in substrings)
            {
                if (int.TryParse(substring, out int result))
                {
                    valPush(result);
                }                   
                // this else if statement will assert that the given string is a variable, as the initial if statement shows that it is not an integer, and all variables require a length of at least 2. 
                else if (substring.Length > 1)
                {
                    valPush(variableEvaluator(substring));
                }
                else if (substring == "+" || substring == "-" || substring == "*" || substring == "/")
                {
                    if (substring =="+" || substring == "-" )
                    {
                        opPop();
                    }
                    operatorStack.Push(substring);
                }
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
                        operatorStack.Pop();
                        //step 3
                        if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            int temp1 = valueStack.Pop();
                            if (operatorStack.Pop() == "*")
                            {
                                valueStack.Push(valueStack.Pop() * temp1);
                            }
                            else if(temp1 != 0)
                            {
                                valueStack.Push(valueStack.Pop() / temp1);
                            }
                        }
                    }
                }
            }
            if (operatorStack.Count != 0)
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
            // end result
            return valueStack.Pop();
        }
        private static void valPush(int input)
        {
            if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                if (operatorStack.Pop() == "*")
                {
                    valueStack.Push(valueStack.Pop() * input);
                }
                else if (input != 0)
                {
                    valueStack.Push(valueStack.Pop() / input);
                }
            }
            else
            {
                valueStack.Push(input);
            }
        }
        private static void opPop() {
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
    }
   
}
