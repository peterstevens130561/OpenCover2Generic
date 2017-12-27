using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageRepositoryScannerTests
    {
        private ICodeCoverageRepositoryObservableScanner _observableScanner;
        private Mock<ICoverageStorageResolver> coverageStorageResolverMock;

        [TestInitialize]
        public void Initialize()
        {
            coverageStorageResolverMock = new Mock<ICoverageStorageResolver>();
            _observableScanner = new CodeCoverageRepositoryObservableScanner(coverageStorageResolverMock.Object);
        }


        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnBeginScanEventCalledOnce()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify( o => o.OnBeginScan(It.IsAny<object>(),It.IsAny<EventArgs>()),Times.Once);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnEndScanEventCalledOnce()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify(o => o.OnEndScan(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnBeginModuleEventNeverCalled()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify(o => o.OnBeginModule(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnEndModuleEventNeverCalled()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify(o => o.OnEndModule(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never);

        }

        [TestMethod]
        public void Scan_OneModule_Scan_OnBeginModuleCalled()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            List<string> oneModule = new List<string>();
            oneModule.Add("a");
            coverageStorageResolverMock.Setup(c => c.GetPathsOfAllModules(It.IsAny<string>())).Returns(oneModule);
            _observableScanner.Scan();
            observerMock.Verify(o => o.OnBeginModule(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);
        }
 
    }
}
