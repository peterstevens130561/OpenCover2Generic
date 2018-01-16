using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestResultsPathsResolverTests
    {
        private ITestResultsPathResolver _fileSystem;
        [TestInitialize]
        public void Initialize()
        {
            _fileSystem = new TestResultsPathResolver();
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
            Assert.AreEqual(2, testResultsPaths.Count());
        }
    }
}
