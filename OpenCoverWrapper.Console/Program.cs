using BHGE.SonarQube.OpenCover2Generic;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using OpenCover2Generic.Converter;
using System.Xml;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BHGE.SonarQube.OpenCoverWrapper
{

    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            commandLineParser.Args = args;
            string openCoverExePath = commandLineParser.GetOpenCoverPath();
            string outputPath = commandLineParser.GetOutputPath();
            string targetPath = commandLineParser.GetTargetPath();
            string targetArgs = commandLineParser.GetTargetArgs();
            string testResultsPath = commandLineParser.GetTestResultsPath();
            string openCoverOutputPath = Path.GetTempFileName();
            var converter = new MultiAssemblyConverter(new Model(),
    new OpenCoverCoverageParser(),
    new GenericCoverageWriter(),
    new OpenCoverCoverageParser(),
    new OpenCoverCoverageWriter());

            var testRunner = new TestRunner(converter);
            string[] testAssemblies = commandLineParser.GetTestAssemblies();

            testRunner.Initialize();
            testRunner.RunTests(openCoverExePath, targetPath, targetArgs,  testAssemblies);
            testRunner.CreateTestResults(testResultsPath);
            testRunner.CreateCoverageFile(outputPath);
        }

        private class TestRunner {

            private JobInfo _jobInfo = new JobInfo();
            private readonly MultiAssemblyConverter _converter;

            public TestRunner(MultiAssemblyConverter converter)
            {
                _converter= converter;

            }

            public void Initialize()
            {
                _jobInfo.CreateRoot();
            }
            public void RunTests(string openCoverExePath, string targetPath, string targetArgs,string[] testAssemblies)
        {
            var jobs = new BlockingCollection<string>();
            testAssemblies.ToList().ForEach(a => jobs.Add(a));
            jobs.CompleteAdding();

            var tasks = new List<Task>();
            for (int i = 1; i <= 20; i++)
            {
                log.Info($"starting task {i}");
                Task task = Task.Run(() => ConsumeJobs(openCoverExePath, targetPath, targetArgs, jobs));
                tasks.Add(task);
            }
            tasks.ForEach(t => t.Wait());
        }

        public void CreateCoverageFile(string outputPath)
        {
            log.Info("Assembling coverage file");
                using (XmlTextWriter xmlWriter = new XmlTextWriter(new StreamWriter(outputPath)))
                {


                    var moduleDirectories = Directory.EnumerateDirectories(_jobInfo.GetIntermediateCoverageDirectory(), "*", SearchOption.TopDirectoryOnly);
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
            var files = Directory.EnumerateFiles(_jobInfo.GetTestResultsDirectory());
            var testResultsConcatenator = new TestResultsConcatenator();

            using (var writer = new XmlTextWriter(new StreamWriter(testResultsPath)))
            {
                testResultsConcatenator.Writer = writer;
                testResultsConcatenator.Begin();
                foreach (var file in files)
                {
                    using (var reader = XmlReader.Create(file))
                    {
                        log.Info($"concatenating {file}");
                        testResultsConcatenator.Concatenate(reader);
                    }

                }
                testResultsConcatenator.End();
            }
        }

            /// <summary>
            /// Looks continuously in jobs until no more jobs found. Runs opencover to provide test results + coverage data
            /// </summary>
            /// <param name="openCoverExePath"></param>
            /// <param name="targetPath"></param>
            /// <param name="targetArgs"></param>
            /// <param name="jobs"></param>
        private void ConsumeJobs(string openCoverExePath, string targetPath, string targetArgs, BlockingCollection<string> jobs)
        {
            while (!jobs.IsCompleted)
            {
                string assembly = GetAssembly(jobs);
                if (assembly == null)
                {
                    continue;
                }
                log.Info($"Running unit test on {assembly}");
                string openCoverOutputPath = _jobInfo.GetOpenCoverOutputPath(assembly);
                var openCoverLogPath = _jobInfo.GetOpenCoverLogPath(assembly);
                log.Debug($"Log of opencover will be stored in {openCoverLogPath}");
                string arguments = $"-register:user -\"output:{openCoverOutputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs} {assembly}\"";
                string resultsPath= _jobInfo.GetTestResultsPath(assembly);
                RunOpenCover(resultsPath, openCoverExePath, arguments, openCoverLogPath);
                    try
                    {
                        using (var reader = new StreamReader(openCoverOutputPath))
                        {
                            new MultiAssemblyConverter().ConvertCoverageFileIntoIntermediate(_jobInfo.GetIntermediateCoverageDirectory(), assembly, reader);
                        }
                    } catch ( Exception e) {
                        log.Error($"Exception thrown during reading {openCoverOutputPath}");
                        throw;
                    }
             
            }
        }

        private void RunOpenCover(string testResultsPath, string openCoverExePath, string arguments,string openCoverLogPath)
        {
            var runner = new OpenCoverRunner();
            runner.AddArgument(arguments);
            runner.SetPath(openCoverExePath);
            using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
            {
                Task task = Task.Run(() => runner.Run(writer));
                task.Wait();
            }
            if (runner.TestResultsPath != null)
            {
                log.Info($"move from {runner.TestResultsPath} to {testResultsPath}");
                File.Move(runner.TestResultsPath, testResultsPath);
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
}
