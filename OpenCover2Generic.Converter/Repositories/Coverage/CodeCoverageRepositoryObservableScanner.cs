using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CodeCoverageRepositoryObservableScanner : ICodeCoverageRepositoryObservableScanner
    {
        private readonly ICoverageStorageResolver coverageStorageResolver;

        public CodeCoverageRepositoryObservableScanner(ICoverageStorageResolver coverageStorageResolver)
        {
            this.coverageStorageResolver = coverageStorageResolver;
        }

        public event EventHandler<EventArgs> OnBeginScan;
        public event EventHandler<EventArgs> OnEndScan;
        public event EventHandler<EventArgs> OnBeginModule;
        public void Scan()
        {
            OnBeginScan?.Invoke(this,EventArgs.Empty);

            OnEndScan?.Invoke(this, EventArgs.Empty);
        }

        public void AddObserver(IScannerObserver observer)
        {
            OnBeginScan += observer.OnBeginScan;
            OnEndScan += observer.OnEndScan;
            OnBeginModule += observer.OnBeginModule;
        }
    }
}
