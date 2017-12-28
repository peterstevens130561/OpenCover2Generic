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
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;
using BHGE.SonarQube.OpenCover2Generic.Writers;

[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace BHGE.SonarQube.OpenCoverWrapper
{

    static class Program
    {
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
            ICodeCoverageRepository codeCoverageRepository = new CodeCoverageRepository(coverageStorageResolver,new OpenCoverCoverageParser());
            IJobConsumerFactory jobConsumerFactory = new JobConsumerFactory(openCoverCommandLineBuilder,
                jobFileSystemInfo, 
                openCoverManagerFactory,testResultsRepository,codeCoverageRepository);
            
            var testRunner = new TestRunner(jobFileSystemInfo,jobConsumerFactory);

            commandLineParser.Args = args;
            openCoverCommandLineBuilder.Args = args;

            testRunner.Initialize();
            try
            {
                int consumers = commandLineParser.GetParallelJobs();
                TimeSpan jobTimeOut = commandLineParser.GetJobTimeOut();
                jobFileSystemInfo.CreateRoot(DateTime.Now.ToString(@"yyMMdd_HHmmss"));
                codeCoverageRepository.RootDirectory = jobFileSystemInfo.GetIntermediateCoverageDirectory();

                testRunner.CreateJobConsumers(consumers, jobTimeOut);

                string[] testAssemblies = commandLineParser.GetTestAssemblies();
                int chunkSize = commandLineParser.GetChunkSize();
                testRunner.CreateJobs(testAssemblies, chunkSize);
                testRunner.Wait();

                string testResultsPath = commandLineParser.GetTestResultsPath();
                using (var writer = new StreamWriter(testResultsPath)) { 
                    testResultsRepository.Write(writer);
                }

                string outputPath = commandLineParser.GetOutputPath();
                var observer = new GenericCoverageWriterObserver(new GenericCoverageWriter());
                using (var writer = new XmlTextWriter(outputPath, Encoding.UTF8))
                {
                    observer.Writer = writer;
                    codeCoverageRepository.Scanner().AddObserver(observer).Scan();
                }
                
            } catch ( CommandLineArgumentException e)
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


    }
 
}
