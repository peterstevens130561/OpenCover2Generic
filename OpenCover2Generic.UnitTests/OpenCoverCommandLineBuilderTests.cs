using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCover2Generic.Converter;
using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverCommandLineBuilderTests
    {
        [TestMethod]
        public void ParseOpenCoverCommandLineArgs()
        {
            ICommandLineParser commandLineParser = new CommandLineParser();
            IOpenCoverCommandLineBuilder builder = new OpenCoverCommandLineBuilder(commandLineParser);
            string[] line = { "-target:bla.exe", "-targetargs:a,b", @"-opencover:Apps\OpenCover\OpenCover.Console.exe" };
            builder.Args = line;
            ProcessStartInfo processStartInfo= builder.Build("test.dll","opencover.xml");
            
            Assert.AreEqual(@"Apps\OpenCover\OpenCover.Console.exe", processStartInfo.FileName);
            Assert.AreEqual(@"-register:user ""-output:opencover.xml"" ""-target:bla.exe"" ""-targetargs:a,b test.dll""", processStartInfo.Arguments);
        }

    }
}
