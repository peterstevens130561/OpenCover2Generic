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

    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            commandLineParser.Args = args;
            string outputPath = commandLineParser.GetOutputPath();
            string testResultsPath = commandLineParser.GetTestResultsPath();
            var converter = new MultiAssemblyConverter();

            var openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            openCoverCommandLineBuilder.Args = args;
            var openCoverManagerFactory = new OpenCoverManagerFactory(new ProcessFactory());
            var testRunner = new TestRunner(converter,openCoverManagerFactory);
            string[] testAssemblies = commandLineParser.GetTestAssemblies();

            testRunner.Initialize();
            testRunner.RunTests(openCoverCommandLineBuilder, testAssemblies);
            testRunner.CreateTestResults(testResultsPath);
            testRunner.CreateCoverageFile(outputPath);
        }
    }
 
}
