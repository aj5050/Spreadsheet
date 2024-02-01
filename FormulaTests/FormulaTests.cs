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
/// 
/// </summary>
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        Func<string, string> normalizer = token => token.ToUpper();
        Func<string, bool> isValid = token => token.Equals("X")||token.Equals("Y");
        // ************************** Exception Tests ************************* //
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaNull()
        {
            Formula f = new Formula(null);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidOperator()
        {
            Formula f = new Formula("+");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidParentheses()
        {
            Formula f = new Formula(")");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaBeginningInvalidVariable()
        {
            Formula f = new Formula("z", normalizer, isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndOperator()
        {
            Formula f = new Formula("1+");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndParentheses()
        {
            Formula f = new Formula("1+2(");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEndVariable()
        {
            Formula f = new Formula("1+z",normalizer,isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidIllegalVarialbe()
        {
            Formula f = new Formula("1+z+3", normalizer, isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaTooManyClosingParentheses()
        {
            Formula f = new Formula("1+(z))+3");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaTooManyOpeningParentheses()
        {
            Formula f = new Formula("1+((z)+3");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidForulaParenthesesThenOp()
        {
            Formula f = new Formula("1+(+3)");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidForulaOpThenParentheses()
        {
            Formula f = new Formula("1+(3+)");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidForulaXtraFollowingRuleCase1()
        {
            Formula f = new Formula("1+(3)x+5");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidForulaXtraFollowingRuleCase2()
        {
            Formula f = new Formula("1+3x+5");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidForulaXtraFollowingRuleCase3()
        {
            Formula f = new Formula("1+x(+5");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaDivisionByZero()
        {
            Formula f = new Formula("1/0");
        }
        [TestMethod]
        public void TestFormulaLegalVariable() 
        {
            Formula f = new Formula("1+z+3");
        }
        
        [TestMethod]
        public void TestFormulaValidLegalVariable()
        {
            Formula f = new Formula("1+x+3",normalizer,isValid);
        }
        // ************************** Evaluator Tests ************************* //
        [TestMethod]
        public void TestFormulaEvaluateAddition()
        {
            Formula f = new Formula("5+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "10");
        }
        [TestMethod]
        public void TestFormulaEvaluateSubtraction()
        {
            Formula f = new Formula("5- 5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "0");
        }
        [TestMethod]
        public void TestFormulaEvaluateMultiplication()
        {
            Formula f = new Formula("5*5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "25");
        }
        [TestMethod]
        public void TestFormulaEvaluateDivision()
        {
            Formula f = new Formula("5/5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "1");
        }
        [TestMethod]
        public void TestFormulaEvaluateParentheses()
        {
            Formula f = new Formula("(5+5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "15");
        }
        [TestMethod]
        public void TestFormulaEvaluateAdvancedParentheses()
        {
            Formula f = new Formula("(25/5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "10");
        }
        [TestMethod]
        public void TestFormulaEvaluateProParentheses()
        {
            Formula f = new Formula("5/(25/5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        [TestMethod]
        public void TestFormulaEvaluateProMultiplicationParentheses()
        {
            Formula f = new Formula("5*(5*5)+5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "130");
        }
        [TestMethod]
        public void TestFormulaEvaluateSwitchedOpsParentheses()
        {
            Formula f = new Formula("5+(25/5)/5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        [TestMethod]
        public void TestFormulaEvaluateNestedParentheses()
        {
            Formula f = new Formula("5+((25/5)/5)");
            Assert.IsTrue(f.Evaluate(null).ToString() == "6");
        }
        [TestMethod]
        public void TestFormulaEvaluatePemdas()
        {
            Formula f = new Formula("5-5+(5/5)*5");
            Assert.IsTrue(f.Evaluate(null).ToString() == "5");
        }
        [TestMethod]
        public void TestFormulaEvaluateVariables()
        {
            Formula f = new Formula("x+x",normalizer,isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "10");
        }
        [TestMethod]
        public void TestFormulaEvaluateSingleParentheses()
        {
            Formula f = new Formula("(x)", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "5");
        }
        [TestMethod]
        public void TestFormulaEvaluateDivisionAfterParentheses()
        {
            Formula f = new Formula("(x)/5", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "1");
        }
        [TestMethod]
        public void TestFormulaEvaluateMultiplicationInParentheses()
        {
            Formula f = new Formula("(x*5)", normalizer, isValid);
            Assert.IsTrue(f.Evaluate((X) => 5).ToString() == "25");
        }
        // ************************** GetVariable Tests ************************* //
        [TestMethod]
        public void TestFormulaGetVariables()
        {
            Formula f = new Formula("(x)", normalizer, isValid);
            Assert.IsTrue(f.GetVariables().First() == "X");
        }
        [TestMethod]
        public void TestFormulaGetVariablesNoNormalizer()
        {
            Formula f = new Formula("(x)");
            Assert.IsTrue(f.GetVariables().First() == "x");
        }
        [TestMethod]
        public void TestFormulaGetDuplicateVariables()
        {
            Formula f = new Formula("(x) + y + X", normalizer, isValid);
            Assert.IsTrue(f.GetVariables().Count() == 2);
            Assert.IsTrue(f.GetVariables().First() == "X");
            Assert.IsTrue(f.GetVariables().Last() == "Y");
            Assert.IsTrue(!f.GetVariables().Contains("x"));
        }
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
    }
}