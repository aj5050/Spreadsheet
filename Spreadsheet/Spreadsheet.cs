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
            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }else if (!Data.ContainsKey(name)||Data[name] is null)
            {
                return "";
            }        
            return Data[name];
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

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override string GetXML()
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IList<string> SetCellContents(string name, double number)
        {
            
            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }
            Data[name] = number;
           
            return GetCellsToRecalculate(name).ToList();

        }
        /// <inheritdoc />
        public override IList<string> SetCellContents(string name, string text)
        {
            HashSet<string> result = new HashSet<string>();

            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }
            else if (text is null)
            {
                throw new ArgumentNullException("text cannot be null");
            }
            else if (text.Contains(name))
            {
                throw new CircularException();
            }
            Data[name] = text;
            return GetCellsToRecalculate(name).ToHashSet();

        }
        /// <inheritdoc />
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> result = new HashSet<string>();
            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }
            else if (formula is null)
            {
                throw new ArgumentNullException("formula cannot be null");
            }
            else if (formula.GetVariables() is not null)
            {
                if (formula.GetVariables().Contains(name))
                {
                    throw new CircularException();
                }
                foreach (string variable in formula.GetVariables())
                {
                    DG.AddDependency(variable, name);
                }
            }
            Data[name] = formula;
            return GetCellsToRecalculate(name).ToHashSet();

        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> result = new HashSet<string>();

            foreach (string dependent in DG.GetDependents(name))
            {
                result.Add(dependent);
            }

            return result;
        }
    }
}
