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

            while(!jobs.IsCompleted)
            {
                string assembly = null;
                try
                {
                    assembly = jobs.Take();
                } catch ( InvalidOperationException e)
                {
                    log.Debug("Exception on take (ignored");
                }
                if(assembly==null)
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
                if (File.Exists(testResultsPath))
                {
                    File.Delete(testResultsPath);
                }
                if (runner.TestResultsPath != null)
                {
                    File.Move(runner.TestResultsPath, testResultsPath);
                }
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


        public string CreateRootDirectory()
        {
            var rootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "coverage_" +Guid.NewGuid().ToString()));
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }
    }
}
