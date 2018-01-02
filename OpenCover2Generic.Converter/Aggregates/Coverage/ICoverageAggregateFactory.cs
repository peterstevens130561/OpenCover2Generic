namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public interface ICoverageAggregateFactory
    {
        ICoverageAggregate Create(string path);
    }
}