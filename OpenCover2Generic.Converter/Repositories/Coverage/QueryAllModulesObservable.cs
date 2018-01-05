using System;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class QueryAllModulesObservable : IQueryAllModulesObservable
    {
        private readonly ICoverageStorageResolver _coverageStorageResolver;
        private readonly ICoverageParser _coverageParser;
        

        public QueryAllModulesObservable(ICoverageStorageResolver coverageStorageResolver,ICoverageParser coverageParser)
        {
            _coverageStorageResolver = coverageStorageResolver;
            _coverageParser = coverageParser;
        }

        public string RootDirectory { get; set; }
        public event EventHandler<EventArgs> OnBeginScan;
        public event EventHandler<EventArgs> OnEndScan;
        public event EventHandler<ModuleEventArgs> OnModule;
        public void Execute()
        {
            OnBeginScan?.Invoke(this,EventArgs.Empty);


            var moduleDirectories = _coverageStorageResolver.GetPathsOfAllModules(RootDirectory);
            foreach (string moduleDirectory in moduleDirectories)
            {
                var model = new IntermediateEntity();
                foreach (string assemblyPath in _coverageStorageResolver.GetTestCoverageFilesOfModule(moduleDirectory))
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
