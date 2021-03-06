﻿using System;
using System.Linq;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
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
        private readonly ICoverageRepositoryPathResolver _coverageRepositoryPathResolver;
        private readonly ICoverageParser _coverageParser;
        private readonly IXmlAdapter _xmlAdapter;
        private readonly ICoverageWriterFactory _coverageWriterFactory;
        private IWorkspace _workspace;

        public CodeCoverageRepository() : this(new CoverageRepositoryPathResolver(),
            new OpenCoverCoverageParser(),
            new XmlAdapter(),
            new CoverageWriterFactory())
        {
            
        }

        public CodeCoverageRepository(
            ICoverageRepositoryPathResolver coverageRepositoryPathResolver, 
            ICoverageParser coverageParser,
            IXmlAdapter xmlAdapter,
            ICoverageWriterFactory coverageWriterFactory)
        {
            _coverageRepositoryPathResolver = coverageRepositoryPathResolver;
            _coverageParser = coverageParser;
            _xmlAdapter = xmlAdapter;
            _coverageWriterFactory = coverageWriterFactory;
        }


        public IWorkspace  Workspace
        {
            get { return _workspace; }

            set
            {
                _workspace = value;
                _coverageRepositoryPathResolver.Root = value.Path;
            }
        }

        public void Save(ICoverageAggregate coverageAggregate)
        {
            coverageAggregate.Modules(WriteModule);
        }

        private void WriteModule(IModule entity)
        {
            if (entity.GetSourceFiles().Any())
            {
                string moduleFile = _coverageRepositoryPathResolver.GetPathForAssembly(entity.NameId, Guid.NewGuid().ToString());
                using (XmlTextWriter tempFileWriter = _xmlAdapter.CreateTextWriter(moduleFile))
                {
                    var moduleWriter = _coverageWriterFactory.CreateOpenCoverCoverageWriter();
                    moduleWriter.WriteBegin(tempFileWriter);
                    moduleWriter.GenerateCoverage(entity, tempFileWriter);
                    moduleWriter.WriteEnd(tempFileWriter);
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
            var queryAllModules = new QueryAllModulesObservable(_coverageRepositoryPathResolver, _coverageParser);
            return queryAllModules;
        }

    }
}
