/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      2-13-2024
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
///     This namespace contains a test class that contains a test method for every method in Spreadsheet.cs
/// 
/// </summary>
using SpreadsheetUtilities;
using System.Xml;
namespace SS
{
    /// <summary>
    /// This test class contains methods that test every method in Spreadsheet.cs
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        Spreadsheet spreadsheet;
        /// <summary>
        /// This method initializes the spreadsheet before each test
        /// </summary>
        [TestInitialize]
        public void testInitialize()
        {
            spreadsheet = new Spreadsheet((x) => true, (x) => x.ToUpper(), "version 1");
        }
        /// <summary>
        /// These tests assert that an exception is thrown for invalid/null/circular cell names
        /// </summary>
        // ************************** Exception Tests ************************* //
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCellNameValue()
        {
            spreadsheet.GetCellValue("1A");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCell()
        {
            spreadsheet.GetCellContents("1A");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCellNum()
        {
            spreadsheet.GetCellContents("1");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalidCellOperator()
        {
            spreadsheet.GetCellContents("+");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNullCell()
        {
            spreadsheet.GetCellContents(null);
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCell()
        {
            spreadsheet.SetContentsOfCell("1A", "0");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCellText()
        {
            spreadsheet.SetContentsOfCell("1A", "text");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidCellFormula()
        {
            spreadsheet.SetContentsOfCell("1A", "=2+2");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCyclicCellFormula()
        {
            spreadsheet.SetContentsOfCell("A1", "=A1+2");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestNestedCyclicCellFormula()
        {
            spreadsheet.SetContentsOfCell("A1", "=B1+2");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCell()
        {
            spreadsheet.SetContentsOfCell(null, "0");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCellTextName()
        {
            spreadsheet.SetContentsOfCell(null, "text");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetNullCellText()
        {
            string input = null;
            spreadsheet.SetContentsOfCell("A1", input);
            Assert.IsTrue("" == spreadsheet.GetCellValue("A1"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullCellFormula()
        {

            spreadsheet.SetContentsOfCell(null, "=2+2");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetCellFormulaNull()
        {

            spreadsheet.SetContentsOfCell("A1", "=");
        }
        /// <summary>
        /// See Title, should pass without exception
        /// </summary>
        public void TestCyclicTextCell()
        {
            spreadsheet.SetContentsOfCell("A1", "A1+2");
            Assert.IsTrue((string)spreadsheet.GetCellValue("A1") == "A1+2");
        }
        /// <summary>
        /// These tests assert that each method works for a single cell
        /// </summary>
        // ************************** Single Cell Tests ************************* //
        /// <summary>
        /// This test just asserts that no exception is thrown for valid cells
        /// </summary>
        [TestMethod]
        public void TestGetValidCells()
        {
            spreadsheet.GetCellContents("x1");
            spreadsheet.GetCellContents("X2");
            spreadsheet.GetCellContents("x1");
            spreadsheet.GetCellContents("x3");
            spreadsheet.GetCellContents("xy3");
            spreadsheet.GetCellContents("z15");

        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestGetSingleCell()
        {
            spreadsheet.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(2.0, spreadsheet.GetCellContents("A1"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetSingleCell()
        {
            foreach (string name in spreadsheet.SetContentsOfCell("A1", "2.0"))
            {
                Assert.IsTrue(name == "A1");
            }
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetSingleTextCell()
        {
            spreadsheet.SetContentsOfCell("A1", "3");
            Assert.AreEqual(3.0, spreadsheet.GetCellValue("A1"));
        }
        [TestMethod]
        public void TestSetSingleNullTextCell()
        {
            string text = null;
            spreadsheet.SetContentsOfCell("A1", text);
            Assert.IsTrue("" == spreadsheet.GetCellValue("A1"));

        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestGetFormulaEval()
        {
            spreadsheet.SetContentsOfCell("A1", "=2+2");
            Assert.IsTrue(4.0 == (double)spreadsheet.GetCellValue("A1"));
        }
        /// <summary>
        /// These tests assert that each method works for a multiple cells
        /// </summary>
        // ************************** Multiple Cell Tests ************************* //
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestGetMultipleCells()
        {
            spreadsheet.SetContentsOfCell("A1", "2.0");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
            spreadsheet.SetContentsOfCell("C1", "B1 + A1");
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            HashSet<string> actual = spreadsheet.GetNamesOfAllNonemptyCells().ToHashSet();
            Assert.IsTrue(expected.ToString() == actual.ToString());
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetMultipleCell()
        {
            spreadsheet.SetContentsOfCell("A1", "2.0");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
            spreadsheet.SetContentsOfCell("C1", "=B1 + A1");
            IList<string> actual = spreadsheet.SetContentsOfCell("A1", "3.0");
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.SetEquals(actual));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetMultipleTextCell()
        {
            spreadsheet.SetContentsOfCell("A1", "test");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
            spreadsheet.SetContentsOfCell("C1", "=B1 + A1");
            IList<string> actual = spreadsheet.SetContentsOfCell("A1", "tests");
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.SetEquals(actual));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestSetMultipleFormulaCell()
        {
            spreadsheet.SetContentsOfCell("A1", "=2+2");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
            spreadsheet.SetContentsOfCell("C1", "=B1 + A1");
            IList<string> actual = spreadsheet.SetContentsOfCell("A1", "=2+3");
            HashSet<string> expected = new HashSet<string> { "A1", "B1", "C1" };
            Assert.IsTrue(expected.SetEquals(actual));
        }
        /// <summary>
        /// this test asserts that the cell evaluates the given formula
        /// </summary>
        [TestMethod]
        public void TestGetFormulaEvalNested()
        {
            spreadsheet.SetContentsOfCell("A1", "=2+2");
            spreadsheet.SetContentsOfCell("B1", "=A1+2");
            spreadsheet.SetContentsOfCell("C1", "=B1 + A1");
            Assert.IsTrue(10.0 == (double)spreadsheet.GetCellValue("C1"));
        }
        /// <summary>
        /// These tests assert that each method works for assignment 5 implementation
        /// </summary>
        // ************************** A5 new Tests ************************* //
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetCellContents()
        {
            spreadsheet.SetContentsOfCell("A1", "42");
            Assert.AreEqual(42.0, spreadsheet.GetCellContents("A1"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidCellName()
        {
            object result = spreadsheet.GetCellContents("InvalidCell");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetCellValueString()
        {
            spreadsheet.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual("Hello", spreadsheet.GetCellValue("A1"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetCellValueFormula()
        {
            spreadsheet.SetContentsOfCell("A1", "5");
            spreadsheet.SetContentsOfCell("B1", "=A1*2");
            Assert.AreEqual(10.0, spreadsheet.GetCellValue("B1"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells()
        {
            ICollection<string> names = new HashSet<string>();
            names.Add("A1");
            names.Add("B2");
            spreadsheet.SetContentsOfCell("A1", "42");
            spreadsheet.SetContentsOfCell("B2", "Hello");
            Assert.IsTrue(new HashSet<string>(spreadsheet.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B2" }));
        }
        /// <summary>
        /// These tests assert that each method works for assignment 5 xml
        /// </summary>
        // ************************** A5 Xml Tests ************************* //
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetSavedVersion()
        {
            using (XmlWriter writer = XmlWriter.Create("test.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");
                writer.WriteEndElement();
            }
            string version = spreadsheet.GetSavedVersion("test.xml");
            Assert.AreEqual("1.0", version);
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionInvalidFileName()
        {
            string version = spreadsheet.GetSavedVersion("invalid.xml");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void GetXML()
        {
            spreadsheet.SetContentsOfCell("A1", "42");
            string xml = spreadsheet.GetXML();
            StringAssert.Contains(xml, "<cell>");
            StringAssert.Contains(xml, "<name>A1</name>");
            StringAssert.Contains(xml, "<contents>42</contents>");
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void Save()
        {
            spreadsheet.SetContentsOfCell("A1", "42");
            spreadsheet.Save("save_test.xml");
            Assert.IsTrue(File.Exists("save_test.xml"));
        }
        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveInvalidFileName()
        {
            spreadsheet.SetContentsOfCell("A1", "42");
            spreadsheet.Save("invalid/*.xml");
        }
        /// <summary>
        /// These stress tests were taken from the AutoGrader for assignment 4 and adjusted to fit Assignment 5
        /// </summary>
        // ************************** Stress Tests ************************* //
        [TestMethod(), Timeout(2000)]
        
        public void TestStress1()
        {
            spreadsheet.SetContentsOfCell("A1", "=B1+B2");
            spreadsheet.SetContentsOfCell("B1", "=C1-C2");
            spreadsheet.SetContentsOfCell("B2", "=C3*C4");
            spreadsheet.SetContentsOfCell("C1", "=D1*D2");
            spreadsheet.SetContentsOfCell("C2", "=D3*D4");
            spreadsheet.SetContentsOfCell("C3", "=D5*D6");
            spreadsheet.SetContentsOfCell("C4", "=D7*D8");
            spreadsheet.SetContentsOfCell("D1", "=E1");
            spreadsheet.SetContentsOfCell("D2", "=E1");
            spreadsheet.SetContentsOfCell("D3", "=E1");
            spreadsheet.SetContentsOfCell("D4", "=E1");
            spreadsheet.SetContentsOfCell("D5", "=E1");
            spreadsheet.SetContentsOfCell("D6", "=E1");
            spreadsheet.SetContentsOfCell("D7", "=E1");
            spreadsheet.SetContentsOfCell("D8", "=E1");
            IList<String> cells = spreadsheet.SetContentsOfCell("E1", "0");
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }

        // Repeated for extra weight
        [TestMethod(), Timeout(2000)]

        public void TestStress1a()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(2000)]
        
        public void TestStress1b()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(2000)]
        
        public void TestStress1c()
        {
            TestStress1();
        }
    }
}