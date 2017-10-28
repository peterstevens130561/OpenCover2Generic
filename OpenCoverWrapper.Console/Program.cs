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
            var jobs = new BlockingCollection<string>();
            testAssemblies.ToList().ForEach(a => jobs.Add(a));
            jobs.CompleteAdding();
            string resultsDirectory = CreateResultsDirectory();

            var tasks = new List<Task>();
                for (int i = 1; i <= 5;i++) {
                log.Info($"starting task {i}");
                Task task = Task.Run(() => RunJobs(resultsDirectory,openCoverExePath, targetPath, targetArgs, openCoverOutputPath, jobs));
                tasks.Add(task);
            }
            tasks.ForEach(t => t.Wait());
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
            log.Info("Assembling coverage file");
            var converter = new MultiAssemblyConverter(new Model(),
                new OpenCoverCoverageParser(),
                new GenericCoverageWriter(),
                new OpenCoverCoverageParser(),
                new OpenCoverCoverageWriter());
            Console.WriteLine($"Converting {openCoverOutputPath} to {outputPath}");
            using (var fileWriter = new StreamWriter(outputPath))
            {
                using (var fileReader = new StreamReader(openCoverOutputPath))
                {

                    converter.Convert(fileWriter, fileReader);
                }
            }
            File.Delete(openCoverOutputPath);
        }

        private static void RunJobs(string resultsDirectory, string openCoverExePath, string targetPath, string targetArgs, string openCoverOutputPath, BlockingCollection<string> jobs)
        {
            while (!jobs.IsCompleted)
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
                if (assembly == null)
                {
                    continue;
                }
                log.Info($"Running unit test on {assembly}");
                string arguments = $"-register:user -\"output:{openCoverOutputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs} {assembly}\"";
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
        }

        public static  string CreateResultsDirectory()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "results_" +Guid.NewGuid().ToString()));
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }
    }
}
