using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class ModuleEventArgs : EventArgs
    {

        public ModuleEventArgs(IModuleCoverageModel model)
        {
            Model = model;
        }

        public IModuleCoverageModel Model { get; private set; }
    }
}
