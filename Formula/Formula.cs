// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      1-30-2024
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
///    The formula constructor, formula evaluator, formula ToString method, formula Equals method, overwritten == and != operators, formula getHashCode method, formula getVariables method, formula getTokens method, and FormulaFormat exception and Formula Error.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Extensions;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private string expression;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {

        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            int openingParentheses = 0;
            int closingParentheses = 0;
            string prevToken = "";


            //throw for null formula
            if (formula == null||formula ==  "")
            {
                throw new FormulaFormatException("formula cannot be empty");
            }
            //get the first token, and if it isn't a number/ an open parentheses, or is an operator, throw an exception
            string firstItem = GetTokens(formula).First();
            if (!double.TryParse(firstItem, out double num) && !firstItem.Equals("(") && Extensions.Extensions.isTokenOperator(firstItem))
            {
                throw new FormulaFormatException("Invalid Formula");
            }
            //get the last token, and if it isn't a number/ a closed parentheses, or is an operator, throw an exception
            string lastItem = GetTokens(formula).Last();
            if (!double.TryParse(lastItem, out double val) && !lastItem.Equals(")") && Extensions.Extensions.isTokenOperator(lastItem))
            {
                throw new FormulaFormatException("Invalid Formula");
            }
            // loop through and find tokens
            foreach (string token in GetTokens(formula))
            {
                if (Extensions.Extensions.isTokenVariable(token))
                {
                    //if there is no isValid or normalize function, then this if statement gets skipped
                    if (!isValid(normalize(token)))
                    {
                        throw new FormulaFormatException("The formula contains an illegal variable");
                    }
                }
                //check if the token is a parentheses and make sure there are never more closing parentheses than opening parentheses
                else if (token.Equals(")") || token.Equals("("))
                {
                    if (token == "(")
                    {
                        openingParentheses++;
                    }
                    else
                    {
                        closingParentheses++;

                    }
                    if (closingParentheses > openingParentheses)
                    {
                        throw new FormulaFormatException("too many closing parentheses");
                    }
                }
                //Parentheses/Operator Following Rule
                if ((prevToken.Equals("(") || Extensions.Extensions.isTokenOperator(prevToken)) && (Extensions.Extensions.isTokenOperator(token) || token.Equals(")")))
                {
                    throw new FormulaFormatException("Invalid Formula");
                }
                //Extra Following Rule
                else if ((prevToken == ")" || double.TryParse(prevToken, out double result) || Extensions.Extensions.isTokenVariable(prevToken)) && !Extensions.Extensions.isTokenOperator(token) && !token.Equals(")"))
                {
                    throw new FormulaFormatException("Invalid Formula");
                }

                prevToken = token;
            }
            // if opening parentheses are greater than closing parentheses, throw an exception
            if (openingParentheses > closingParentheses)
            {
                throw new FormulaFormatException("too many opening parentheses");
            }

            //normalize all tokens in formula and then put that in the expression
            StringBuilder sb = new StringBuilder();

            foreach (string token in GetTokens(formula))
            {
                if (token != " ")
                {
                    sb.Append(normalize(token));
                }

            }

            expression = sb.ToString();
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<string> operatorStack = new Stack<string>();
            Stack<double> valueStack = new Stack<double>();
            string[] substrings = GetTokens(expression).ToArray();
            foreach (string substring in substrings)
            {
                // this if statement asserts if the substring is an double or not, and if it is, it calls the valPush helper method. 
                if (double.TryParse(substring, out double result))
                {

                    if (result == 0 && operatorStack.Count > 0 && operatorStack.Peek() == "/")
                    {
                        return new FormulaError();
                    }
                    Extensions.Extensions.valPush(result, operatorStack, valueStack);
                }
                // this else if statement will assert that the given string is a variable, as the initial if statement shows that it is not an double,
                // and all variables require a length of at least 2. 
                else if (Extensions.Extensions.isTokenVariable(substring))
                {
                    if (lookup(substring) == 0 && operatorStack.Count > 0 && operatorStack.Peek() == "/")
                    {
                        return new FormulaError();
                    }
                    Extensions.Extensions.valPush(lookup(substring), operatorStack, valueStack);
                }
                // this else if statement asserts that the substring is an operator, and if it is, then it either calls the opPop helper method
                // (for the + and - operators) or pushes the operator onto the operatorStack
                else if (substring == "+" || substring == "-" || substring == "*" || substring == "/")
                {
                    if (substring == "+" || substring == "-")
                    {
                        Extensions.Extensions.opPop(operatorStack, valueStack);
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
                        Extensions.Extensions.opPop(operatorStack, valueStack);
                        //step 2
                        if (operatorStack.Count > 0)
                        {
                            operatorStack.Pop();
                        }

                        //step 3
                        if (operatorStack.Count != 0 && valueStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            double temp1 = valueStack.Pop();
                            if (operatorStack.Pop() == "*")
                            {
                                valueStack.Push(valueStack.Pop() * temp1);
                            }
                            else if (temp1 == 0)
                            {
                                return new FormulaError("Cannot divide by zero");
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
                    double temp1 = valueStack.Pop();
                    valueStack.Push(valueStack.Pop() - temp1);
                }
            }

            // end result
            return valueStack.Pop();
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            List<String> variables = new List<String>();

            foreach (string token in GetTokens(expression))
            {
                if (Extensions.Extensions.isTokenVariable(token) && !variables.Contains(token))
                {
                    variables.Add(token);
                }
            }

            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return expression;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {

            if (obj != null && obj.GetType() == typeof(Formula))
            {
                Formula objFormula = (Formula)obj;
                string formulaStr = this.ToString();
                string objFormulaStr = objFormula.ToString();
                if (formulaStr == objFormulaStr)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {

            return expression.GetHashCode();

        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
