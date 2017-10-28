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
        public void NoFileResultsInValidEmptyFile()
        {
            

            _concatenator.Begin();
            _concatenator.End();
            string expected= @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"" />";
            ThenResultMatches(expected);


        }

        [TestMethod]
        public void OneFileResultsInSame()
        {
            string oneFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file path=""E:\Cadence\ESIETooLink\Main\Tests\Bhi.Esie.Calibration.Algorithms.UnitTest\CircularBufferTest.cs"" />
</unitTest>";

            _concatenator.Begin();
            WhenConcatenating(oneFile);
            _concatenator.End();

            ThenResultMatches(oneFile);


        }

        [TestMethod]
        public void TwoFilesShouldBeConcatenated()
        {
            string firstFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
	<file path=""E:\Cadence\ESIETooLink\Main\Tests\Bhi.Esie.Calibration.Algorithms.UnitTest\CircularBufferTest.cs"">
		<testCase name=""TestA"" duration=""585"" />
		<testCase name=""TestB"" duration=""15"" />
	</file>
</unitTest>";
            string secondFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
	<file path=""E:\SecondTests.cs"">
		<testCase name=""TestC"" duration=""585"" />
		<testCase name=""TestD"" duration=""15"" />
	</file>
</unitTest>";

            string concatenatedFile =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
	<file path=""E:\Cadence\ESIETooLink\Main\Tests\Bhi.Esie.Calibration.Algorithms.UnitTest\CircularBufferTest.cs"">
		<testCase name=""TestA"" duration=""585"" />
		<testCase name=""TestB"" duration=""15"" />
	</file>
	<file path=""E:\SecondTests.cs"">
		<testCase name=""TestC"" duration=""585"" />
		<testCase name=""TestD"" duration=""15"" />
	</file>
</unitTest>";
            _concatenator.Begin();
            WhenConcatenating(firstFile);
            WhenConcatenating(secondFile);
            _concatenator.End();

            ThenResultMatches(concatenatedFile);
        }

        private void WhenConcatenating(string input)
        {
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            XmlReader xmlReader = XmlReader.Create(inputStream);
            _concatenator.Concatenate(xmlReader);
        }

        private void ThenResultMatches(string expected) {
            StreamReader resultReader = new StreamReader(new MemoryStream(_resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            TestUtils.AssertStringsSame(expected, text);
        }
    }
}
