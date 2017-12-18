using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageStorageResolverTests
    {

        private ICoverageStorageResolver _resolver;
        private Mock<IFileSystemAdapter> _fileSystemMock;

        [TestInitialize]
        public void Initialize()
        {
            _fileSystemMock = new Mock<IFileSystemAdapter>();
            _resolver=new CoverageStorageResolver(_fileSystemMock.Object);
        }

        [TestMethod]
        public void GetPathForModule_Valid_ProperLocation()
        {
            const string rootPath = @"E:\fun";
            const string testAssemblyPath = @"F:/assemblies/bla.dll";
            const string moduleName = @"module";
            string path = _resolver.GetPathForAssembly(rootPath, moduleName, testAssemblyPath);
            Assert.AreEqual(@"E:\fun\module\bla.xml",path);
        }

        [TestMethod]
        public void GetPathsOfAllModules_Empty_EmptyList()
        {
            const string rootPath = @"E:\fun";
            _fileSystemMock.Setup(f => f.EnumerateDirectories(rootPath,"*",SearchOption.TopDirectoryOnly)).Returns(new List<string>());

            IEnumerable<string> paths = _resolver.GetPathsOfAllModules(rootPath);

            Assert.IsNotNull(paths);
            Assert.AreEqual(0,paths.Count());
        }


        [TestMethod]
        public void GetPathsOfAllModules_OneModule_ListWithOne()
        {
            const string rootPath = @"E:\fun";
            var list = new List<string>();
            _fileSystemMock.Setup(f => f.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly)).Returns(list);
            list.Add(@"E:\fun\bla");

            IEnumerable<string> paths = _resolver.GetPathsOfAllModules(rootPath);

            Assert.IsNotNull(paths);
            Assert.AreEqual(1, paths.Count());
            Assert.AreEqual(@"E:\fun\bla", paths.First());
        }
    }
}
