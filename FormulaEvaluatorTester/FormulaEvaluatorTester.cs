/// <summary>
/// Author:    [Your Name]
/// Partner:   [Partner Name or None]
/// Date:      [Date of Creation]
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and [Your Name(s)] - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, [your name], certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    [... and of course you should describe the contents of the 
///    file in broad terms here ...]
/// </summary>
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
        try
        {
            Evaluator.Evaluate("5/0", null);
        }catch (Exception ex)
        {
            if(ex.GetType() == typeof(ArgumentException)) {
                Console.WriteLine("Zero Exception Works");
            }
        }
        if (Evaluator.Evaluate("5+5*2", null) == 15) Console.WriteLine("PEMDAS success");
        if (Evaluator.Evaluate("5-5+(5/5)*5", null) == 5) Console.WriteLine("Ultimate PEMDAS success");
        if (Evaluator.Evaluate("X1+X1", (x) => 5) == 10) Console.WriteLine("Bananas");
    }
}