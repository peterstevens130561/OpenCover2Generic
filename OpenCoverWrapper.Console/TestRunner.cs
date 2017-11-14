using BHGE.SonarQube.OpenCover2Generic;
using BHGE.SonarQube.OpenCover2Generic.Consumer;
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
using System.Xml;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    class TestRunner : ITestRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestRunner));
        private readonly IJobFileSystem _jobFileSystemInfo;
        private readonly MultiAssemblyConverter _converter;
        private readonly IJobConsumerFactory _jobConsumerFactory;
        private readonly List<Task> _tasks = new List<Task>();
        private readonly IJobs  _jobs = new Jobs();
        public TestRunner(IJobFileSystem jobFileSystemInfo,
            MultiAssemblyConverter converter, 
            IJobConsumerFactory jobConsumerFactory)
        {
            _jobFileSystemInfo = jobFileSystemInfo;
            _converter = converter;
            _jobConsumerFactory = jobConsumerFactory;

        }

        public void Initialize()
        {
            _jobFileSystemInfo.CreateRoot(DateTime.Now.ToString("yyMMdd_HHmmss"));
        }

        /// <summary>
        /// Runs the tests parallel
        /// </summary>
        /// <param name="testAssemblies"></param>
        /// <param name="parallelJobs">number of consumers, which will run in parallel</param>
        internal void RunTests( string[] testAssemblies,int parallelJobs)
        {

            CreateJobs(testAssemblies, 1);
            CreateJobConsumers(parallelJobs);
            _tasks.ForEach(t => t.Wait());
        }

        public void CreateJobs(string[] testAssemblies, int chunkSize)
        {
            log.Info($"Will run tests for {testAssemblies.Count()} assemblies");
            var list = testAssemblies.ToList();
            int count = testAssemblies.Count();
            for (int index=0;index<count;index+=chunkSize)
            {
                chunkSize = Math.Min(chunkSize, count - index);
                var chunk = list.GetRange(index, chunkSize);
                _jobs.Add(new Job(chunk));
            }
            _jobs.CompleteAdding();
        }

        public IJobs Jobs { get { return _jobs; } }
         public void CreateJobConsumers(int consumers)
        {

            for (int i = 1; i <= consumers; i++)
            {
                Task task = Task.Run(() => _jobConsumerFactory.Create().ConsumeJobs(_jobs));
                _tasks.Add(task);
            }
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
                log.Info($"Found ${testResultsConcatenator.ExecutedTestCases} test cases");
                testResultsConcatenator.End();
            }
        }
  

    }
}
