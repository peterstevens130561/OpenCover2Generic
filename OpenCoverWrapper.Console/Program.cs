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
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.Repositories;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BHGE.SonarQube.OpenCoverWrapper
{

    static class Program
    {
        public static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());       
            var converter = new MultiAssemblyConverter();
            var fileSystem = new FileSystemAdapter();
            IOpenCoverCommandLineBuilder openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            JobFileSystem jobFileSystemInfo = new JobFileSystem(new FileSystemAdapter());
            IOpenCoverManagerFactory openCoverManagerFactory = new OpenCoverManagerFactory(new ProcessFactory());
            var testResultsRepository = new TestResultsRepository(jobFileSystemInfo, fileSystem);
            IJobConsumerFactory jobConsumerFactory = new JobConsumerFactory(openCoverCommandLineBuilder,
                jobFileSystemInfo, 
                openCoverManagerFactory,testResultsRepository);
            
            var testRunner = new TestRunner(jobFileSystemInfo,converter,jobConsumerFactory);

            commandLineParser.Args = args;
            openCoverCommandLineBuilder.Args = args;

            testRunner.Initialize();
            try
            {
                int consumers = commandLineParser.GetParallelJobs();
                TimeSpan jobTimeOut = commandLineParser.GetJobTimeOut();
                testRunner.CreateJobConsumers(consumers, jobTimeOut);

                string[] testAssemblies = commandLineParser.GetTestAssemblies();
                int chunkSize = commandLineParser.GetChunkSize();
                testRunner.CreateJobs(testAssemblies, chunkSize);
                string testResultsPath = commandLineParser.GetTestResultsPath();

                testRunner.Wait();

                using (var writer = new StreamWriter(testResultsPath)) { 
                    testResultsRepository.Write(writer);
                }

                string outputPath = commandLineParser.GetOutputPath();
                testRunner.CreateCoverageFile(outputPath);
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
                Environment.Exit(1);
            }
        }


    }
 
}
