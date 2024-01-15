
using FormulaEvaluator;

internal class FormulaEvaluatorTester
{
    private static void Main(string[] args)
    {
        if (Evaluator.Evaluate("5+5", null) == 10) Console.WriteLine("Happy Day!");
    }
}