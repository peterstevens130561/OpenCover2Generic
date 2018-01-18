using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface ICoverageStatisticsAggregator : IQueryAllModulesResultObserver
    {
        int CoveredLines { get; }
        int Files { get; }
        int Lines { get; }
    }
}
