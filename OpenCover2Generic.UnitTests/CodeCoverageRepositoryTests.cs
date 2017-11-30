using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Repositories;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CodeCoverageRepositoryTests
    {
        private ICodeCoverageRepository _repository;
        [TestInitialize]
        public void Initialize()
        {
            _repository = new CodeCoverageRepository();
            _repository.RootDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }
        [TestMethod]
        public void Instantiate()
        {
            ICodeCoverageRepository repository = new CodeCoverageRepository();
            Assert.IsNotNull(repository);
        }

        /// <summary>
        /// We may have a coverage file with only skipped modules. In that case there
        /// should be nothing stored
        /// </summary>
        [TestMethod]
        public void Add_EmptyFile_ShouldBeInRightPlace()
        {
            string path = @"Resources/OnlySkippedModules.xml";
            _repository.Add(path,@"key");

        }

        [TestMethod]
        public void AddFileWithSomeModules_ShouldBeOnCorrectPlaces()
        {
            
        }

    }
}
