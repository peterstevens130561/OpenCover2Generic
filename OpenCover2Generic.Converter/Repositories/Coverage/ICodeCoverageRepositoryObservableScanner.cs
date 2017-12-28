using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICodeCoverageRepositoryObservableScanner
    {
        void Scan();
        void AddObserver(IScannerObserver scannerObserver);
    }
}
