using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICodeCoverageRepository
    {
        string RootDirectory { get; set; }

        void Save(ICoverageAggregate coverageAggregate);

        IQueryAllModulesObservable QueryAllModules();
    }
}