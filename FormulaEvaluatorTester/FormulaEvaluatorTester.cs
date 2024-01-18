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
///    This file contains a main function that is used to test the Evaluator Class from the FormulaEvaluator library
///    
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
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Zero Exception Works");
            }
        }
        if (Evaluator.Evaluate("5+5*2", null) == 15) Console.WriteLine("PEMDAS success");
        if (Evaluator.Evaluate("5-5+(5/5)*5", null) == 5) Console.WriteLine("Ultimate PEMDAS success");
        if (Evaluator.Evaluate("X1+X1", (x) => 5) == 10) Console.WriteLine("Variable Success");
        try
        {
            Evaluator.Evaluate("-A-", null);
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("-5", null) == 5)
            {
                Console.WriteLine("subtract statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("+5", null) == 5)
            {
                Console.WriteLine("addition statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("*5", null) == 5)
            {
                Console.WriteLine("multiplication statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("/5", null) == 5)
            {
                Console.WriteLine("division statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("(5", null) == 5)
            {
                Console.WriteLine("open parentheses statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate(")5", null) == 5)
            {
                Console.WriteLine("closed parentheses statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }
        try
        {
            if (Evaluator.Evaluate("(5)", null) == 5)
            {
                Console.WriteLine("in parentheses statement is valid");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
            {
                Console.WriteLine("Invalid Expression Throws");
            }
        }

    }
}