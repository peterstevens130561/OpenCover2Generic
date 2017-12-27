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
        private int _timesOnBeginScanCalled = 0;
        private int _timesOnEndScanCalled = 0;

        [TestInitialize]
        public void Initialize()
        {
            _observableScanner = new CodeCoverageRepositoryObservableScanner();
        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnBeginScanCalled()
        {
            _observableScanner.OnBeginScan += OnBeginScan;
            _observableScanner.Scan();
            Assert.AreEqual(1, _timesOnBeginScanCalled);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnEndScanCalled()
        {
            _observableScanner.OnEndScan += OnEndScan;
            _observableScanner.Scan();
            Assert.AreEqual(1, _timesOnEndScanCalled);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnBeginScanDelegateCalled()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify( o => o.OnBeginScan(It.IsAny<object>(),It.IsAny<EventArgs>()),Times.Once);

        }

        [TestMethod]
        public void Scan_EmptyRepository_Scan_OnEndScanDelegateCalled()
        {
            Mock<IScannerObserver> observerMock = new Mock<IScannerObserver>();
            _observableScanner.AddObserver(observerMock.Object);
            _observableScanner.Scan();
            observerMock.Verify(o => o.OnEndScan(It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Once);

        }

        private void OnBeginScan(object sender, EventArgs e)
        {
            ++_timesOnBeginScanCalled;
        }

        private void OnEndScan(object sender, EventArgs e)
        {
            ++_timesOnEndScanCalled;
        }

 
    }
}
