using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.TestJobConsumer
{
    public class JobConsumer : IJobConsumer
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(JobConsumer));
        private readonly IJobFileSystem _jobFileSystemInfo;
        private readonly IOpenCoverCommandLineBuilder _openCoverCommandLineBuilder;
        private readonly IOpenCoverManagerFactory _openCoverManagerFactory;
        private readonly ITestResultsRepository _testResultsRepository;
        private readonly ICodeCoverageRepository _codeCoverageRepository;
        private readonly ICoverageAggregateFactory _coverageAggregateFactory;

        public JobConsumer(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder,
            IJobFileSystem jobFileSystem,
            IOpenCoverManagerFactory openCoverManagerFactory,
            ITestResultsRepository testResultsRepository,
            ICodeCoverageRepository codeCoverageRepository,
            ICoverageAggregateFactory coverageAggregateFactory)
        {
            _openCoverCommandLineBuilder = openCoverCommandLineBuilder;
            _jobFileSystemInfo = jobFileSystem;
            _openCoverManagerFactory = openCoverManagerFactory;
            _testResultsRepository = testResultsRepository;
            _codeCoverageRepository = codeCoverageRepository;
            _coverageAggregateFactory = coverageAggregateFactory;
        }

        public void ConsumeTestJobs(IJobs jobs,TimeSpan jobTimeOut)
        {
            try
            {
                while (!jobs.IsCompleted())
                {
                    ITestJob testJob = GetAssembly(jobs);
                    if (testJob == null)
                    {
                        continue;
                    }
                    Consume(testJob, jobTimeOut);

                }
            }
            catch (Exception e)
            {
                _log.Error($"{e}\n{e.StackTrace}");
                throw;
            }
        }

        private void Consume(ITestJob testJob,TimeSpan jobTimeOut)
        {
            _testResultsRepository.SetWorkspace(testJob.Workspace);
            _jobFileSystemInfo.CreateRoot(testJob.Workspace);
            var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(testJob.FirstAssembly);
            string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(testJob.FirstAssembly);
            var openCoverManager = _openCoverManagerFactory.CreateManager();
            openCoverManager.SetTimeOut(jobTimeOut);
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {
                _openCoverCommandLineBuilder.Args = testJob.Args;
                var processStartInfo = _openCoverCommandLineBuilder.Build(testJob.Assemblies, openCoverOutputPath);
                Task task = Task.Run(() => openCoverManager.Run(processStartInfo, writer,testJob.Assemblies));
                task.Wait();
            }
            if (openCoverManager.HasTests)
            {
                _testResultsRepository.Add(openCoverManager.TestResultsPath);
            }
            var coverageAggregate = _coverageAggregateFactory.Create(openCoverOutputPath);
            _codeCoverageRepository.Directory = testJob.RepositoryRootDirectory;
            _codeCoverageRepository.Save(coverageAggregate);
        }

        private ITestJob GetAssembly(IJobs jobs)
        {
            ITestJob testJob = null;
            try
            {
                testJob = jobs.Take();
            }
            catch (InvalidOperationException)
            {
                // an exception at this place may happen, as one job might be just ahead. See doc.
                _log.Debug("Exception on take (ignored, may happen)");
            }

            return testJob;
        }
    }
}
