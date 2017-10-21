﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using BHGE.SonarQube.OpenCoverWrapper;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Summary description for OpenCoverWrapperCommandLineParserTests
    /// </summary>
    [TestClass]
    public class OpenCoverWrapperCommandLineParserTests
    {
        public OpenCoverWrapperCommandLineParserTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CheckOutput()
        {
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-output:SomeFile.xml" };
            commandLineParser.Args = line;
            Assert.AreEqual("SomeFile.xml", commandLineParser.GetOutputPath());
        }

        [TestMethod]
        public void CheckTarget()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-target:bla.xml" };
            commandLineParser.Args = line;
            Assert.AreEqual("bla.xml", commandLineParser.GetTargetPath());
        }

        [TestMethod]
        public void CheckTargetArgs()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-targetargs:fun and joy" };
            commandLineParser.Args = line;
            Assert.AreEqual("fun and joy", commandLineParser.GetTargetArgs());
        }

        [TestMethod]
        public void CheckOpenCoverPath()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-opencover:Apps\OpenCover\OpenCover.Console.exe" };
            commandLineParser.Args = line;
            Assert.AreEqual(@"Apps\OpenCover\OpenCover.Console.exe", commandLineParser.GetOpenCoverPath());
        }
    }
}