// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      1-18-2024
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
///     This file contains the DependencyGraph class and it's methods.
/// 
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, List<string>> DG;
        private List<string> dependentsList;

        private int pairSize;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependentsList = new List<string>();
            DG = new Dictionary<string, List<string>>();
            pairSize = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return pairSize; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {

            get
            {
                if (HasDependees(s))
                {
                    return GetDependees(s).Count();
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //the dependent graph can contain a key with an empty list because of the remove method, to get around this added additional && statement
            if (DG.ContainsKey(s) && DG[s].Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //if it's in the dependent list it has to have a dependee
            if (dependentsList.Contains(s))
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //returns an empty list if s has no dependents, returns dependents otherwise
            List<string> result = new List<string>();
            if (HasDependents(s))
            {
                result = DG[s];
            }
            return result;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //returns an empty list if s has no dependees, otherwise returns list of dependees
            List<string> result = new List<string>();
            if (HasDependees(s))
            {
                foreach (string key in DG.Keys)
                {
                    if (DG[key].Contains(s))
                    {
                        result.Add(key);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {

            List<string> temp;
            //if there is no list of dependents associated with s, then add s to the dictionary and put the dependent t in the pair list for s and in the private dependentList.
            if (!DG.TryGetValue(s, out temp))
            {
                temp = new List<string>();
                DG.Add(s, temp);


            }//if a list of dependents is found with dependee s and the list already contains the dependent, do not add anything and return.
            else if (temp.Contains(t))
            {
                return;
            }
            //if a list of dependents is found with dependee s but the list doesnt contain the dependent, add the dependent to the pair list for s and to the private dependentList.
            temp.Add(t);
            dependentsList.Add(t);
            pairSize++;
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //since AddDependecy does not add duplicates, it is safe to assume that you only need to remove the first instance of t. Also remove first instance of t in dependent list
            if (DG.ContainsKey(s) && pairSize > 0)
            {
                DG[s].Remove(t);
                pairSize--;
                dependentsList.Remove(t);
            }//if there are no more pairs or the pair doesn't exist, throw an exception
            else
            {
                throw new ArgumentException("Dependency does not exist");
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //if the dependee exists, replace its dependents
            if (DG.ContainsKey(s))
            {
                //cannot use foreach loop, as the list changes when we remove a dependency.
                for (int i = 0; DG[s].Count > 0; i++)
                {
                    RemoveDependency(s, DG[s][i]);
                }
                foreach (string value in newDependents)
                {
                    AddDependency(s, value);
                }

            }


        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //if the dependent exists, replace its dependees
            if (dependentsList.Contains(s))
            {
                foreach (string key in GetDependees(s))
                {
                    RemoveDependency(key, s);
                }
                foreach (string key in newDependees)
                {
                    AddDependency(key, s);
                }
            }

        }

    }

}
