using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageStorageResolverTests
    {

        private ICoverageStorageResolver _resolver;

        [TestInitialize]
        public void Initialize()
        {
            _resolver=new CoverageStorageResolver();
        }

        [TestMethod]
        public void GetPathForModule_Valid_ProperLocation()
        {
            string rootPath = "E:/fun";
            string testAssemblyPath = "F:/assemblies/bla.dll";
            string path = _resolver.GetPathForAssembly(rootPath, testAssemblyPath);
        }
    }
}
