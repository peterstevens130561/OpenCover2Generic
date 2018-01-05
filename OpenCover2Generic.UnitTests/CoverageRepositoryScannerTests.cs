using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageRepositoryScannerTests
    {
        private IQueryAllModulesObservable _observable;
        private Mock<ICoverageStorageResolver> _coverageStorageResolverMock;
        private Mock<ICoverageParser> _moduleParserMock;

        [TestInitialize]
        public void Initialize()
        {
            _coverageStorageResolverMock = new Mock<ICoverageStorageResolver>();
            _moduleParserMock=new Mock<ICoverageParser>();
            _observable = new QueryAllModulesObservable(_coverageStorageResolverMock.Object,_moduleParserMock.Object);
        }


        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnBeginScanEventCalledOnce()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = new Mock<IQueryAllModulesResultObserver>();
            _observable.AddObserver(observerMock.Object);
            _observable.Execute();
            observerMock.Verify( o => o.OnBeginScan(It.IsAny<object>(),It.IsAny<EventArgs>()),Times.Once);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnEndScanEventCalledOnce()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = new Mock<IQueryAllModulesResultObserver>();
            _observable.AddObserver(observerMock.Object);
            _observable.Execute();
            observerMock.Verify(o => o.OnEndScan(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);

        }


        [TestMethod]
        public void Scan_OneModuleWithOneFile_Scan_ParserCalledOnce()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = GivenOneModule();
            GivenOneFile();
            _observable.Execute();
            ThenParsed("f1");

            
        }

        [TestMethod]
        public void Scan_OneModuleWithTwoFile_Scan_ParserCalledTwice()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = GivenOneModule();
            GivenTwoFiles();
            _observable.Execute();
            ThenParsed("f1");
            ThenParsed("f2");

        }

        [TestMethod]
        public void Scan_OneModuleWithTwoFile_Scan_OnModuleCalledOnce()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = GivenOneModule();
            GivenTwoFiles();
            _observable.Execute();
            observerMock.Verify(o => o.OnModule(It.IsAny<object>(), It.IsAny<ModuleEventArgs>()), Times.Once);

        }

        private Mock<IQueryAllModulesResultObserver> GivenOneModule()
        {
            Mock<IQueryAllModulesResultObserver> observerMock = new Mock<IQueryAllModulesResultObserver>();
            _observable.AddObserver(observerMock.Object);
            List<string> oneModule = new List<string>();
            oneModule.Add("a");
            _coverageStorageResolverMock.Setup(c => c.GetPathsOfAllModules(It.IsAny<string>())).Returns(oneModule);
            return observerMock;
        }

        private void GivenOneFile()
        {
            List<string> files = new List<string>();
            files.Add("f1");
            _coverageStorageResolverMock.Setup(c => c.GetTestCoverageFilesOfModule(It.IsAny<string>())).Returns(files);
        }

        private void GivenTwoFiles()
        {
            List<string> files = new List<string>();
            files.Add("f1");
            files.Add("f2");
            _coverageStorageResolverMock.Setup(c => c.GetTestCoverageFilesOfModule(It.IsAny<string>())).Returns(files);
        }

        private void ThenParsed(string name)
        {
            _moduleParserMock.Verify(m => m.ParseFile(It.IsAny<IntermediateEntity>(), name), Times.Once);
        }
    }
}
