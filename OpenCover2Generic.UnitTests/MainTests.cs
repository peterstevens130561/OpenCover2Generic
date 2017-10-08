using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic;
using System.IO;
using System.Text;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class UnitTest1
    {
        private IConverter converter;
        [TestMethod]
        public void EmptyModuleOnlyShouldCreateHeaderOnly()
        {
            converter = new Converter();
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();
            string result = WhenConverting(resultStream,input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkippedModuleOnlyShouldBeIgnored()
        {
            converter = new Converter();
            string input= @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
  <Modules>
    <Module skippedDueTo=""Filter"" hash=""CF-B4-AA-F9-F3-33-2A-2E-60-04-2A-3E-24-D0-0D-04-B8-49-54-84"">
      <ModulePath>C:\windows\Microsoft.Net\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll</ModulePath>
      <ModuleTime>2016-07-14T12:43:42Z</ModuleTime>
      <ModuleName>mscorlib</ModuleName>
      <Classes />
    </Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";
            Assert.AreEqual(expected, result);
        }
        private string WhenConverting(MemoryStream resultStream, string input)
        {
            StreamWriter writer = new StreamWriter(resultStream);
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            StreamReader reader = new StreamReader(inputStream);
            converter.Convert(writer, reader);
            StreamReader resultReader = new StreamReader(new MemoryStream(resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            return text;
        }
    }
}
