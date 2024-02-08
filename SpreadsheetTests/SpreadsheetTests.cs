/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      2-5-2024
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
/// 
/// </summary>
using SpreadsheetUtilities;
namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        Spreadsheet spreadsheet;
        [TestInitialize]
        public void testInitialize()
        {
            spreadsheet = new Spreadsheet();
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCell()
        {
            
            spreadsheet.GetCellContents("1A");
        }
        [TestMethod]
        public void TestGetValidCells()
        {
            spreadsheet.GetCellContents("x");
            spreadsheet.GetCellContents("X");
            spreadsheet.GetCellContents("x_");
            spreadsheet.GetCellContents("_x");
            spreadsheet.GetCellContents("x_y");
            spreadsheet.GetCellContents("z_15");
            spreadsheet.GetCellContents("___");
            spreadsheet.GetCellContents("_");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCellNum()
        {
            spreadsheet.GetCellContents("1");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCellOperator()
        {
            spreadsheet.GetCellContents("+");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNullCell()
        {
            spreadsheet.GetCellContents(null);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCell()
        {
            spreadsheet.SetCellContents("1A", 0);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCellText()
        {
            spreadsheet.SetCellContents("1A", "text");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCellFormula()
        {
            spreadsheet.SetCellContents("1A", new Formula("2+2"));
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCyclicCellFormula()
        {
            spreadsheet.SetCellContents("A1", new Formula("A1+2"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCell()
        {
            spreadsheet.SetCellContents(null,0);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCellTextName()
        {
            spreadsheet.SetCellContents(null, "text");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullCellText()
        {
            string input = null;
            spreadsheet.SetCellContents("A1", input);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCellFormula()
        {
            
            spreadsheet.SetCellContents(null, new Formula("2+2"));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellFormulaNull()
        {
            Formula f = null;
            spreadsheet.SetCellContents("A1", f);
        }
        [TestMethod]
        public void TestGetSingleCell()
        {
            spreadsheet.SetCellContents("A1", 2.0);
            Assert.AreEqual(2.0, spreadsheet.GetCellContents("A1"));
        }
        [TestMethod]
        public void TestGetMultipleCells()
        {
            spreadsheet.SetCellContents("A1", 2.0);
            spreadsheet.SetCellContents("B1", new Formula("A1+2"));
            spreadsheet.SetCellContents("C1", new Formula("B1 + A1"));
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            HashSet<string> actual = spreadsheet.GetNamesOfAllNonemptyCells().ToHashSet();
            Assert.IsTrue(expected.ToString() == actual.ToString());
        }
        [TestMethod]
        public void TestSetSingleCell()
        {
            foreach (string name in spreadsheet.SetCellContents("A1",2.0))
            {
                Assert.IsTrue(name == "A1");
            }
        }
        [TestMethod]
        public void TestSetMultipleCell()
        {
            spreadsheet.SetCellContents("A1", 2.0);
            spreadsheet.SetCellContents("B1", new Formula("A1+2"));
            spreadsheet.SetCellContents("C1", new Formula("B1 + A1"));
            ISet<string> actual = spreadsheet.SetCellContents("A1", 3.0);
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.ToString()==actual.ToString());
        }
        [TestMethod]
        public void TestSetSingleTextCell()
        {
            spreadsheet.SetCellContents("A1", "3");
            Assert.AreEqual("3", spreadsheet.GetCellContents("A1"));
        }
        [TestMethod]
        public void TestSetMultipleTextCell()
        {
            spreadsheet.SetCellContents("A1", "test");
            spreadsheet.SetCellContents("B1", new Formula("A1+2"));
            spreadsheet.SetCellContents("C1", new Formula("B1 + A1"));
            ISet<string> actual = spreadsheet.SetCellContents("A1", "tests");
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.ToString() == actual.ToString());
        }
        [TestMethod]
        public void TestSetMultipleForula()
        {
            spreadsheet.SetCellContents("A1", new Formula("2+2"));
            spreadsheet.SetCellContents("B1", new Formula("A1+2"));
            spreadsheet.SetCellContents("C1", new Formula("B1 + A1"));
            ISet<string> actual = spreadsheet.SetCellContents("A1", new Formula("2+3"));
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.ToString() == actual.ToString());
        }
    }
}