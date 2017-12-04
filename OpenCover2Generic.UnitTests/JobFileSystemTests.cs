using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCover2Generic.Converter;
using Moq;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobFileSystemTests
    {
        private Mock<IFileSystemAdapter> _fileSystemAdapterMock;
        private JobFileSystem _fileSystem;
        [TestInitialize]
        public void Initialize()
        {
            _fileSystemAdapterMock = new Mock<IFileSystemAdapter>();
            _fileSystemAdapterMock.Setup(f => f.GetTempPath()).Returns("Q:/temp");
            _fileSystem= new JobFileSystem(_fileSystemAdapterMock.Object);
        }
        [TestMethod]
        public void CreateRootTests()
        {
            _fileSystem.CreateRoot("key");
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\TestResults"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverLogs"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverIntermediate"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverOutput"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(It.IsAny<string>()), Times.Exactly(5));
        }

        [TestMethod]
        public void CheckIntermediateCoverageDirectory()
        {
            _fileSystem.CreateRoot("key");
            var intermediateCoverageDirectory=_fileSystem.GetIntermediateCoverageDirectory();
            Assert.AreEqual(@"Q:\temp\opencover_key\OpenCoverIntermediate", intermediateCoverageDirectory);
        }

        [TestMethod]
        public void CheckOpenCoverLogPath()
        {
            _fileSystem.CreateRoot("key");
            var openCoverLogPath = _fileSystem.GetOpenCoverLogPath("A:/B/C/test.dll") ;
            Assert.AreEqual(@"Q:\temp\opencover_key\OpenCoverLogs\1_test.log", openCoverLogPath);
        }

        [TestMethod]
        public void CheckOpenCoverOutputPath()
        {
            _fileSystem.CreateRoot("key");
            var openCoverLogPath = _fileSystem.GetOpenCoverOutputPath(@"A:\B\C\output.dll");
            Assert.AreEqual(@"Q:\temp\opencover_key\OpenCoverOutput\1_output.xml", openCoverLogPath);
        }

        [TestMethod]
        public void CheckTestResultsDirectory()
        {
            _fileSystem.CreateRoot("key");
            var testResultsDirectory = _fileSystem.GetTestResultsDirectory();
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsPath_Path_Converted()
        {
            _fileSystem.CreateRoot("key");

            var testResultsDirectory = _fileSystem.GetTestResultsPath(@"A:B\C\test.dll");
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults\1_test.xml", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsPaths_TwoPaths_TwoTestResultsPaths()
        {
            _fileSystem.CreateRoot("key");
            var paths = new Collection<string>();
            paths.Add("a");
            paths.Add("b");

            _fileSystemAdapterMock.Setup(f => f.EnumerateFiles(@"Q:\temp\opencover_key\TestResults")).Returns(paths);

            var testResultsPaths = _fileSystem.GetTestResultsPaths();
            Assert.AreEqual(2,testResultsPaths.Count());
        }


        [TestMethod]
        public void GetIntermediateCoverageOutputPath_SameName_DifferentIndex()
        {
            _fileSystem.CreateRoot("key");
            _fileSystem.GetIntermediateCoverageOutputPath(@"A:B\C\test.dll", "mymodule");
            var intermediateCoverageOuputPath = _fileSystem.GetIntermediateCoverageOutputPath(@"A:B\D\test.dll", "mymodule");
            Assert.AreEqual(@"Q:\temp\opencover_key\OpenCoverIntermediate\mymodule\2_test.xml", intermediateCoverageOuputPath);
        }

        [TestMethod]
        public void GetModuleCoverageDirectories_NoModules_EmptyList()
        {
            var dirs = GivenAModel();

            var modules = _fileSystem.GetModuleCoverageDirectories();

            Assert.IsNotNull(modules);
            Assert.AreEqual(0, modules.Count());
        }


        [TestMethod]
        public void GetModuleCoverageDirectories_TwoModules_ListOfTwo()
        {
            var dirs = GivenAModel();
            dirs.Add("a");
            dirs.Add("b");

            var modules = _fileSystem.GetModuleCoverageDirectories();

            Assert.IsNotNull(modules);
            Assert.AreEqual(2, modules.Count());
        }

        private Collection<string> GivenAModel()
        {
            var dirs = new Collection<string>();
            _fileSystem.CreateRoot("key");
            _fileSystemAdapterMock.Setup(f =>
                f.EnumerateDirectories(It.IsAny<string>(), "*", SearchOption.TopDirectoryOnly)
            ).Returns(dirs);
            return dirs;
        }
    }
}
