using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Consumer
{
    /// <summary>
    /// Creates JobConsumers
    /// </summary>
    public class JobConsumerFactory : IJobConsumerFactory
    {
        private readonly IJobFileSystem _jobFileSystem;
        private readonly IOpenCoverCommandLineBuilder _openCoverCommandLineBuilder;
        private readonly IOpenCoverManagerFactory _openCoverManagerFactory;
        private readonly ITestResultsRepository _testResultsRepository;
        private readonly ICodeCoverageRepository _codeCoverageRepository;

        public JobConsumerFactory(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, 
            IJobFileSystem jobFileSystem, 
            IOpenCoverManagerFactory openCoverManagerFactory,
            ITestResultsRepository testResultsRepository,
            ICodeCoverageRepository codeCoverageRepository)
        {
            _openCoverCommandLineBuilder = openCoverCommandLineBuilder;
            _jobFileSystem = jobFileSystem;
            _openCoverManagerFactory = openCoverManagerFactory;
            _testResultsRepository = testResultsRepository;
            _codeCoverageRepository = codeCoverageRepository;
        } 

        public IJobConsumer Create()
        {
            return new JobConsumer(_openCoverCommandLineBuilder, _jobFileSystem, _openCoverManagerFactory,_testResultsRepository,_codeCoverageRepository);
        }
    }
}
