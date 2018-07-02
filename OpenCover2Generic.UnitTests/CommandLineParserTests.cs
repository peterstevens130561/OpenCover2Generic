using System;
using BHGE.SonarQube.OpenCover2Generic.CoverageConverters.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CommandLineParserTests
    {
        [TestMethod]
        public void GetArgumentArray_SeperateArguments_ExpectArray()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();
            string[] line = { @"-testassembly:a:/My Documents/fun.dll", "-testassembly:second.dll" };
            commandLineParser.Args = line;
            string[] assemblies = commandLineParser.GetArgumentArray("-testassembly");
            Assert.AreEqual(2, assemblies.Length);
            Assert.AreEqual("a:/My Documents/fun.dll", assemblies[0]);
            Assert.AreEqual("second.dll", assemblies[1]);
        }

        [TestMethod]
        public void GetArgumentArray_SeperatedByComma_ExpectArray()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();
            string[] line = { @"-testassembly:a:/My Documents/fun.dll,second.dll" };
            commandLineParser.Args = line;
            string[] assemblies = commandLineParser.GetArgumentArray("-testassembly");
            Assert.AreEqual(2, assemblies.Length);
            Assert.AreEqual("a:/My Documents/fun.dll", assemblies[0]);
            Assert.AreEqual("second.dll", assemblies[1]);
        }

        [TestMethod]
        public void GetArgument_ArgsNotSet_ExpectException()
        {
            //Givenparser with Args not set
            ICommandLineParser commandLineParser = new CommandLineParser();

            //When
            try
            {
                commandLineParser.GetArgument("-testassembly");
            }  catch (ArgumentNullException )
            {
                return;
            }
            Assert.Fail("Expected exception");

        }

        [TestMethod]
        public void GetArgumentArray_ArgsNotSet_ExpectException()
        {
            //Givenparser with Args not set
            ICommandLineParser commandLineParser = new CommandLineParser();

            //When
            try
            {
                commandLineParser.GetArgumentArray("-testassembly");
            }
            //then
            catch (ArgumentNullException)
            {
                return;
            }
            Assert.Fail("Expected exception");

        }

        [TestMethod]
        public void GetOptionalPositiveInt_Negative_ExpectException()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();

            //When
            try
            {
                string[] line = { @"-argument:-1" };
                commandLineParser.Args = line;
                commandLineParser.GetOptionalPositiveInt("-argument","5",0);
            }
            //then
            catch (CommandLineArgumentException)
            {
                return;
            }
            Assert.Fail("Expected exception");
        }

        [TestMethod]
        public void GetOptionalPositiveInt_Ok_ExpectValue()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();

            string[] line = { @"-argument:3" };
            commandLineParser.Args = line;
            var value=commandLineParser.GetOptionalPositiveInt("-argument", "5", 0);

            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void GetOptionalPositiveInt_NotProvided_ExpectDefault()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();

            string[] line = { @"" };
            commandLineParser.Args = line;
            var value = commandLineParser.GetOptionalPositiveInt("-argument", "5", 0);

            Assert.AreEqual(5, value);
        }
    }
}
