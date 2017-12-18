using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Model;
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
        public JobConsumer(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder,
            IJobFileSystem jobFileSystem,
            IOpenCoverManagerFactory openCoverManagerFactory,
            ITestResultsRepository testResultsRepository,
            ICodeCoverageRepository codeCoverageRepository)
        {
            _openCoverCommandLineBuilder = openCoverCommandLineBuilder;
            _jobFileSystemInfo = jobFileSystem;
            _openCoverManagerFactory = openCoverManagerFactory;
            _testResultsRepository = testResultsRepository;
            _codeCoverageRepository = codeCoverageRepository;
        }

        public void ConsumeTestJobs(IJobs jobs,TimeSpan jobTimeOut)
        {
            while (!jobs.IsCompleted())
            {
                IJob job = GetAssembly(jobs);
                if (job == null)
                {
                    continue;
                }
                Consume(job,jobTimeOut);

            }
        }

        private void Consume(IJob job,TimeSpan jobTimeOut)
        {

            var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(job.FirstAssembly);
            string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(job.FirstAssembly);
            var openCoverManager = _openCoverManagerFactory.CreateManager();
            openCoverManager.SetTimeOut(jobTimeOut);
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {

                var processStartInfo = _openCoverCommandLineBuilder.Build(job.Assemblies, openCoverOutputPath);
                Task task = Task.Run(() => openCoverManager.Run(processStartInfo, writer,job.Assemblies));
                task.Wait();
            }
            if (openCoverManager.HasTests)
            {
                _testResultsRepository.Add(openCoverManager.TestResultsPath);
            }
            _codeCoverageRepository.Add(openCoverOutputPath, job.FirstAssembly);


        }

        private IJob GetAssembly(IJobs jobs)
        {
            IJob job = null;
            try
            {
                job = jobs.Take();
            }
            catch (InvalidOperationException)
            {
                // an exception at this place may happen, as one job might be just ahead. See doc.
                _log.Debug("Exception on take (ignored, may happen)");
            }

            return job;
        }
    }
}
