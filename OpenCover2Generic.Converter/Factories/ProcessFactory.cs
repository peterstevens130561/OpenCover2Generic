using BHGE.SonarQube.OpenCover2Generic.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess()
        {
            return new SimpleProcess();
        }
    }
}
