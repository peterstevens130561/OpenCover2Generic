using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICodeCoverageRepository
    {
        string Directory { get; set; }

        void Save(ICoverageAggregate coverageAggregate);

        IQueryAllModulesObservable QueryAllModules();
    }
}