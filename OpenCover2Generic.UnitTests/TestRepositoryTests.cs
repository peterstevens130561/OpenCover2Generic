using BHGE.SonarQube.OpenCover2Generic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenCover2Generic.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestRepositoryTests
    {
        [TestMethod]
        public void Write_EmptyRepository_EmptyFile()
        {
            var fileSystemMock = new Mock<IJobFileSystem>();
            ITestResultsRepository testResultsRepository = new TestResultsRepository(fileSystemMock.Object);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            testResultsRepository.Write(writer);
            var result = Encoding.ASCII.GetString(stream.ToArray());
            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Write_TwoFilesInRepository_ConcatenatedFiles()
        {
            var fileSystemMock = new Mock<IJobFileSystem>();
            ITestResultsRepository testResultsRepository = new TestResultsRepository(fileSystemMock.Object);
            string[] files = {"Resources/1_TestResults.xml", "Resources/2_TestResults.xml" };
            fileSystemMock.Setup(f => f.GetTestResultsFiles()).Returns(files);

            XDocument doc = WhenWriting(testResultsRepository);

            var fileCount=doc.Root.Elements("file").Count();
            Assert.AreEqual(33, fileCount);

            var testCasesCount = doc.Root.Elements("file").Elements("testCase").Count();
            Assert.AreEqual(186, testCasesCount);
        }

        [TestMethod]
        public void Write_TwoSameFilesInRepository_ExpectOne()
        {
            var fileSystemMock = new Mock<IJobFileSystem>();
            ITestResultsRepository testResultsRepository = new TestResultsRepository(fileSystemMock.Object);
            string[] files = { "Resources/1_TestResults.xml", "Resources/1_TestResults.xml" };
            fileSystemMock.Setup(f => f.GetTestResultsFiles()).Returns(files);
            XDocument doc = WhenWriting(testResultsRepository);

            var fileCount = doc.Root.Elements("file").Count();
            Assert.AreEqual(1, fileCount);

            var testCasesCount = doc.Root.Elements("file").Elements("testCase").Count();
            Assert.AreEqual(7, testCasesCount);
        }

        [TestMethod]
        public void Add_File_ShouldBeInRepository()
        {
            var fileSystemMock = new Mock<IJobFileSystem>();
            ITestResultsRepository testResultsRepository = new TestResultsRepository(fileSystemMock.Object);

            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            fileSystemMock.Setup(f => f.GetTestResultsDirectory()).Returns(tempDir);

            testResultsRepository.Add("Resources/1_TestResults.xml");

            var resultsFiles = Directory.EnumerateFiles(tempDir);
            Assert.IsNotNull(resultsFiles);
            Assert.AreEqual(1, resultsFiles.Count());
            Assert.AreEqual("1_TestResults.xml", resultsFiles.First());
        }

        private static XDocument WhenWriting(ITestResultsRepository testResultsRepository)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            testResultsRepository.Write(writer);
            var result = Encoding.ASCII.GetString(stream.ToArray());

            XDocument doc = XDocument.Parse(result);
            return doc;
        }
    }
}
