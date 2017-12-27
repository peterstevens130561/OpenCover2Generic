using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CodeCoverageRepositoryObservableScanner : ICodeCoverageRepositoryObservableScanner
    {
        public event EventHandler<EventArgs> OnBeginScan;

        public void Scan()
        {
            throw new NotImplementedException();
        }
    }
}
