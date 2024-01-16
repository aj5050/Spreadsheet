
using FormulaEvaluator;

internal class FormulaEvaluatorTester
{
    private static void Main(string[] args)
    {
        if (Evaluator.Evaluate("5+5", null) == 10) Console.WriteLine("Add Works");
        if (Evaluator.Evaluate("5-5", null) == 0) Console.WriteLine("Subtract Works");
        if (Evaluator.Evaluate("5*5", null) == 25) Console.WriteLine("Multiplication Works");
        if (Evaluator.Evaluate("5/5", null) == 1) Console.WriteLine("Division Works");
        if (Evaluator.Evaluate("(5+5)+5", null) == 15) Console.WriteLine("Basic Parentheses Works");
        if (Evaluator.Evaluate("(25/5)+5", null) == 10) Console.WriteLine("Advanced Parentheses Works");
        if (Evaluator.Evaluate("5/(25/5)+5", null) == 6) Console.WriteLine("Professional Parentheses Works");
        if (Evaluator.Evaluate("5+(25/5)/5", null) == 6) Console.WriteLine("Switched Operations Works");
        if (Evaluator.Evaluate("5+((25/5)/5)", null) == 6) Console.WriteLine("Nested Parentheses Works");
        if (Evaluator.Evaluate("5/0", null) == 5) Console.WriteLine("Division by 0 is wrong");
        if (Evaluator.Evaluate("5+5*2", null) == 15) Console.WriteLine("PEMDAS success");
        if (Evaluator.Evaluate("5-5+(5/5)*5", null) == 5) Console.WriteLine("Ultimate PEMDAS success");
    }
}