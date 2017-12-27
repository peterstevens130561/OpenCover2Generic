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
        public event EventHandler<EventArgs> OnBeginScan;
        public event EventHandler<EventArgs> OnEndScan;
        private ICollection<IScannerObserver> scannerObservers = new Collection<IScannerObserver>();
        public void Scan()
        {
            OnBeginScan?.Invoke(this,EventArgs.Empty);
            OnEndScan?.Invoke(this, EventArgs.Empty);
        }

        public void AddObserver(IScannerObserver observer)
        {
            scannerObservers.Add(observer);
        }
    }
}
