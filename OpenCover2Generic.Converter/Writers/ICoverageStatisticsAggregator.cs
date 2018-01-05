namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface ICoverageStatisticsAggregator
    {
        int CoveredLines { get; }
        int Files { get; }
        int Lines { get; }
    }
}
