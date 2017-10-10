using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CommandLineParserTests
    {
        [TestMethod]
        public void TwoArgmentsShouldParse()
        {
            string[] args= { "-OpenCover:SomeFile.xml", "-Generic:SomeResult.xml" };
            ICommandLineParser = new CommandLineParserTests();
        }

    }
}
