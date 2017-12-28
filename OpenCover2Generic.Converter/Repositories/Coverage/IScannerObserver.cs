using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface IScannerObserver
    {
        void OnBeginScan(object sender, EventArgs eventArgs);
        void OnEndScan(object sender, EventArgs eventArgs);
        void OnBeginModule(object sender, EventArgs eventArgs);
        void OnEndModule(object sender, EventArgs eventArgs);
        void OnModule(object v, ModuleEventArgs moduleEventArgs);
    }
}
