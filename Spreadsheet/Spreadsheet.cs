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
        /// <inheritdoc />
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
        }
        /// <inheritdoc />
        public override bool Changed { get => changed; protected set => changed = value; }

        /// <inheritdoc />
        public override object GetCellContents(string name)
        {
            if (name is null || !Extensions.Extensions.isValidCell(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            else if (!Data.ContainsKey(Normalize(name)) || Data[Normalize(name)] is null)
            {
                return "";
            }

            return Data[Normalize(name)];
        }
        /// <inheritdoc />
        public override object GetCellValue(string name)
        {
            //essentially GetCellContents, only if it is a formula evaluate it and give the correct answer.
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
                if (Data[Normalize(name)] is double)
                {
                    return Data[Normalize(name)];
                }
                else
                {
                    return ((Formula)Data[Normalize(name)]).Evaluate((x) => (double)GetCellValue(x));
                }
            }

        }

        /// <inheritdoc />
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string key in Data.Keys)
            {
                if (Data[key] is not null && Data[key] != "")
                {
                    result.Add(key);
                }
            }
            return result;
        }
        /// <inheritdoc />
        public override string GetSavedVersion(string filename)
        {
            //try reading the file, get the version if the file exists, throw SpreadsheetReadWriteException if the file couldn't be read, and throws fileNotFoundException to make GetSavedVersion happy
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "spreadsheet")
                        {
                            return reader.GetAttribute("version");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SpreadsheetReadWriteException("Filename couldn't be accessed/couldn't be written/doesn't exist");
            }
            throw new FileNotFoundException(filename);
        }
        /// <inheritdoc />
        public override string GetXML()
        {
            //build a string representation of the XML of this spreadsheet
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
        /// <inheritdoc />
        public override void Save(string filename)
        {
            //saves if the file has been changed, doesn't save if the file hasn't been changed
            if (Changed)
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

                }
                catch (Exception ex)
                {
                    throw new SpreadsheetReadWriteException("Couldn't save under this file's name");
                }

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
                text = "";
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
        /// <inheritdoc />
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            //for each case of content, return the appropriate IList from the appropriate SetCellContents method
            if (double.TryParse(content, out double value))
            {
                return SetCellContents(name, value);
            }

            else if (content is not null && content.StartsWith('='))
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
