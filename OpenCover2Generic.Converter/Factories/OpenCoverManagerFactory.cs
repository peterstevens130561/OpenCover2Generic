using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System.Timers;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public class OpenCoverManagerFactory : IOpenCoverManagerFactory
    {

        private readonly IProcessFactory _processFactory;

        public OpenCoverManagerFactory(IProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public IOpenCoverRunnerManager CreateManager()
        {
            return new OpenCoverRunnerManager(_processFactory,new Timer());
        }
    }
}
