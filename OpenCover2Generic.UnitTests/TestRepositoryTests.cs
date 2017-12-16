using BHGE.SonarQube.OpenCover2Generic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestRepositoryTests
    {
        private Mock<IJobFileSystem> _jobFileSystemMock;
        private ITestResultsRepository _testResultsRepository;
        private Mock<IFileSystemAdapter> _fileSystemMock;

        [TestInitialize]
        public void Initialize(){
            _jobFileSystemMock = new Mock<IJobFileSystem>();
            _fileSystemMock = new Mock<IFileSystemAdapter>();
            _testResultsRepository = new TestResultsRepository(_jobFileSystemMock.Object,_fileSystemMock.Object);
        }


        [TestMethod]
        public void Write_EmptyRepository_EmptyFile()
        {

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            _testResultsRepository.Write(writer);
            var result = Encoding.ASCII.GetString(stream.ToArray());
            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<unitTest version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Write_TwoFilesInRepository_ConcatenatedFiles()
        {

            string[] files = {"Resources/1_TestResults.xml", "Resources/2_TestResults.xml" };
            _jobFileSystemMock.Setup(f => f.GetTestResultsFiles()).Returns(files);

            XDocument doc = WhenWriting(_testResultsRepository);

            var fileCount=doc.Root.Elements("file").Count();
            Assert.AreEqual(33, fileCount);

            var testCasesCount = doc.Root.Elements("file").Elements("testCase").Count();
            Assert.AreEqual(186, testCasesCount);
        }

        [TestMethod]
        public void Write_TwoSameFilesInRepository_ExpectOne()
        {
            string[] files = { "Resources/1_TestResults.xml", "Resources/1_TestResults.xml" };
            _jobFileSystemMock.Setup(f => f.GetTestResultsFiles()).Returns(files);
            XDocument doc = WhenWriting(_testResultsRepository);

            var fileCount = doc.Root.Elements("file").Count();
            Assert.AreEqual(1, fileCount);

            var testCasesCount = doc.Root.Elements("file").Elements("testCase").Count();
            Assert.AreEqual(7, testCasesCount);
        }

        [TestMethod]
        public void Add_File_ShouldBeInRepository()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            _jobFileSystemMock.Setup(f => f.GetTestResultsDirectory()).Returns(tempDir);

            var fileToAdd="Resources/1_TestResults.xml";
            _testResultsRepository.Add(fileToAdd);

            _fileSystemMock.Verify(f => f.CopyFile(fileToAdd,tempDir + "\\1_TestResults.xml"),Times.Exactly(1));
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
