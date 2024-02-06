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
/// <inheritdoc>
using SpreadsheetUtilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS
{
    /// <summary>
    /// 
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        DependencyGraph DG = new DependencyGraph();
        Dictionary<string, object> Data = new Dictionary<string, object>();
        /// <summary>
        ///   Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is null or invalid
        /// </exception>
        /// 
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// 
        /// <returns>
        ///   The return value should be either a string, a double, or a Formula.
        ///   See the class header summary 
        /// </returns>
        public override object GetCellContents(string name)
        {
            if (!Extensions.Extensions.isValidCell(name)||name == null)
            {
                throw new InvalidNameException();
            }
            return Data.TryGetValue(name, out object result);
        }
        /// <inheritdoc />
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return Data.Keys;
        }
        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///      The method returns a set consisting of name plus the names of all other cells whose value depends, 
        ///      directly or indirectly, on the named cell.
        ///   </para>
        /// 
        ///   <para>
        ///      For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///      set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> result = new HashSet<string>();
            if (!Extensions.Extensions.isValidCell(name) || name == null)
            {
                throw new InvalidNameException();
            }
            Data[name] = number;
            result.Add(name);
            if (DG.HasDependents(name))
            {
                foreach(string dependent in DG.GetDependents(name))
                {
                    result.Add(dependent);
                }
            }
            return result;
        }
        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   The method returns a set consisting of name plus the names of all 
        ///   other cells whose value depends, directly or indirectly, on the 
        ///   named cell.
        /// 
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> result = new HashSet<string>();
            if (!Extensions.Extensions.isValidCell(name) || name == null)
            {
                throw new InvalidNameException();
            }else if(text == null)
            {
                throw new ArgumentNullException("text cannot be null");
            }
            Data[name] = text;
            result.Add(name);
            if (DG.HasDependents(name))
            {
                foreach (string dependent in DG.GetDependents(name))
                {
                    result.Add(dependent);
                }
            }
            return result;
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }
}
