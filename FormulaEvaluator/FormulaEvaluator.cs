using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
public static class Evaluator{
    private static Stack<String> operatorStack;
    private static Stack<int> valueStack;
    public delegate int Lookup(String variable_name);

    public static int Evaluate(String expression, Lookup variableEvaluator)
    {
        string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
        foreach (string substring in substrings)
        {
            if(int.TryParse(substring,out int result))
            {
                if(operatorStack.Count != 0 && valueStack.Count!=0 && (operatorStack.Peek() == "*" || operatorStack.Peek()=="/"))
                {
                    if (operatorStack.Peek() == "*")
                    {
                        valueStack.Push(valueStack.Pop() * result);
                    }
                    else
                    {
                        valueStack.Push(valueStack.Pop()/result);
                    }
                }
                else
                {
                    valueStack.Push(result);
                }
                /// this else if statement will assert that the given string is a variable, as the initial if statement shows that it is not an integer, and all variables require a length of at least 2. 
            }else if (substring.Length > 1)
            {
                if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                {
                    if (operatorStack.Peek() == "*")
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
        }
        // TODO...
        return 0;
    }
}