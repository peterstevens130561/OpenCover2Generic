using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICodeCoverageRepository 
    {
        IWorkspace Workspace { get; set; }

        void Save(ICoverageAggregate coverageAggregate);

        IQueryAllModulesObservable QueryAllModules();
    }
}