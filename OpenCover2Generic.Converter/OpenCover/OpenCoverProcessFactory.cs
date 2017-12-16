using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverProcessFactory : IOpenCoverProcessFactory
    {
        private readonly ProcessFactory _processFactory;

        public OpenCoverProcessFactory(ProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public IOpenCoverProcess Create()
        {
            return new OpenCoverProcess(_processFactory.CreateProcess(), new TimerSeam());
        }
    }
}
