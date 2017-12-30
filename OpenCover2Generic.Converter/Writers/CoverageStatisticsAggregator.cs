using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    class CoverageStatisticsAggregator : ICoverageStatisticsAggregator, IScannerObserver
    {
        public int Lines { get; private set; }


        public void OnBeginScan(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void OnEndScan(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void OnModule(object v, ModuleEventArgs moduleEventArgs)
        {
            throw new NotImplementedException();
        }

    }
}
