using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestResultsPathsResolverTests
    {
        private Mock<IFileSystemAdapter> _fileSystemAdapterMock;
        private ITestResultsPathResolver _fileSystem;
        [TestInitialize]
        public void Initialize()
        {
            _fileSystemAdapterMock = new Mock<IFileSystemAdapter>();
            _fileSystemAdapterMock.Setup(f => f.GetTempPath()).Returns("Q:/temp");
            _fileSystem = new TestResultsPathResolver(_fileSystemAdapterMock.Object);
            _fileSystem.Root = @"Q:\temp\opencover_key";
        }
        [TestMethod]
        public void CheckTestResultsDirectory()
        {
            var testResultsDirectory = _fileSystem.GetDirectory();
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsPath_Path_Converted()
        {
            var testResultsDirectory = _fileSystem.GetResultsPath(@"A:B\C\test.dll");
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults\1_test.xml", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsPaths_TwoPaths_TwoTestResultsPaths()
        {
            var paths = new Collection<string>();
            paths.Add("a");
            paths.Add("b");

            _fileSystemAdapterMock.Setup(f => f.EnumerateFiles(@"Q:\temp\opencover_key\TestResults")).Returns(paths);

            var testResultsPaths = _fileSystem.GetTestResultsPaths();
            Assert.AreEqual(2, testResultsPaths.Count());
        }
    }
}
