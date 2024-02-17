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
///     This file contains the namespace SS which contains an implementation of the AbstractSpreadsheet abstract class.
/// 
/// </summary>
using SpreadsheetUtilities;
using System.Reflection;
using System.Text;
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
        private bool changed = false;

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
        }
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get => changed; protected set => changed = value; }

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
        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell that we want the value of (will be normalized)</param>
        /// 
        /// <returns>
        ///   Returns the value (as opposed to the contents) of the named cell.  The return
        ///   value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </returns>
        public override object GetCellValue(string name)
        {
            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            else if (Data[Normalize(name)] is string)
            {
                return Data[Normalize(name)];
            }
            else
            {
                if(Data[Normalize(name)] is double)
                {
                    return Data[Normalize(name)];
                }
                else
                {
                    return ((Formula)Data[Normalize(name)]).Evaluate((x) => (double)GetCellContents(x));
                }
            }
            
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
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                       if(reader.Name == "spreadsheet")
                        {
                            return reader.GetAttribute("version");
                        }
                    }
                }
            }catch(Exception ex) {
                throw new SpreadsheetReadWriteException("Filename couldn't be accessed/couldn't be written/doesn't exist");
            }
            throw new FileNotFoundException(filename);
        }
        /// <summary>
        ///   Return an XML representation of the spreadsheet's contents
        /// </summary>
        /// <returns> contents in XML form </returns>
        public override string GetXML()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", Version);
                foreach (string key in Data.Keys)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", key);
                    writer.WriteElementString("contents", Data[key].ToString());
                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
            }
            return sb.ToString();
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
            try
            {
               
                using (XmlWriter writer = XmlWriter.Create(filename, new XmlWriterSettings()))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (string key in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", key);
                        writer.WriteElementString("contents", Data[key].ToString());
                        writer.WriteEndElement();

                    }
                    writer.WriteEndElement();
                    Changed = false;
                }
                
            }catch(Exception ex)
            {
                throw new SpreadsheetReadWriteException("Couldn't save under this file's name");
            }
            
            
        }

        /// <inheritdoc />
        protected override IList<string> SetCellContents(string name, double number)
        {
            
            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            DG.ReplaceDependees(name, new List<string>());
            Data[Normalize(name)] = number;
            Changed = true;
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
            DG.ReplaceDependees(name, new List<string>());
            Data[Normalize(name)] = text;
            Changed = true;
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
            IEnumerable<string> ogDependees = DG.GetDependees(Normalize(name));
            if (formula.GetVariables() is not null)
            {

                if (formula.GetVariables().Contains(Normalize(name)))
                {
                    throw new CircularException();
                }
                DG.ReplaceDependees(Normalize(name), formula.GetVariables());
            }
            try
            {
                result = GetCellsToRecalculate(Normalize(name)).ToHashSet();
            }
            catch (Exception ex)
            {
                DG.ReplaceDependees(Normalize(name), ogDependees);
                throw ex;
            }
            Data[Normalize(name)] = formula;
            Changed = true;
            return result.ToList();

        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if(double.TryParse(content,out double value))
            {
                return SetCellContents(name, value);
            }else if (content.StartsWith('='))
            {
                string input = content.Remove(0, 1);
                return SetCellContents(name, new Formula(input));
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
