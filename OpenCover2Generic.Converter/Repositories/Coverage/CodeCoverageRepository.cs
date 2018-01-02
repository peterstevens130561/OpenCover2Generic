using System;
using System.Text;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    /// <summary>
    /// provides abstraction of storage/retrieval of the code coverage information
    /// </summary>
    public class CodeCoverageRepository : ICodeCoverageRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CodeCoverageRepository));
        private IModuleCoverageModel _model;
        private ICoverageParser _parser;
        private ICoverageWriter _moduleWriter;
        private readonly Object _lock = new object();
        private readonly ICoverageStorageResolver _coverageStorageResolver;
        private readonly ICoverageParser _coverageParser;

        public CodeCoverageRepository(
            ICoverageStorageResolver coverageStorageResolver, ICoverageParser coverageParser)
        {
            _coverageStorageResolver = coverageStorageResolver;
            _coverageParser = coverageParser;
        }

        public string RootDirectory { get; set; }

        public void Add(ICoverageAggregate coverageAggregate)
        {
            Add(coverageAggregate.Path,coverageAggregate.Key);
        }
        /// <summary>
        /// Add a coverage output file of OpenCover to the repository
        /// </summary>
        /// <param name="path">to the openCover file</param>
        /// <param name="key">some unique key</param>
        private void Add(string path, string key)
        {
            lock (_lock)
            {
                _parser = new OpenCoverCoverageParser();
                _moduleWriter = new OpenCoverCoverageWriter();
                try
                {
                    using (var xmlReader = XmlReader.Create(path))
                    {
                        xmlReader.MoveToContent();
                        _model = new IntermediateModel();
                        while (_parser.ParseModule(_model, xmlReader))
                        {
                            WriteModule(RootDirectory, key);
                        }
                        WriteModule(RootDirectory, key);
                    }

                }
                catch (Exception e)
                {
                    _log.Error($"Exception thrown during reading {path}\n{e.Message}\n{e.StackTrace}");
                    throw;
                }
            }
        }

        private void WriteModule(string rootPath, string key)
        {
            if (_model.GetSourceFiles().Count > 0)
            {
                string moduleFile = _coverageStorageResolver.GetPathForAssembly(rootPath, _parser.ModuleName,key);
                WriteModuleToFile(moduleFile);
                _model.Clear();
            }
        }

        private void WriteModuleToFile(string moduleFile)
        {
            using (XmlTextWriter tempFileWriter = new XmlTextWriter(moduleFile, Encoding.UTF8))
            {
                //write it to the temp file
                _moduleWriter.WriteBegin(tempFileWriter);
                _moduleWriter.GenerateCoverage(_model, tempFileWriter);
                _moduleWriter.WriteEnd(tempFileWriter);
            }
        }

        /// <summary>
        /// provides an observable fluent query of the coverage information by module.
        /// As the repository can be many GB, this query should be executed once, first registering all
        /// observers, then executing
        /// </summary>
        /// <returns></returns>
        public IQueryAllModulesObservable QueryAllModules()
        {
            var scanner = new QueryAllModulesObservable(_coverageStorageResolver, _coverageParser);
            scanner.RootDirectory = RootDirectory;
            return scanner;
        }

    }
}
