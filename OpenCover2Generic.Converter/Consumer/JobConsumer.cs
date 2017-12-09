using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using log4net;
using OpenCover2Generic.Converter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Consumer
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

        public void ConsumeJobs(IJobs jobs,TimeSpan jobTimeOut)
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
            _log.Info($"{Path.GetFileName(job.Assemblies)}");

            var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(job.FirstAssembly);
            string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(job.FirstAssembly);
            var runner = _openCoverManagerFactory.CreateManager();
            runner.SetTimeOut(jobTimeOut);
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {

                var processStartInfo = _openCoverCommandLineBuilder.Build(job.Assemblies, openCoverOutputPath);
                Task task = Task.Run(() => runner.Run(processStartInfo, writer,job.Assemblies));
                task.Wait();
            }
            _testResultsRepository.Add(runner.TestResultsPath);
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
                _log.Debug("Exception on take (ignored, may happen)");
            }

            return job;
        }
    }
}
