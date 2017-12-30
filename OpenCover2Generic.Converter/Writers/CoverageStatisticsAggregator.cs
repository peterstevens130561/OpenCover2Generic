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
        public int CoveredLines { get; private set; }

        public void OnBeginScan(object sender, EventArgs eventArgs)
        {
        }

        public void OnEndScan(object sender, EventArgs eventArgs)
        {
        }

        public void OnModule(object v, ModuleEventArgs moduleEventArgs)
        {
            var moduleModel = moduleEventArgs.Model;
            foreach (var sourceFileCoverageModel in moduleModel.GetSourceFiles())
            {
                Lines += sourceFileCoverageModel.SequencePoints.Count;
                CoveredLines+=sourceFileCoverageModel.SequencePoints.Count(p => p.Covered);
            }
        }

    }
}
