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
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.Factories;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BHGE.SonarQube.OpenCoverWrapper
{

    static class Program
    {
        public static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());       
            var converter = new MultiAssemblyConverter();

            var openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            JobFileSystem jobFileSystemInfo = new JobFileSystem(new FileSystemAdapter());
            IOpenCoverManagerFactory openCoverManagerFactory = new OpenCoverManagerFactory(new ProcessFactory());
            IJobConsumerFactory jobConsumerFactory = new JobConsumerFactory(openCoverCommandLineBuilder,
                jobFileSystemInfo, 
                openCoverManagerFactory);
            
            var testRunner = new TestRunner(jobFileSystemInfo,converter,jobConsumerFactory);

            commandLineParser.Args = args;
            openCoverCommandLineBuilder.Args = args;

            testRunner.Initialize();

            string[] testAssemblies = commandLineParser.GetTestAssemblies();
            testRunner.RunTests(testAssemblies,5);

            string testResultsPath = commandLineParser.GetTestResultsPath();
            testRunner.CreateTestResults(testResultsPath);

            string outputPath = commandLineParser.GetOutputPath();
            testRunner.CreateCoverageFile(outputPath);
        }
    }
 
}
