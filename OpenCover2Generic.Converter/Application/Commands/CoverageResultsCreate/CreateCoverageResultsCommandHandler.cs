using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCoverWrapper;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate
{
    class CreateCoverageResultsCommandHandler : ICommandHandler<ICreateCoverageResultsCommand>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CreateCoverageResultsCommandHandler));
        private readonly ICodeCoverageRepository _codeCoverageRepository;
        private readonly IOpenCoverWrapperCommandLineParser _commandLineParser;
        private readonly IQueryAllModulesResultObserver _statisticsObserver;
        private readonly IQueryAllModulesResultObserver _genericCoverageWriterObserver;

        public CreateCoverageResultsCommandHandler() : this(new OpenCoverWrapperCommandLineParser(), 
            new CodeCoverageRepository(),
            new GenericCoverageWriterObserver(new GenericCoverageWriter()),
            new CoverageStatisticsAggregator()
            )
        {
            
        }
        public CreateCoverageResultsCommandHandler(IOpenCoverWrapperCommandLineParser openCoverWrapperCommandLineParser, 
            ICodeCoverageRepository codeCoverageRepository,
            IQueryAllModulesResultObserver genericCoverageWriterObserver,
            IQueryAllModulesResultObserver statisticsObserver)
        {
            _commandLineParser = openCoverWrapperCommandLineParser;
            _codeCoverageRepository = codeCoverageRepository;
            _statisticsObserver = statisticsObserver;
            _genericCoverageWriterObserver = genericCoverageWriterObserver;
        }

        public void Execute(ICreateCoverageResultsCommand command)
        {

            _commandLineParser.Args = command.Args;
            _codeCoverageRepository.Workspace = command.Workspace;

            string outputPath = _commandLineParser.GetOutputPath();

            using (var writer = new XmlTextWriter(outputPath, Encoding.UTF8))
            {
               ((IGenericCoverageWriterObserver)_genericCoverageWriterObserver).Writer = writer;
                _codeCoverageRepository.QueryAllModules()
                    .AddObserver(_genericCoverageWriterObserver)
                    .AddObserver(_statisticsObserver)
                    .Execute();
            }
            _log.Info($"Files         : {statisticsObserver.Files}");
            _log.Info($"Lines         : {statisticsObserver.Lines} ");
            _log.Info($"Covered Lines : {statisticsObserver.CoveredLines}");
        }
    }
}
