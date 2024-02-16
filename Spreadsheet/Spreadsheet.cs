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
///     This file contains the namespace SS which contains an implimentation of the AbstractSpreadsheet abstract class.
/// 
/// </summary>
using SpreadsheetUtilities;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        DependencyGraph DG = new DependencyGraph();
        Dictionary<string, object> Data = new Dictionary<string, object>();

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
        }

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public override object GetCellContents(string name)
        {
            if (name is null || !Extensions.Extensions.isValidCell(name)|| !IsValid(name))
            {
                throw new InvalidNameException();
            }else if (!Data.ContainsKey(Normalize(name))||Data[Normalize(name)] is null)
            {
                return "";
            }        
            
            return Data[Normalize(name)];
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> result = new HashSet<string>();
            foreach(string key in  Data.Keys) {
                if(Data[key] is not null && Data[key] != "")
                {
                    result.Add(key);
                }
            }
            return result;
        }
        /// <summary>
        ///   Look up the version information in the given file. If there are any problems opening, reading, 
        ///   or closing the file, the method should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// 
        /// <remarks>
        ///   In an ideal world, this method would be marked static as it does not rely on an existing SpreadSheet
        ///   object to work; indeed it should simply open a file, lookup the version, and return it.  Because
        ///   C# does not support this syntax, we abused the system and simply create a "regular" method to
        ///   be implemented by the base class.
        /// </remarks>
        /// 
        /// <exception cref="SpreadsheetReadWriteException"> 
        ///   1Thrown if any problem occurs while reading the file or looking up the version information.
        /// </exception>
        /// 
        /// <param name="filename"> The name of the file (including path, if necessary)</param>
        /// <returns>Returns the version information of the spreadsheet saved in the named file.</returns>
        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///   Return an XML representation of the spreadsheet's contents
        /// </summary>
        /// <returns> contents in XML form </returns>
        public override string GetXML()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            using(XmlWriter writer = XmlWriter.Create(filename,new XmlWriterSettings()))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement();
            }
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IList<string> SetCellContents(string name, double number)
        {
            
            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            Data[Normalize(name)] = number;
           
            return GetCellsToRecalculate(Normalize(name)).ToList();

        }
        /// <inheritdoc />
        protected override IList<string> SetCellContents(string name, string text)
        {
            HashSet<string> result = new HashSet<string>();

            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            else if (text is null)
            {
                throw new ArgumentNullException("text cannot be null");
            }
            else if (text.Contains(Normalize(name)))
            {
                throw new CircularException();
            }
            Data[Normalize(name)] = text;
            return GetCellsToRecalculate(Normalize(name)).ToList();

        }
        /// <inheritdoc />
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> result = new HashSet<string>();
            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            else if (formula is null)
            {
                throw new ArgumentNullException("formula cannot be null");
            }
            else if (formula.GetVariables() is not null)
            {
                if (formula.GetVariables().Contains(Normalize(name)))
                {
                    throw new CircularException();
                }
                foreach (string variable in formula.GetVariables())
                {
                    DG.AddDependency(variable, Normalize(name));
                }
            }
            Data[Normalize(name)] = formula;
            return GetCellsToRecalculate(Normalize(name)).ToList();

        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if(double.TryParse(content,out double value))
            {
                return SetCellContents(name, value);
            }else if (content.StartsWith('='))
            {
                content.Remove(0, 1);
                return SetCellContents(name, new Formula(content));
            }
            else
            {
                return SetCellContents(name, content);
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> result = new HashSet<string>();
            
            foreach (string dependent in DG.GetDependents(Normalize(name)))
            {
                result.Add(dependent);
            }

            return result;
        }
    }
}
