using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class QueryAllModulesObservable : IQueryAllModulesObservable
    {
        private readonly ICoverageRepositoryPathResolver _coverageRepositoryPathResolver;
        private readonly ICoverageParser _coverageParser;
        

        public QueryAllModulesObservable(ICoverageRepositoryPathResolver coverageRepositoryPathResolver,ICoverageParser coverageParser)
        {
            _coverageRepositoryPathResolver = coverageRepositoryPathResolver;
            _coverageParser = coverageParser;
        }

        public string RootDirectory { get; set; }
        public event EventHandler<EventArgs> OnBeginScan;
        public event EventHandler<EventArgs> OnEndScan;
        public event EventHandler<ModuleEventArgs> OnModule;
        public void Execute()
        {
            OnBeginScan?.Invoke(this,EventArgs.Empty);


            var moduleDirectories = _coverageRepositoryPathResolver.GetPathsOfAllModules();
            foreach (string moduleDirectory in moduleDirectories)
            {
                var model = new AggregatedModule();
                foreach (string assemblyPath in _coverageRepositoryPathResolver.GetTestCoverageFilesOfModule(moduleDirectory))
                {
                    _coverageParser.ParseFile(model,assemblyPath);
                }
                OnModule?.Invoke(this, new ModuleEventArgs(model));
            }
            OnEndScan?.Invoke(this, EventArgs.Empty);
            
        }

        public IQueryAllModulesObservable AddObserver(IQueryAllModulesResultObserver observer)
        {
            OnBeginScan += observer.OnBeginScan;
            OnEndScan += observer.OnEndScan;
            OnModule += observer.OnModule;
            return this;
        }
    }
}
