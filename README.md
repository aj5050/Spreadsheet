```
Author: Austin January
Start Date: 01-11-2024
Course: CS 3500, University of Utah, School of Computing
GitHub ID: aj5050
Repo: https://github.com/uofu-cs3500-spring24/spreadsheet-aj5050
Commit Date: 2-9-2024 9:15am
Solution: Spreadsheet
Copyright: CS 3500 and Austin January - This work may not be copied for use in Academic Coursework.
```
# Spreadsheet functionality overview
	Jan 11th: Currently setting up GitHub, README files, and analyzing the assignment. Added all files up
	until instruction 6 on the "Assignment One - Formula Evaluator" Google doc. I added the
	"FormulaEvaluatorTester" project with it's FrameWork set to net 8.0, since I couldn't add it as a 6.0
	as instructed. 
	Jan 13th: Added additional README file for the FormulaEvaluatorTester program. Added stacks for
	operators and values, delegates and methods, and finished the first 2 conditions of the evaluator
	method (the substring is an integer or a stack)
	Jan 15th: Finished the rough draft of the Evaluator, writing test code for debugging. Figured out that
	the project type was incorrect, fixed it very painfully. Debugged code after running very basic test,
	addition works now.
	Jan 16th: Wrote more tests for the evaluator, used String's .Trim() method to get rid of whitespace
	leading and trailing. Added 2 private  helper methods for integer tokens and operator tokens. Dividing
	by zero now throws an exception
	Jan 17th: Filled out the XML comments. Currently working on the delegate method. Figured out I didn't
	need to work on the delegate method, wrote more exceptions for Formula Evaluator (like if the
	valueStack is empty). Any "" token is now ignored. Both stacks now reset upon running the evaluator
	method.
	----------------------------------
	Jan 18th: Added some files for assignment 2, still need to add README files and "Extension" project
	Jan 19th: Updated/Formated README files
	Jan 23rd: Added Extension Library, filled all methods up to "RemoveDependency" in DependencyGraph.
	Adjusting methods using new Dictionary. All methods adjusted, ran tests, completed pre-tests, need to
	add more.
	Jan 24th: Added Assignment 1 auto-grader tests, fixed bugs that were present in Evaluator.cs. Added
	more tests to DependencyGraphTests.
	Jan 25th: Added additional tests, fixed bugs presented by additional tests. Got rid of unnecessary
	for loops in methods like
	GetDependents/Dependees. Added some more conditions to prevent bugs, wrote comments, finished first
	draft.
	----------------------------------
	Jan 29th: Added the Formula.cs API, read through instructions.
	Jan 30th: Added test file for Formula.cs, added README files for both classes, added helper methods
	to Extensions (need to add header comment and README file), implemented parsing rules for Formula
	Jan 31st: Added a README file to Extensions as well as more helper methods, wrote most the code for 
	Formula (still need to write GetHashCode method), fixed all bugs presented by Assignment 2 autoGrader
	tests. 
	Feb 1st: Wrote 16 test cases specifically for throwing exceptions, fixed bugs presented by these 
	tests. Also commented why the exceptions are throwing. Wrote 21 test cases for Evaluator method and 
	GetVariable method. Added 10 more tests to get more coverage, fixed bugs presented by these tests.
	Finished writing tests, added comments, fixed isTokenVariable extension, formatted all files. Also
	finished the getHashCode method, so the first rough draft is done, ready for submission. 
	----------------------------------
	Feb 5th: Started assignment 4, added header comments and README files to all classes. 
	Feb 6th: Fixed both DependencyGraph efficiency and Formula bugs. Started writing methods for 
	assignment 4, got 4 methods done. 
	Feb 7th: Finished SetCell(name, formula).
	Feb 8th: Finished rough draft of Spreadsheet, need to write tests and consult t.a.'s. Wrote more tests,
	fixed bugs presented by tests. Got 100% coverage. Updated some text for SetCellContents, need to ask
	t.a.'s if update was needed. Fixed bugs presented by the T.A.
	----------------------------------
# Time Spent
	Assignment 1: Estimated Hours : 17 hrs
	Jan 11th: 1 1/2 hours
	Jan 13th: 1 hour 10 min
	Jan 15th: 12:00pm - 12:25pm 1:00pm - 2:13pm 7:55pm - 8:00pm 
	Jan 16th: 11:25am - 12pm 12:45pm - 1:45pm 2:10pm - 2:50pm 
	Jan 17th: 4:20pm - 5:45pm 7:30pm - 8:40pm 
	Assignment 1 Total Hours: ~9 hrs
	----------------------------------
	Assignment 2: Estimated Hours: 15 hrs
	Jan 18th: 30 min
	Jan 19th: 30 min 
	Jan 23rd: 10:45am - 12:00pm 12:25pm - 1:45pm 2:10pm - 3:20pm 4:00pm - 5:22 pm 
	Jan 24th: 2:30pm - 3:30pm 
	Jan 25th: 10:45am - 12:05pm 2:00pm - 3:10pm 
	Assignment 2 Total Hours: ~9 hours 50 minutes
	----------------------------------
	Assignment 3 Estimated Hours: 13 hrs
	Jan 29th: 1:00pm - 1:20pm
	Jan 30th: 12:40pm - 12:55pm 3:40pm - 5:20pm
	Jan 31st: 2:30pm - 4:50pm
	Feb 1st: 10:50am - 11:55am 12:15pm - 1:45pm 2:05pm - 3:15pm 3:45pm - 5:25pm 7:25pm - 9:00pm
	Assignment 3 Total Hours: ~9 hours 10 minutes
	----------------------------------
	Assignment 4 Estimated Hours: 10 hrs
	Feb 5th: 8:20pm - 8:36pm
	Feb 6th: 11:45am - 12:05pm 12:25pm - 12:50pm 2:10pm - 3:15pm 
	Feb 7th: 1:25pm - 1:35pm
	Feb 8th: 10:55am - 12:00pm 12:25pm - 1:40pm 2:05pm - 3:05pm 3:30pm - 5:00pm
	Assignment 4 Total Hours: ~7hrs 40 minutes
	----------------------------------
	Assignment 5 Estimated Hours: 10 hrs