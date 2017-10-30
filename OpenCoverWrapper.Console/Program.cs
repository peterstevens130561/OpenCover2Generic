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
        static void Main(string[] args)
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
            string[] testAssemblies = commandLineParser.GetTestAssemblies();
            string resultsDirectory = CreateResultsDirectory();
            string coverageDirectory = CreateCoverageDirectory();
            RunTests(openCoverExePath, targetPath, targetArgs, openCoverOutputPath, testAssemblies, resultsDirectory,coverageDirectory);
            CreateTestResults(testResultsPath, resultsDirectory);
            CreateCoverageFile(outputPath, openCoverOutputPath);
        }

        private static void RunTests(string openCoverExePath, string targetPath, string targetArgs, string openCoverOutputPath, string[] testAssemblies, string resultsDirectory,string coverageDirectory)
        {
            var jobs = new BlockingCollection<string>();
            testAssemblies.ToList().ForEach(a => jobs.Add(a));
            jobs.CompleteAdding();

            var tasks = new List<Task>();
            for (int i = 1; i <= 20; i++)
            {
                log.Info($"starting task {i}");
                Task task = Task.Run(() => ConsumeJobs(resultsDirectory, openCoverExePath, targetPath, targetArgs, openCoverOutputPath, jobs,coverageDirectory));
                tasks.Add(task);
            }
            tasks.ForEach(t => t.Wait());
        }

        private static void CreateCoverageFile(string outputPath, string openCoverOutputPath)
        {
            log.Info("Assembling coverage file");
            var converter = new MultiAssemblyConverter(new Model(),
                new OpenCoverCoverageParser(),
                new GenericCoverageWriter(),
                new OpenCoverCoverageParser(),
                new OpenCoverCoverageWriter());
            log.Info($"Converting {openCoverOutputPath} to {outputPath}");
            using (var fileWriter = new StreamWriter(outputPath))
            {
                using (var fileReader = new StreamReader(openCoverOutputPath))
                {

                    converter.Convert(fileWriter, fileReader);
                }
            }
            File.Delete(openCoverOutputPath);
        }

        private static void CreateTestResults(string testResultsPath, string resultsDirectory)
        {
            log.Info($"Creating test results file into {testResultsPath}");
            var files = Directory.EnumerateFiles(resultsDirectory);
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

        private static void ConsumeJobs(string resultsDirectory, string openCoverExePath, string targetPath, string targetArgs, string openCoverOutputPath, BlockingCollection<string> jobs,string coverageDirectory)
        {
            while (!jobs.IsCompleted)
            {
                string assembly = GetAssembly(jobs);
                if (assembly == null)
                {
                    continue;
                }
                log.Info($"Running unit test on {assembly}");
                string arguments = $"-register:user -\"output:{openCoverOutputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs} {assembly}\"";
                RunOpenCover(resultsDirectory, openCoverExePath, arguments,coverageDirectory);
            }
        }

        private static void RunOpenCover(string resultsDirectory, string openCoverExePath, string arguments,string coverageDirectory)
        {
            var runner = new Runner();
            runner.AddArgument(arguments);
            runner.SetPath(openCoverExePath);
            Task task = Task.Run(() => runner.Run());
            task.Wait();

            if (runner.TestResultsPath != null)
            {
                string testResultsPath = Path.Combine(resultsDirectory, Guid.NewGuid().ToString() + ".xml");
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

        public static  string CreateResultsDirectory()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "results_" +Guid.NewGuid().ToString()));
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }

        public static string CreateCoverageDirectory()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "coverage_" + Guid.NewGuid().ToString()));
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }
    }
}
