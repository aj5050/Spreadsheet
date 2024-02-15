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
        /// <inheritdoc />
        public override object GetCellContents(string name)
        {
            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }
            else if (!Data.ContainsKey(name) || Data[name] is null)
            {
                return "";
            }
            return Data[name];
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
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> result = new HashSet<string>();
            if (name is null || !Extensions.Extensions.isValidCell(name))
            {
                throw new InvalidNameException();
            }
            result = GetCellsToRecalculate(name).ToHashSet();
            Data[name] = number;
            return result;

        }
        /// <inheritdoc />
        public override ISet<string> SetCellContents(string name, string text)
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

            result = GetCellsToRecalculate(name).ToHashSet();
            Data[name] = text;
            return result;

        }
        /// <inheritdoc />
        public override ISet<string> SetCellContents(string name, Formula formula)
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
            result = GetCellsToRecalculate(name).ToHashSet();
            Data[name] = formula;
            return result;

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
