using SpreadsheetUtilities;

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
///     This file contains test methods for the Formula.cs Class. 
/// </summary>
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        Func<string, string> normalizer = token => token.ToUpper();
        Func<string, bool> isValid = token => token.Equals("X") || token.Equals("Y");
        /// <summary>
        /// The Test Methods in this section test all the cases for exceptions presented in the parsing rules 
        /// paragraph of the assignment doc. The test names correlate with the parsing rule/exception they throw.
        /// </summary>
        // ************************** Exception Tests ************************* //
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaNull()
        {
            Formula f = new Formula(null);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidOperator()
        {
            Formula f = new Formula("+");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidParentheses()
        {
            Formula f = new Formula(")");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidVariable()
        {
            Formula f = new Formula("z", normalizer, isValid);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndOperator()
        {
            Formula f = new Formula("1+");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndParentheses()
        {
            Formula f = new Formula("1+2(");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndVariable()
        {
            Formula f = new Formula("1+z", normalizer, isValid);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidIllegalVariable()
        {
            Formula f = new Formula("1+z+3", normalizer, isValid);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaTooManyClosingParentheses()
        {
            Formula f = new Formula("1+(z))+3");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaTooManyOpeningParentheses()
        {
            Formula f = new Formula("1+((z)+3");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidFormulaParenthesesThenOp()
        {
            Formula f = new Formula("1+(+3)");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidFormulaOpThenParentheses()
        {
            Formula f = new Formula("1+(3+)");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidFormulaXtraFollowingRuleCase1()
        {
            Formula f = new Formula("1+(3)x+5");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidFormulaXtraFollowingRuleCase2()
        {
            Formula f = new Formula("1+3x+5");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidFormulaXtraFollowingRuleCase3()
        {
            Formula f = new Formula("1+x(+5");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaLegalVariable()
        {
            Formula f = new Formula("1+z+3");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaValidLegalVariable()
        {
            Formula f = new Formula("1+x+3", normalizer, isValid);
        }
        /// <summary>
        /// This section contains test methods that assert that the Evaluate method of Formula.cs is working. The test names describe what the assertion is testing. 
        /// </summary>
        // ************************** Evaluator Tests ************************* //
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateAddition()
        {
            Formula f = new Formula("5+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "10");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateSubtraction()
        {
            Formula f = new Formula("5- 5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "0");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateMultiplication()
        {
            Formula f = new Formula("5*5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "25");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateDivision()
        {
            Formula f = new Formula("5/5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "1");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateParentheses()
        {
            Formula f = new Formula("(5+5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "15");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateAdvancedParentheses()
        {
            Formula f = new Formula("(25/5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "10");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateProParentheses()
        {
            Formula f = new Formula("5/(25/5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateProMultiplicationParentheses()
        {
            Formula f = new Formula("5*(5*5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "130");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateSwitchedOpsParentheses()
        {
            Formula f = new Formula("5+(25/5)/5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateNestedParentheses()
        {
            Formula f = new Formula("5+((25/5)/5)");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluatePemdas()
        {
            Formula f = new Formula("5-5+(5/5)*5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "5");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateVariables()
        {
            Formula f = new Formula("x+x", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "10");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateSingleParentheses()
        {
            Formula f = new Formula("(x)", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "5");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateDivisionAfterParentheses()
        {
            Formula f = new Formula("(x)/5", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "1");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateMultiplicationInParentheses()
        {
            Formula f = new Formula("(x*5)", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "25");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateScientificNotation()
        {
            Formula f = new Formula("10e2 *5", normalizer, isValid);
            Assert.AreEqual(f.Evaluate(null).ToString(), "5000");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluate()
        {
            Formula f = new Formula("100*5", normalizer, isValid);
            Assert.AreEqual(f.Evaluate(null).ToString(), "500");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluatorDivisionByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluatorDivisionByZeroVariable()
        {
            Formula f = new Formula("5/x", normalizer, isValid);
            Assert.IsInstanceOfType(f.Evaluate((X) => 0), typeof(FormulaError));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluatorDivisionByZeroOverload()
        {
            Formula f = new Formula("1/(0/5)", normalizer, isValid);
            Assert.IsInstanceOfType(f.Evaluate((X) => 0), typeof(FormulaError));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEvaluateMultiplicationOverload()
        {
            Formula f = new Formula("2*(x*5)*2", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "100");
        }
        /// <summary>
        ///  This section contains tests that assert that the GetVariable method of the Formula.cs class is working. The test method name describes what the test is asserting. 
        /// </summary>
        // ************************** GetVariable Tests ************************* //
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaGetVariables()
        {
            Formula f = new Formula("(x)", normalizer, isValid);
            Assert.IsTrue(f.GetVariables().First() == "X");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaGetVariablesNoNormalizer()
        {
            Formula f = new Formula("(x)");
            Assert.IsTrue(f.GetVariables().First() == "x");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaGetDuplicateVariables()
        {
            Formula f = new Formula("(x) + y + X", normalizer, isValid);
            Assert.IsTrue(f.GetVariables().Count() == 2);
            Assert.IsTrue(f.GetVariables().First() == "X");
            Assert.IsTrue(f.GetVariables().Last() == "Y");
            Assert.IsTrue(!f.GetVariables().Contains("x"));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaGetDuplicateVariablesNoNormalizer()
        {
            string[] variables = new string[3];
            Formula f = new Formula("(x) + y + X");
            variables[0] = "x";
            variables[1] = "y";
            variables[2] = "X";
            string[] formulaVars = f.GetVariables().ToArray();
            for (int i = 0; i < variables.Length; i++)
            {
                Assert.IsTrue((variables[i] == formulaVars[i]));
            }
        }
        /// <summary>
        /// This section contains tests that assert that the ToString method for Formula.cs is working. The test names describe what is being asserted.
        /// </summary>
        // ************************** ToString Tests ************************* //
        /// <summary>
        /// See title
        /// </summary>        
        [TestMethod]
        public void TestFormulaToString()
        {
            Formula f = new Formula("(x)", normalizer, isValid);
            Assert.IsTrue(f.ToString() == "(X)");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaToStringNoNormalizer()
        {
            Formula f = new Formula("(x)");
            Assert.IsTrue(f.ToString() == "(x)");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaToStringSpaces()
        {
            Formula f = new Formula("(x + x)", normalizer, isValid);
            Assert.IsTrue(f.ToString() == "(X+X)");
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaToStringSpacesNoNormalizer()
        {
            Formula f = new Formula("(x + x)");
            Assert.IsTrue(f.ToString() == "(x+x)");
        }

        /// <summary>
        /// This section contains tests that assert that the Equals, == and != operators, and GetHashCode method for the Formula.cs class work. The test names describe what is being asserted. 
        /// </summary>
        // ************************** Equals Tests ************************* //
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEquals()
        {
            Formula f = new Formula("(x + x)", normalizer, isValid);
            Formula f2 = new Formula("(X+X)", normalizer, isValid);
            Assert.IsTrue(f.Equals(f2));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEqualsFalse()
        {
            Formula f = new Formula("(x + y)", normalizer, isValid);
            Formula f2 = new Formula("(X+X)", normalizer, isValid);
            Assert.IsFalse(f.Equals(f2));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEqualsNull()
        {
            Formula f = new Formula("(x + y)", normalizer, isValid);
            Formula f2 = null;
            Assert.IsFalse(f.Equals(f2));
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEqualsOp()
        {
            Formula f = new Formula("(x + x)", normalizer, isValid);
            Formula f2 = new Formula("(X+X)", normalizer, isValid);
            Assert.IsTrue(f == f2);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEqualsFalseOp()
        {
            Formula f = new Formula("(x + y)", normalizer, isValid);
            Formula f2 = new Formula("(X+X)", normalizer, isValid);
            Assert.IsTrue(f != f2);
        }
        /// <summary>
        /// See title
        /// </summary>
        [TestMethod]
        public void TestFormulaEqualsHash()
        {
            Formula f = new Formula("(x + x)", normalizer, isValid);
            Formula f2 = new Formula("(X+X)", normalizer, isValid);
            Assert.IsTrue(f.GetHashCode() == f2.GetHashCode());
        }
    }
}