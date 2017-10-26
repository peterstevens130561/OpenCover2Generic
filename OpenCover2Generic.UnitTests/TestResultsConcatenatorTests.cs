using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCover2Generic.Converter;
using System.IO;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Summary description for TestResultsConcatenatorTests
    /// </summary>
    [TestClass]
    public class TestResultsConcatenatorTests
    {
        private ITestResultsConcatenator _concatenator;
        private MemoryStream _resultStream;

        [TestInitialize]
        public void Initialize()
        {
            _concatenator = new TestResultsConcatenator();
            _resultStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(_resultStream);
            _concatenator.Writer = new XmlTextWriter(writer);
        }
        [TestMethod]
        public void OneFileResultsInSame()
        {
            string oneFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
	<file path=""E:\Cadence\ESIETooLink\Main\Tests\Bhi.Esie.Calibration.Algorithms.UnitTest\CircularBufferTest.cs"">
		<testCase name=""IsFilled"" duration=""585"" />
		<testCase name=""PushGet_RightOrder"" duration=""15"" />
		<testCase name=""Get_IndexOutOfRangeHigh"" duration=""24"" />
		<testCase name=""Get_IndexOutOfRangeLow"" duration=""3"" />
		<testCase name=""Array_RightOrder"" duration=""5"" />
	</file>
</unitTest>";

            _concatenator.Begin();
            _concatenator.End();
            string expected= @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"" />";
            ThenResultMatches(expected);


        }
        private void ThenResultMatches(string expected) {
            StreamReader resultReader = new StreamReader(new MemoryStream(_resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            TestUtils.AssertStringsSame(expected, text);
        }
    }
}
