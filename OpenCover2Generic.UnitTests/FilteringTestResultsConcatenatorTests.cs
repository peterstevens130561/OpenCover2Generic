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
    public class FilteringTestResultsConcatenatorTests
    {
        private ITestResultsConcatenator _concatenator;
        private MemoryStream _resultStream;

        [TestInitialize]
        public void Initialize()
        {
            _concatenator = new FilteringTestResultsConcatenator();
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
        public void Random()
        {
            string oneFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file />
    <file />
</unitTest>";

            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file />
    <file />
</unitTest>";
            _concatenator.Begin();
            WhenConcatenating(oneFile);
            _concatenator.End();

            ThenResultMatches(expected);


        }

        [TestMethod]
        public void WithEndElements()
        {
            string oneFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file p=""a"" >
    </file>
    <file p=""b"" >
    </file>
</unitTest>";

            _concatenator.Begin();
            WhenConcatenating(oneFile);
            _concatenator.End();
            string expected  = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file p=""a"" />
    <file p=""b"" />
</unitTest>";
            ThenResultMatches(expected);


        }

        [TestMethod]
        public void OneFileWithTestCasesResultsInSame()
        {
            string oneFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"">
    <file path=""E:\Cadence\ESIETooLink\Main\Tests\Bhi.Esie.Services.JobConfiguration.UnitTest\IdGeneratorTest.cs"">
        <testCase name=""Generate_starts_with_1"" duration=""7"" />
        <testCase name=""GenerateShort_starts_with_1"" duration=""0"" />
        <testCase name=""Generate_starts_with_1_increments_by_1"" duration=""0"" />
        <testCase name=""GenerateShort_starts_with_1_increments_by_1"" duration=""0"" />
        <testCase name=""GenerateShort_ThrowsException_when_out_of_range"" duration=""11"" />
        <testCase name=""Generate_ThrowsException_when_out_of_range"" duration=""1"" />
    </file>
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
            Assert.AreEqual(4, _concatenator.TestCases);
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
