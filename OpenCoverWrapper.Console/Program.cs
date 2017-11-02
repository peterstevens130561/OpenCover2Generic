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

            var openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            openCoverCommandLineBuilder.Args = args;
            var testRunner = new TestRunner(converter);
            string[] testAssemblies = commandLineParser.GetTestAssemblies();

            testRunner.Initialize();
            testRunner.RunTests(openCoverCommandLineBuilder, testAssemblies);
            testRunner.CreateTestResults(testResultsPath);
            testRunner.CreateCoverageFile(outputPath);
        }

        private class TestRunner {

            private readonly JobInfo _jobFileSystemInfo = new JobInfo();
            private readonly MultiAssemblyConverter _converter;

            public TestRunner(MultiAssemblyConverter converter)
            {
                _converter= converter;

            }

            public void Initialize()
            {
                _jobFileSystemInfo.CreateRoot();
            }

            internal void RunTests(OpenCoverCommandLineBuilder openCoverCommandLineBuilder, string[] testAssemblies)
            {
                var jobs = new BlockingCollection<string>();
                testAssemblies.ToList().ForEach(a => jobs.Add(a));
                jobs.CompleteAdding();

                var tasks = new List<Task>();
                for (int i = 1; i <= 1; i++)
                {
                    log.Info($"starting task {i}");
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
                        log.Info($"concatenating {file}");
                        testResultsConcatenator.Concatenate(reader);
                    }

                }
                testResultsConcatenator.End();
            }
        }


            private void ConsumeJobs(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, BlockingCollection<string> jobs)
            {
                while (!jobs.IsCompleted)
                {
                    string assembly = GetAssembly(jobs);
                    if (assembly == null)
                    {
                        continue;
                    }
                    log.Info($"Running unit test on {assembly}");
                   
                    var openCoverLogPath = _jobFileSystemInfo.GetOpenCoverLogPath(assembly);
                    string openCoverOutputPath = _jobFileSystemInfo.GetOpenCoverOutputPath(assembly);
                    var runner = new OpenCoverRunner();
                    using (var writer = new StreamWriter(openCoverLogPath, false, Encoding.UTF8))
                    {
                        var processStartInfo = openCoverCommandLineBuilder.Build(assembly, openCoverOutputPath);
                        log.Debug($"OpenCover commandline {processStartInfo.Arguments}");
                        log.Debug($"Log of opencover will be stored in {openCoverLogPath}");
                        Task task = Task.Run(() => runner.Run(processStartInfo,writer));
                        task.Wait();
                    }
                    if (runner.TestResultsPath != null)
                    {
                        string testResultsPath = _jobFileSystemInfo.GetTestResultsPath(assembly);
                        log.Debug($"move from {runner.TestResultsPath} to {testResultsPath}");
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
