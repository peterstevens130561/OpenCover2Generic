using BHGE.SonarQube.OpenCover2Generic.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess()
        {
            return new SimpleProcess();
        }


        public IOpenCoverProcess CreateOpenCoverProcess()
        {
            return new OpenCoverProcess(CreateProcess(),new TimerSeam());
        }
    }
}
