namespace FormulaEvaluator
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// 
    /// </summary>
    public static class Evaluator
    {
        private static Stack<String> operatorStack;
        private static Stack<int> valueStack;
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (string substring in substrings)
            {
                if (int.TryParse(substring, out int result))
                {
                    if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (operatorStack.Pop() == "*")
                        {
                            valueStack.Push(valueStack.Pop() * result);
                        }
                        else
                        {
                            valueStack.Push(valueStack.Pop() / result);
                        }
                    }
                    else
                    {
                        valueStack.Push(result);
                    }
                    /// this else if statement will assert that the given string is a variable, as the initial if statement shows that it is not an integer, and all variables require a length of at least 2. 
                }
                else if (substring.Length > 1)
                {
                    if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (operatorStack.Pop() == "*")
                        {
                            /// the variableEvaluator should throw an exception if the variable cannot be found
                            valueStack.Push(valueStack.Pop() * variableEvaluator(substring));
                        }
                        else
                        {
                            valueStack.Push(valueStack.Pop() / variableEvaluator(substring));
                        }
                    }
                    else
                    {
                        valueStack.Push(variableEvaluator(substring));
                    }
                }
                else if (substring == "+" || substring == "-" || substring == "*" || substring == "/")
                {
                    if ((operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && valueStack.Count >= 2)
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
                        ///step one
                        if ((operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && valueStack.Count >= 2)
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
                        ///step 2
                        operatorStack.Pop();
                        ///step 3
                        if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            int temp1 = valueStack.Pop();
                            if (operatorStack.Pop() == "*")
                            {
                                valueStack.Push(valueStack.Pop() * temp1);
                            }
                            else
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
    }
}
