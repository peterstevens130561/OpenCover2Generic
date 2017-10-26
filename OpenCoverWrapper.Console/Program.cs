using BHGE.SonarQube.OpenCover2Generic;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            commandLineParser.Args = args;
            string openCoverExePath = commandLineParser.GetOpenCoverPath();
            string outputPath = commandLineParser.GetOutputPath();
            string targetPath = commandLineParser.GetTargetPath();
            string targetArgs = commandLineParser.GetTargetArgs();
            string testResultsPath = commandLineParser.GetTestResultsPath();
            string openCoverOutputPath = Path.GetTempFileName();
            string[] testAssemblies = commandLineParser.GetTestAssemblies();
            string arguments = $"-register:user -\"output:{openCoverOutputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs}\"";
            foreach (string assembly in testAssemblies)
            {
                var runner = new Runner();
                runner.AddArgument(arguments);
                runner.SetPath(openCoverExePath);
                runner.SetAssembly(assembly);
                Task task = Task.Run(() => runner.Run());
                task.Wait();
                if (File.Exists(testResultsPath))
                {
                    File.Delete(testResultsPath);
                }
                File.Move(runner.TestResultsPath, testResultsPath);
            }

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
