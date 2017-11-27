using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using BHGE.SonarQube.OpenCoverWrapper;
using BHGE.SonarQube.OpenCover2Generic.Utils;

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

        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
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
        public void GetOutputPath_ValidValue_ShouldMatch()
        {
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-output:SomeFile.xml" };
            commandLineParser.Args = line;
            Assert.AreEqual("SomeFile.xml", commandLineParser.GetOutputPath());
        }

        [TestMethod]
        public void GetTargetPath_ValidValue_ShouldMatch()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-target:bla.xml" };
            commandLineParser.Args = line;
            Assert.AreEqual("bla.xml", commandLineParser.GetTargetPath());
        }

        [TestMethod]
        public void GetTargetArgs_ValidValue_ShouldMatch()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { "-targetargs:fun and joy" };
            commandLineParser.Args = line;
            Assert.AreEqual("fun and joy", commandLineParser.GetTargetArgs());
        }

        [TestMethod]
        public void GetOpenCoverPath_ValueValue_ShouldMath()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-opencover:Apps\OpenCover\OpenCover.Console.exe" };
            commandLineParser.Args = line;
            Assert.AreEqual(@"Apps\OpenCover\OpenCover.Console.exe", commandLineParser.GetOpenCoverPath());
        }

        [TestMethod]
        public void GetTestResultsPath_ValidValue_ShouldMatch()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-testresults:../testresults.xml" };
            commandLineParser.Args = line;
            Assert.AreEqual(@"../testresults.xml", commandLineParser.GetTestResultsPath());
        }

        [TestMethod]
        public void GetTestAssemblies_SpecifyTwo_ShouldMatch()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-testassembly:a:/My Documents/fun.dll", "-testassembly:second.dll"};
            commandLineParser.Args = line;
            string[] assemblies = commandLineParser.GetTestAssemblies();
            Assert.AreEqual(2, assemblies.Length);
            Assert.AreEqual("a:/My Documents/fun.dll", assemblies[0]);
            Assert.AreEqual("second.dll", assemblies[1]);
        }

        [TestMethod]
        public void GetParallelJobs_SpecifyInvalid_ExpectException()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-parallel:none" };
            commandLineParser.Args = line;
            try
            {
                commandLineParser.GetParallelJobs();
            } catch (CommandLineArgumentException) 
            {
                return;
            }
            Assert.Fail("expected argumentexception");
        }

        [TestMethod]
        public void GetParallelJobs_Specify0_ExpectException()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-parallel:0" };
            commandLineParser.Args = line;
            try
            {
                commandLineParser.GetParallelJobs();
            }
            catch (CommandLineArgumentException)
            {
                return;
            }
            Assert.Fail("expected argumentexception");
        }

        [TestMethod]
        public void GetParallelJobs_Specify1_Same()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-parallel:1" };
            commandLineParser.Args = line;
            int jobs=commandLineParser.GetParallelJobs();

            Assert.AreEqual(1, jobs);
        }

        [TestMethod]
        public void GetParallelJobs_NotSpecified_One()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"" };
            commandLineParser.Args = line;
            int jobs = commandLineParser.GetParallelJobs();

            Assert.AreEqual(1, jobs);
        }

        [TestMethod]
        public void GetJobTimeOut_OneMinute_ExpectOneMinute()
        {
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            string[] line = { @"-jobtimeout:1" };
            commandLineParser.Args = line;
            TimeSpan timeSpan= commandLineParser.GetJobTimeOut();

            Assert.AreEqual(60, timeSpan.TotalSeconds);
        }
    }
}
