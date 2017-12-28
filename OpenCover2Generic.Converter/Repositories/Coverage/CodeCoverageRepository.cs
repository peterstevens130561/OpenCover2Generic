using System;
using System.Text;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CodeCoverageRepository : ICodeCoverageRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CodeCoverageRepository));
        private IModuleCoverageModel _model;
        private ICoverageParser _parser;
        private ICoverageWriter _moduleWriter;
        private readonly Object _lock = new object();
        private readonly IOpenCoverOutput2RepositorySaver _converter;
        private readonly ICoverageStorageResolver _coverageStorageResolver;
        private readonly ICoverageParser _coverageParser;

        public CodeCoverageRepository(IOpenCoverOutput2RepositorySaver saver,
            ICoverageStorageResolver coverageStorageResolver, ICoverageParser coverageParser)
        {
            _converter = saver;
            _coverageStorageResolver = coverageStorageResolver;
            _coverageParser = coverageParser;
        }

        public string RootDirectory { get; set; }

        public void Add(string path, string key)
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

        private void WriteModule(string rootPath, string testAssemblyPath)
        {
            if (_model.GetSourceFiles().Count > 0)
            {
                string moduleFile = _coverageStorageResolver.GetPathForAssembly(rootPath, _parser.ModuleName,testAssemblyPath);
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

        public void CreateGenericCoverageFile(string outputPath)
        {
            using (var writer = new XmlTextWriter(outputPath,Encoding.UTF8))
            {
                CreateGenericCoverageFile(writer);
            }
        }

        public void CreateGenericCoverageFile(XmlTextWriter xmlWriter)
        {
            var moduleDirectories = _coverageStorageResolver.GetPathsOfAllModules(RootDirectory);
            _converter.BeginCoverageFile(xmlWriter);
            foreach (string moduleDirectory in moduleDirectories)
            {
                _converter.BeginModule();
                foreach (string assemblyFile in _coverageStorageResolver.GetTestCoverageFilesOfModule(moduleDirectory))
                {
                    _converter.ReadIntermediateFile(assemblyFile);
                }
                _converter.AppendModuleToCoverageFile(xmlWriter);
            }
            _converter.EndCoverageFile(xmlWriter);
        }

        public ICodeCoverageRepositoryObservableScanner Scanner()
        {
            var scanner = new CodeCoverageRepositoryObservableScanner(_coverageStorageResolver, _coverageParser);
            scanner.RootDirectory = RootDirectory;
            return scanner;
        }

    }
}
