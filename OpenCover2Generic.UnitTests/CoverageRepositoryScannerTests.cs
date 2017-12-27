using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
