﻿using BHGE.SonarQube.OpenCover2Generic.Factories;
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
        private readonly JobFileSystem _jobFileSystemInfo;
        private readonly IProcessFactory _processFactory;

        public JobConsumer(JobFileSystem jobFileSystem,IProcessFactory processFactory)
        {
            _jobFileSystemInfo = jobFileSystem;
            _processFactory = processFactory;
        }
        public void ConsumeJobs(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, BlockingCollection<string> jobs)
        {
            while (!jobs.IsCompleted)
            {
                string assembly = GetAssembly(jobs);
                if (assembly == null)
                {
                    continue;
                }
                Consume(openCoverCommandLineBuilder, assembly);

            }
        }

        public void Consume(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, string assembly)
        {
            log.Info($"Running unit test on {Path.GetFileName(assembly)}");

            var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(assembly);
            string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(assembly);
            var runner = new OpenCoverRunnerManager(_processFactory);
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {
                var processStartInfo = openCoverCommandLineBuilder.Build(assembly, openCoverOutputPath);
                Task task = Task.Run(() => runner.Run(processStartInfo, writer));
                task.Wait();
            }
            if (runner.TestResultsPath != null)
            {
                string testResultsPath = _jobFileSystemInfo.GetTestResultsPath(assembly);
                File.Move(runner.TestResultsPath, testResultsPath);
                try
                {
                    using (var reader = new StreamReader(openCoverOutputPath))
                    {
                        new MultiAssemblyConverter().ConvertCoverageFileIntoIntermediate(_jobFileSystemInfo.GetIntermediateCoverageDirectory(), assembly, reader);
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Exception thrown during reading {openCoverOutputPath}");
                    throw;
                }
            }
        }

        private static string GetAssembly(BlockingCollection<string> jobs)
        {
            string assembly = null;
            try
            {
                assembly = jobs.Take();
            }
            catch (InvalidOperationException e)
            {
                log.Debug("Exception on take (ignored");
            }

            return assembly;
        }
    }
}
