using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
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
        private readonly ICoverageStatisticsAggregator _statisticsObserver;
        private readonly IGenericCoverageWriterObserver _genericCoverageWriterObserver;
        private readonly IXmlAdapter _xmlAdapter;
        public CreateCoverageResultsCommandHandler() : this(new OpenCoverWrapperCommandLineParser(), 
            new CodeCoverageRepository(),
            new GenericCoverageWriterObserver(new GenericCoverageWriter()),
            new CoverageStatisticsAggregator(),
            new XmlAdapter()
            )
        {
            
        }
        public CreateCoverageResultsCommandHandler(IOpenCoverWrapperCommandLineParser openCoverWrapperCommandLineParser, 
            ICodeCoverageRepository codeCoverageRepository,
            IGenericCoverageWriterObserver genericCoverageWriterObserver,
            ICoverageStatisticsAggregator statisticsObserver,
            IXmlAdapter xmlAdapter)
        {
            _commandLineParser = openCoverWrapperCommandLineParser;
            _codeCoverageRepository = codeCoverageRepository;
            _statisticsObserver = statisticsObserver;
            _genericCoverageWriterObserver = genericCoverageWriterObserver;
            _xmlAdapter = xmlAdapter;
        }

        public void Execute(ICreateCoverageResultsCommand command)
        {

            _commandLineParser.Args = command.Args;
            _codeCoverageRepository.Workspace = command.Workspace;

            string outputPath = _commandLineParser.GetOutputPath();

            using (var writer = _xmlAdapter.CreateTextWriter(outputPath))
            {
                _genericCoverageWriterObserver.Writer = writer;
                _codeCoverageRepository.QueryAllModules()
                    .AddObserver(_genericCoverageWriterObserver)
                    .AddObserver(_statisticsObserver)
                    .Execute();
            }
            _log.Info($"Files         : {_statisticsObserver.Files}");
            _log.Info($"Lines         : {_statisticsObserver.Lines} ");
            _log.Info($"Covered Lines : {_statisticsObserver.CoveredLines}");
        }
    }
}
