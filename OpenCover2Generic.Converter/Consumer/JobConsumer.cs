using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(JobConsumer));
        private readonly IJobFileSystem _jobFileSystemInfo;
        private readonly IOpenCoverCommandLineBuilder _openCoverCommandLineBuilder;
        private readonly IOpenCoverManagerFactory _openCoverManagerFactory;

        public JobConsumer(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder,IJobFileSystem jobFileSystem,IOpenCoverManagerFactory openCoverManagerFactory)
        {
            _openCoverCommandLineBuilder = openCoverCommandLineBuilder;
            _jobFileSystemInfo = jobFileSystem;
            _openCoverManagerFactory = openCoverManagerFactory;
        }

        public void ConsumeJobs(IJobs jobs)
        {
            while (!jobs.IsCompleted())
            {
                IJob job = GetAssembly(jobs);
                if (job == null)
                {
                    continue;
                }
                Consume(job);

            }
        }

        private void Consume(IJob job)
        {
            log.Info($"Running unit test on {Path.GetFileName(job.Assemblies)}");

            var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(job.FirstAssembly);
            string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(job.FirstAssembly);
            var runner = _openCoverManagerFactory.CreateManager();
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {
                var processStartInfo = _openCoverCommandLineBuilder.Build(job.Assemblies, openCoverOutputPath);
                Task task = Task.Run(() => runner.Run(processStartInfo, writer));
                task.Wait();
            }
            if (runner.TestResultsPath != null)
            {
                string testResultsPath = _jobFileSystemInfo.GetTestResultsPath(job.FirstAssembly);
                File.Move(runner.TestResultsPath, testResultsPath);
                try
                {
                    using (var reader = new StreamReader(openCoverOutputPath))
                    {
                        new MultiAssemblyConverter().ConvertCoverageFileIntoIntermediate(_jobFileSystemInfo.GetIntermediateCoverageDirectory(), job.FirstAssembly, reader);
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Exception thrown during reading {openCoverOutputPath}");
                    throw;
                }
            }
        }

        private IJob GetAssembly(IJobs jobs)
        {
            IJob job = null;
            try
            {
                job = jobs.Take();
            }
            catch (InvalidOperationException e)
            {
                log.Debug("Exception on take (ignored");
            }

            return job;
        }
    }
}
