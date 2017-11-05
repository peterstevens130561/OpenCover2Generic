using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCover2Generic.Converter;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobFileSystemTests
    {
        private Mock<IFileSystemAdapter> _mock;
        private JobFileSystem _fileSystem;
        [TestInitialize]
        public void Initialize()
        {
            _mock = new Mock<IFileSystemAdapter>();
            _mock.Setup(f => f.GetTempPath()).Returns("Q:/temp");
            _fileSystem= new JobFileSystem(_mock.Object);
        }
        [TestMethod]
        public void CreateRootTests()
        {
            _fileSystem.CreateRoot("key");
            _mock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key"));
            _mock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\TestResults"));
            _mock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverLogs"));
            _mock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverIntermediate"));
            _mock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverOutput"));
            _mock.Verify(f => f.CreateDirectory(It.IsAny<string>()), Times.Exactly(5));
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
    }
}
