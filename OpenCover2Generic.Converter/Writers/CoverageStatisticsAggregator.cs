using System;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class CoverageStatisticsAggregator : ICoverageStatisticsAggregator, IQueryAllModulesResultObserver
    {
        public int Lines { get; private set; }
        public int CoveredLines { get; private set; }

        public int Files { get; private set; }
        public void OnBeginScan(object sender, EventArgs eventArgs)
        {
        }

        public void OnEndScan(object sender, EventArgs eventArgs)
        {
           
        }

        public void OnModule(object v, ModuleEventArgs moduleEventArgs)
        {
            var moduleModel = moduleEventArgs.Entity;
            foreach (var sourceFileCoverageModel in moduleModel.GetSourceFiles())
            {
                Lines += sourceFileCoverageModel.SequencePoints.Count;
                CoveredLines+=sourceFileCoverageModel.SequencePoints.Count(p => p.Covered);
                ++Files;
            }
        }

    }
}
