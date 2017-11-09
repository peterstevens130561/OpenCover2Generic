using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CommandLineParserTests
    {
        [TestMethod]
        public void ArgumentArraySeperateArguments()
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
        public void ArgumentArrayComma()
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
            catch (ArgumentNullException)
            {
                return;
            }
            Assert.Fail("Expected exception");

        }

    }
}
