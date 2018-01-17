using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageStorageResolverTests
    {

        private ICoverageRepositoryPathResolver _resolver;
        private Mock<IFileSystemAdapter> _fileSystemMock;

        [TestInitialize]
        public void Initialize()
        {
            _fileSystemMock = new Mock<IFileSystemAdapter>();
            _resolver=new CoverageRepositoryPathResolver(_fileSystemMock.Object);
        }


        [TestMethod]
        public void GetPathForModule_RootSet_Valid_GetPath_ProperLocation()
        {
            const string rootPath = @"E:\fun";
            const string testAssemblyPath = @"F:/assemblies/bla.dll";
            const string moduleName = @"module";
            _resolver.Root = @"E:\fun";

            string path = _resolver.GetPathForAssembly(moduleName, testAssemblyPath);
            Assert.AreEqual(@"E:\fun\OpenCoverIntermediate\module\bla.xml", path);
            _fileSystemMock.Verify(f => f.CreateDirectory(@"E:\fun\OpenCoverIntermediate"));
        }

        [TestMethod]
        public void GetPathsOfAllModules_RootSet_Empty_EmptyList()
        {
            const string rootPath = @"E:\fun";
            _resolver.Root = @"E:\fun";
            _fileSystemMock.Setup(f => f.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly)).Returns(new List<string>());

            IEnumerable<string> paths = _resolver.GetPathsOfAllModules();

            Assert.IsNotNull(paths);
            Assert.AreEqual(0, paths.Count());
        }


        [TestMethod]
        public void GetPathsOfAllModules_OneModule_ListWithOne()
        {
            const string rootPath = @"E:\fun";
            var list = new List<string>();
            _resolver.Root = @"E:\fun";
            _fileSystemMock.Setup(f => f.EnumerateDirectories(@"E:\fun\OpenCoverIntermediate", "*", SearchOption.TopDirectoryOnly)).Returns(list);
            list.Add(@"E:\fun\OpenCoverIntermediate\bla");

            IEnumerable<string> paths = _resolver.GetPathsOfAllModules();

            Assert.IsNotNull(paths);
            Assert.AreEqual(1, paths.Count());
            Assert.AreEqual(@"E:\fun\OpenCoverIntermediate\bla", paths.First());
        }

        [TestMethod]
        public void Directory_RootNotSet_Exception()
        {
            _resolver.Root = null;
            try
            {
                string path = _resolver.GetDirectory();
            }
            catch (InvalidOperationException)
            {
                return;
            }
            Assert.Fail("Expected exception");
        }
    }
}
