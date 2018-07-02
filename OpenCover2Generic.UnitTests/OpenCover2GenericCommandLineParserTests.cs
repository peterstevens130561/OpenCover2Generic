using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCover2GenericCommandLineParserTests
    {
        [TestMethod]
        public void TwoArgmentsCheckOpenCoverShouldParse()
        {
            string[] args= { "-OpenCover:SomeFile.xml", "-Generic:SomeResult.xml" };
            IOpenCover2GenericCommandLineParser parser = new OpenCover2GenericCommandLineParser(new CommandLineParser());
            parser.Args = args;
            Assert.AreEqual("SomeFile.xml", parser.OpenCoverPath());
        }


        [TestMethod]
        public void TwoArgmentsCheckGenericShouldParse()
        {
            string[] args = { "-OpenCover:SomeFile.xml", "-Generic:SomeResult.xml" };
            IOpenCover2GenericCommandLineParser parser = new OpenCover2GenericCommandLineParser(new CommandLineParser());
            parser.Args = args;
            Assert.AreEqual("SomeResult.xml", parser.GenericPath());
        }
    }
}
