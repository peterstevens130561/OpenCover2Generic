﻿using System;
using System.Linq;
using System.Text;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
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
        private readonly IXmlAdapter _xmlAdapter;
        private readonly ICoverageWriterFactory _coverageWriterFactory;

        public CodeCoverageRepository(
            ICoverageStorageResolver coverageStorageResolver, 
            ICoverageParser coverageParser,
            IXmlAdapter xmlAdapter,
            ICoverageWriterFactory coverageWriterFactory)
        {
            _coverageStorageResolver = coverageStorageResolver;
            _coverageParser = coverageParser;
            _xmlAdapter = xmlAdapter;
            _coverageWriterFactory = coverageWriterFactory;
        }

        public string RootDirectory { get; set; }

        public void Add(ICoverageAggregate coverageAggregate)
        {
            coverageAggregate.Modules(WriteModule);
        }

        private void WriteModule(IModuleCoverageModel model)
        {
            if (model.GetSourceFiles().Any())
            {
                string moduleFile = _coverageStorageResolver.GetPathForAssembly(RootDirectory, model.Name, Guid.NewGuid().ToString());
                using (XmlTextWriter tempFileWriter = _xmlAdapter.CreateTextWriter(moduleFile))
                {
                    _moduleWriter = _coverageWriterFactory.CreateOpenCoverCoverageWriter();
                    _moduleWriter.WriteBegin(tempFileWriter);
                    _moduleWriter.GenerateCoverage(model, tempFileWriter);
                    _moduleWriter.WriteEnd(tempFileWriter);
                }
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
