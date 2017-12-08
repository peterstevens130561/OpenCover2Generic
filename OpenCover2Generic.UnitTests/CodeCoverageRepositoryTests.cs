using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using Moq;
using OpenCover2Generic.Converter;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CodeCoverageRepositoryTests
    {
        Mock<IJobFileSystem> _jobFileSystemMock = new Mock<IJobFileSystem>();
        private Mock<IOpenCoverOutput2RepositorySaver> _saver = new Mock<IOpenCoverOutput2RepositorySaver>();
        private ICodeCoverageRepository _repository;
        [TestInitialize]
        public void Initialize()
        {
            _repository = new CodeCoverageRepository(_jobFileSystemMock.Object,_saver.Object);
            _repository.RootDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }
        [TestMethod]
        public void Instantiate()
        {
            ICodeCoverageRepository repository = new CodeCoverageRepository(_jobFileSystemMock.Object, _saver.Object);
            Assert.IsNotNull(repository);
        }

        /// <summary>
        /// We may have a coverage file with only skipped modules. In that case there
        /// should be nothing stored
        /// </summary>
        [TestMethod]

        public void CreateCoverageFile_Nothing_ValidEmptyFile()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(new StreamWriter(new MemoryStream())))
            {
                var dirs = new Collection<string>();
                _jobFileSystemMock.Setup(j => j.GetModuleCoverageDirectories()).Returns(dirs);
                _repository.CreateCoverageFile(writer);

                _saver.Verify(s => s.BeginCoverageFile(writer), Times.Exactly(1));
                _saver.Verify(s => s.EndCoverageFile(writer),Times.Exactly(1));
                _saver.Verify(s=>s.AppendModuleToCoverageFile(writer),Times.Exactly(0));
                _saver.Verify(s => s.BeginModule(), Times.Exactly(0));
            }
        }

        [TestMethod]
        public void CreateCoverageFile_OneModuleOneFile_FileWithOneModuleAndFile()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(new StreamWriter(new MemoryStream())))
            {
                var dirs = new Collection<string>();
                dirs.Add("a");
                var files = new Collection<string>();
                files.Add("b");
                _jobFileSystemMock.Setup(j => j.GetModuleCoverageDirectories()).Returns(dirs);
                _jobFileSystemMock.Setup(j => j.GetTestCoverageFilesOfModule("a")).Returns(files);
                _repository.CreateCoverageFile(writer);

                ThenFileIsWritten(writer);
                _saver.Verify(s => s.BeginModule(), Times.Exactly(1));
                ThenFileIsRead("b");
                ThenModuleIsAppended(writer, 1);

            }
        }



        [TestMethod]
        public void CreateCoverageFile_OneModuleTwoFiles_FileWithOneModuleTwoFiles()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(new StreamWriter(new MemoryStream())))
            {
                var dirs = new Collection<string>();
                _jobFileSystemMock.Setup(j => j.GetModuleCoverageDirectories()).Returns(dirs);
                dirs.Add("a");

                var files = new Collection<string>();
                files.Add("b");
                files.Add("c");

                _jobFileSystemMock.Setup(j => j.GetTestCoverageFilesOfModule("a")).Returns(files);
                _repository.CreateCoverageFile(writer);
                ThenFileIsWritten(writer);
                ThenThereAreModules(1);
                ThenFileIsRead("b");
                ThenFileIsRead("c");
                ThenModuleIsAppended(writer, 1);


            }
        }

        private void ThenThereAreModules(int modulesCount)
        {
            _saver.Verify(s => s.BeginModule(), Times.Exactly(modulesCount));
        }

        private void ThenFileIsWritten(XmlTextWriter writer)
        {
            _saver.Verify(s => s.BeginCoverageFile(writer), Times.Exactly(1));
            _saver.Verify(s => s.EndCoverageFile(writer), Times.Exactly(1));
        }

        private void ThenFileIsRead(string name)
        {
            _saver.Verify(s => s.ReadIntermediateFile(name), Times.Exactly(1));
        }

        private void ThenModuleIsAppended(XmlTextWriter writer,int times)
        {
            _saver.Verify(s => s.AppendModuleToCoverageFile(writer), Times.Exactly(times));
        }
    }
}
