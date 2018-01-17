using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;

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
        public void CreateRoot_Invoke_CreateRoot_DirectoriesCreated()
        {

            _fileSystem.CreateRoot("key");
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverLogs"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverIntermediate"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(@"Q:\temp\opencover_key\OpenCoverOutput"));
            _fileSystemAdapterMock.Verify(f => f.CreateDirectory(It.IsAny<string>()), Times.Exactly(4));
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
        public void GetIntermediateCoverageOutputPath_SameName_DifferentIndex()
        {
            _fileSystem.CreateRoot("key");
            _fileSystem.GetIntermediateCoverageOutputPath(@"A:B\C\test.dll", "mymodule");
            var intermediateCoverageOuputPath = _fileSystem.GetIntermediateCoverageOutputPath(@"A:B\D\test.dll", "mymodule");
            Assert.AreEqual(@"Q:\temp\opencover_key\OpenCoverIntermediate\mymodule\2_test.xml", intermediateCoverageOuputPath);
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
