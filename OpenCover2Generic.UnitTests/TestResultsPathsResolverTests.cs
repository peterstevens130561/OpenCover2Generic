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
            _fileSystem = new TestResultsPathResolver(_fileSystemAdapterMock.Object);
            _fileSystem.Root = @"Q:\temp\opencover_key";
        }

        [TestMethod]
        public void Root_NotSet_Root_Null()
        {
            _fileSystem.Root = null;
            Assert.IsNull(_fileSystem.Root);
        }

        [TestMethod]
        public void Root_Set_Root_Same()
        {
            _fileSystem.Root = "abc";
            Assert.AreEqual("abc",_fileSystem.Root);
        }
        [TestMethod]
        public void TestResultsDirectory_Specified()
        {
            var testResultsDirectory = _fileSystem.GetDirectory();
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsDestinationPath_Assembly_GetTestResultsDestinationPath_Converted()
        {
            var testResultsDirectory = _fileSystem.GetTestResultsDestinationPath(@"A:B\C\test.dll");
            Assert.AreEqual(@"Q:\temp\opencover_key\TestResults\1_test.xml", testResultsDirectory);
        }

        [TestMethod]
        public void GetTestResultsPaths_TwoPathsInDirectory_GetTestResultsPaths_TwoTestResultsPaths()
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
