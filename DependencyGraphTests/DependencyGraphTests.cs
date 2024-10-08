/// <summary>
/// Author:    Austin January
/// Partner:   None
/// Date:      1-18-2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Austin January - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Austin January, certify that I wrote all code after the Stress Test from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///   This file contains tests for the DependencyGraph project. 
/// 
/// </summary>
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }
        ///<summary>
        ///It should be possible to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
                HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
                HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        /// <summary>
        /// This test asserts that the Dependency Graph can be cyclic, with the dependee being a dependent on its dependent and the dependent being a dependee of its dependee
        /// </summary>
        [TestMethod()]
        public void cyclicDependencyTest()
        {
            //create cycle
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.AddDependency("y", "x");
            Assert.AreEqual(t.Size, 2);
            //assert that both have dependents and dependees
            Assert.IsTrue(t.HasDependents("x"));
            Assert.IsTrue(t.HasDependents("y"));
            Assert.IsTrue(t.HasDependees("x"));
            Assert.IsTrue(t.HasDependees("y"));
            //assert that the dependents and dependees are in a cycle (x is a dependent/dependee of y and vice versa)
            foreach (string dependee in t.GetDependees("x"))
            {
                Assert.IsTrue(dependee.Equals("y"));
            }
            foreach (string dependee in t.GetDependees("y"))
            {
                Assert.IsTrue(dependee.Equals("x"));
            }
            foreach (string dependent in t.GetDependents("x"))
            {
                Assert.IsTrue(dependent.Equals("y"));
            }
            foreach (string dependent in t.GetDependents("y"))
            {
                Assert.IsTrue(dependent.Equals("x"));
            }
        }
        /// <summary>
        /// This test asserts that an empty graph throws an exception for remove (remove throws for both an empty graph and for a dependency that doesn't exist)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void throwsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("x", "y");
        }
        /// <summary>
        /// This test asserts that the overwritten syntax of the brackets counts the amount of dependees for the given string
        /// </summary>
        [TestMethod()]
        public void bracketsDependeesTest()
        {
            //add 3 dependencies
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("w", "y");
            t.AddDependency("v", "y");
            Assert.AreEqual(3, t["y"]);
            //remove a dependency
            t.RemoveDependency("x", "y");
            Assert.AreEqual(2, t["y"]);
            //remove all dependencies
            t.RemoveDependency("w", "y");
            t.RemoveDependency("v", "y");
            Assert.AreEqual(0, t["y"]);
            //add another dependency to assert that when a new graph is created the dependees go back down to zero
            t.AddDependency("u", "y");
            t = new DependencyGraph();
            Assert.AreEqual(0, t["y"]);
        }
        /// <summary>
        /// This test asserts that an exception is thrown if there are no more pairs in the graph or the pair provided by the parameter does not exist
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void removeAfterNoPairs()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.RemoveDependency("x", "y");
            t.RemoveDependency("x", "y");
        }
        /// <summary>
        /// This test asserts that duplicate pairs cannot be added into the graph
        /// </summary>
        [TestMethod()]
        public void addDuplicatePairsTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsTrue(t.Size == 0);
            //add two pairs and make sure the size doesn't increase/ the duplicate dependency is not added
            t.AddDependency("x", "y");
            Assert.IsTrue(t.Size == 1);
            t.AddDependency("x", "y");
            Assert.IsTrue(t.Size == 1);
        }
    }
}
