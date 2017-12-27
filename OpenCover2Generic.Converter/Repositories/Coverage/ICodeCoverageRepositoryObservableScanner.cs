using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    interface ICodeCoverageRepositoryObservableScanner
    {
        event EventHandler<EventArgs> OnBeginScan;
        event EventHandler<EventArgs> OnEndScan;
        void Scan();
    }
}
