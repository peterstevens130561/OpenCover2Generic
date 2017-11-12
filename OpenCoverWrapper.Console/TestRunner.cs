using BHGE.SonarQube.OpenCover2Generic;
using BHGE.SonarQube.OpenCover2Generic.Consumer;
using BHGE.SonarQube.OpenCover2Generic.Factories;
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
using System.Xml;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    class TestRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestRunner));
        private readonly JobFileSystem _jobFileSystemInfo = new JobFileSystem(new FileSystemAdapter());
        private readonly MultiAssemblyConverter _converter;
        private readonly IProcessFactory _processFactory;

        public TestRunner(MultiAssemblyConverter converter,IProcessFactory processFactory)
        {
            _converter = converter;
            _processFactory = processFactory;

        }

        public void Initialize()
        {
            _jobFileSystemInfo.CreateRoot(DateTime.Now.ToString("yyMMdd_HHmmss"));
        }

        internal void RunTests(OpenCoverCommandLineBuilder openCoverCommandLineBuilder, string[] testAssemblies)
        {
            var jobs = new BlockingCollection<string>();
            log.Info($"Will run tests for {testAssemblies.Count()} assemblies");
            testAssemblies.ToList().ForEach(a => jobs.Add(a));
            jobs.CompleteAdding();

            var tasks = new List<Task>();
            for (int i = 1; i <= 5; i++)
            {
                Task task = Task.Run(() => ConsumeJobs(openCoverCommandLineBuilder, jobs));
                tasks.Add(task);
            }
            tasks.ForEach(t => t.Wait());
        }

        public void CreateCoverageFile(string outputPath)
        {
            log.Info("Assembling coverage file");
            using (XmlTextWriter xmlWriter = new XmlTextWriter(new StreamWriter(outputPath)))
            {
                var moduleDirectories = Directory.EnumerateDirectories(_jobFileSystemInfo.GetIntermediateCoverageDirectory(), "*", SearchOption.TopDirectoryOnly);
                _converter.BeginCoverageFile(xmlWriter);
                foreach (string moduleDirectory in moduleDirectories)
                {
                    _converter.BeginModule();
                    foreach (string assemblyFile in Directory.EnumerateFiles(moduleDirectory))
                    {
                        _converter.ReadIntermediateFile(assemblyFile);
                    }
                    _converter.AppendModuleToCoverageFile(xmlWriter);
                }
                _converter.EndCoverageFile(xmlWriter);
            }
        }

        public void CreateTestResults(string testResultsPath)
        {
            log.Info($"Creating test results file into {testResultsPath}");
            var files = Directory.EnumerateFiles(_jobFileSystemInfo.GetTestResultsDirectory());
            var testResultsConcatenator = new TestResultsConcatenator();

            using (var writer = new XmlTextWriter(new StreamWriter(testResultsPath)))
            {
                testResultsConcatenator.Writer = writer;
                testResultsConcatenator.Begin();
                foreach (var file in files)
                {
                    using (var reader = XmlReader.Create(file))
                    {
                        testResultsConcatenator.Concatenate(reader);
                    }

                }
                log.Info($"Found ${testResultsConcatenator.TestCases} test cases");
                testResultsConcatenator.End();
            }
        }


        private void ConsumeJobs(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, BlockingCollection<string> jobs)
        {
            var consumer = new JobConsumer(openCoverCommandLineBuilder,_jobFileSystemInfo, _processFactory);
            consumer.ConsumeJobs(jobs);
        }
  

    }
}
