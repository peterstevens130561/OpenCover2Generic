using System;
using System.IO;
using System.Text;
using log4net.Config;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;

[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace BHGE.SonarQube.OpenCoverWrapper
{

    static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());       
            var fileSystem = new FileSystemAdapter();
            IOpenCoverCommandLineBuilder openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            JobFileSystem jobFileSystemInfo = new JobFileSystem(fileSystem);
            IOpenCoverManagerFactory openCoverManagerFactory = new OpenCoverManagerFactory(new OpenCoverProcessFactory(new ProcessFactory()));

            var testResultsRepository = new TestResultsRepository(jobFileSystemInfo, fileSystem);
            IFileSystemAdapter fileSystemAdapter = new FileSystemAdapter();
            ICoverageStorageResolver coverageStorageResolver = new CoverageStorageResolver(fileSystemAdapter);
            ICodeCoverageRepository codeCoverageRepository = new CodeCoverageRepository(
                coverageStorageResolver,
                new OpenCoverCoverageParser(),
                new XmlAdapter(),
                new CoverageWriterFactory());
            IOpenCoverageParserFactory openCoverageParserFactory = new OpenCoverageParserFactory();
            ICoverageAggregateFactory coverageAggregateFactory=new CoverageAggregateFactory(openCoverageParserFactory);
            IJobConsumerFactory jobConsumerFactory = new JobConsumerFactory(openCoverCommandLineBuilder,
                jobFileSystemInfo, 
                openCoverManagerFactory,
                testResultsRepository,
                codeCoverageRepository,
                coverageAggregateFactory);
            
            var testRunner = new TestRunner(jobFileSystemInfo,jobConsumerFactory);

            commandLineParser.Args = args;
            openCoverCommandLineBuilder.Args = args;

            testRunner.Initialize();
            try
            {
                jobFileSystemInfo.CreateRoot(DateTime.Now.ToString(@"yyMMdd_HHmmss"));
                codeCoverageRepository.RootDirectory = jobFileSystemInfo.GetIntermediateCoverageDirectory();

                RunTests(commandLineParser, testRunner);

                CreateTestResults(commandLineParser, testResultsRepository);
                CreateCoverageResults(commandLineParser, codeCoverageRepository);

            }
            catch ( CommandLineArgumentException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            } catch ( JobTimeOutException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            } catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                if (e.InnerException != null)
                {
                    Console.Error.WriteLine(e.InnerException.Message);
                    Console.Error.WriteLine(e.InnerException.StackTrace);
                }
                Environment.Exit(1);
            }
        }

        private static void RunTests(IOpenCoverWrapperCommandLineParser commandLineParser, TestRunner testRunner)
        {
            int consumers = commandLineParser.GetParallelJobs();
            TimeSpan jobTimeOut = commandLineParser.GetJobTimeOut();
            testRunner.CreateJobConsumers(consumers, jobTimeOut);

            string[] testAssemblies = commandLineParser.GetTestAssemblies();
            int chunkSize = commandLineParser.GetChunkSize();
            testRunner.CreateJobs(testAssemblies, chunkSize);
            testRunner.Wait();
        }

        private static void CreateTestResults(IOpenCoverWrapperCommandLineParser commandLineParser, TestResultsRepository testResultsRepository)
        {
            string testResultsPath = commandLineParser.GetTestResultsPath();
            using (var writer = new StreamWriter(testResultsPath))
            {
                testResultsRepository.Write(writer);
            }
        }

        private static void CreateCoverageResults(IOpenCoverWrapperCommandLineParser commandLineParser, ICodeCoverageRepository codeCoverageRepository)
        {
            string outputPath = commandLineParser.GetOutputPath();
            var genericCoverageWriterObserver = new GenericCoverageWriterObserver(new GenericCoverageWriter());
            var statisticsObserver = new CoverageStatisticsAggregator();
            using (var writer = new XmlTextWriter(outputPath, Encoding.UTF8))
            {
                genericCoverageWriterObserver.Writer = writer;
                codeCoverageRepository.QueryAllModules()
                    .AddObserver(genericCoverageWriterObserver)
                    .AddObserver(statisticsObserver)
                    .Execute();
            }
            _log.Info($"Files         : {statisticsObserver.Files}");
            _log.Info($"Lines         : {statisticsObserver.Lines} ");
            _log.Info($"Covered Lines : {statisticsObserver.CoveredLines}");
        }
    }
 
}
