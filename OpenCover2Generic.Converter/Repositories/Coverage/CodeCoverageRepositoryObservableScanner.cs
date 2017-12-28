using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CodeCoverageRepositoryObservableScanner : ICodeCoverageRepositoryObservableScanner
    {
        private readonly ICoverageStorageResolver _coverageStorageResolver;
        private readonly ICoverageParser _coverageParser;
        private  IntermediateModel _model;

        public CodeCoverageRepositoryObservableScanner(ICoverageStorageResolver coverageStorageResolver,ICoverageParser coverageParser)
        {
            this._coverageStorageResolver = coverageStorageResolver;
            _coverageParser = coverageParser;
        }

        public string RootDirectory { get; set; }
        public event EventHandler<EventArgs> OnBeginScan;
        public event EventHandler<EventArgs> OnEndScan;
        public event EventHandler<ModuleEventArgs> OnModule;
        public void Scan()
        {
            OnBeginScan?.Invoke(this,EventArgs.Empty);


            var moduleDirectories = _coverageStorageResolver.GetPathsOfAllModules(RootDirectory);
            foreach (string moduleDirectory in moduleDirectories)
            {
                _model = new IntermediateModel();
                foreach (string assemblyPath in _coverageStorageResolver.GetTestCoverageFilesOfModule(moduleDirectory))
                {
                    _coverageParser.ParseFile(_model,assemblyPath);
                }
                OnModule?.Invoke(this, new ModuleEventArgs(_model));
            }
            OnEndScan?.Invoke(this, EventArgs.Empty);
        }

        public void AddObserver(IScannerObserver observer)
        {
            OnBeginScan += observer.OnBeginScan;
            OnEndScan += observer.OnEndScan;
            OnModule += observer.OnModule;
        }
    }
}
