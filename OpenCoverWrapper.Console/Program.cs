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
            string openCoverPath = commandLineParser.GetOpenCoverPath();
            string outputPath = commandLineParser.GetOutputPath();
            string targetPath = commandLineParser.GetTargetPath();
            string targetArgs = commandLineParser.GetTargetArgs();
            string testResultsPath = commandLineParser.GetTestResultsPath();
            string arguments = $"-register:user -\"output:{outputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs}\"";
            var runner = new Runner();
            runner.AddArgument(arguments);
            runner.SetPath(openCoverPath);
            runner.Run();
            File.Move(runner.TestResultsPath, testResultsPath);

        }
    }
}
