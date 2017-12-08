using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenCover2Generic.Converter;

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
    }
}
