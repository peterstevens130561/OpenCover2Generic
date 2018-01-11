﻿using System;
using System.IO;
using System.Text;
using log4net.Config;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Application;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete.Services.Workspace;
using BHGE.SonarQube.OpenCover2Generic.CoverageConverters.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace BHGE.SonarQube.OpenCoverWrapper
{

    static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
        public static void Main(string[] args)
        {
            var commandBus = new ApplicationCommandBus();
            var serviceBus = new ApplicationServiceBus();
            XmlConfigurator.Configure();
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());       
            var fileSystem = new FileSystemAdapter();
            IOpenCoverCommandLineBuilder openCoverCommandLineBuilder = new OpenCoverCommandLineBuilder(new CommandLineParser());
            JobFileSystem jobFileSystem = new JobFileSystem(fileSystem);
            IOpenCoverManagerFactory openCoverManagerFactory = new OpenCoverManagerFactory(new OpenCoverProcessFactory(new ProcessFactory()));

            var testResultsRepository = new TestResultsRepository(jobFileSystem, fileSystem);
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
                jobFileSystem, 
                openCoverManagerFactory,
                testResultsRepository,
                codeCoverageRepository,
                coverageAggregateFactory);
            
            var testRunner = new TestRunnerCommandHandler(jobConsumerFactory);

            commandLineParser.Args = args;
            openCoverCommandLineBuilder.Args = args;

            try
            {
                string id = DateTime.Now.ToString(@"yyMMdd_HHmmss");
                var workspaceService = serviceBus.Create<IWorkspaceService>();
                workspaceService.Id = id;
                var workspace = serviceBus.Execute(workspaceService);
                jobFileSystem.CreateRoot(id);
                //CreateWorkspace(commandBus, workspace);

                codeCoverageRepository.RootDirectory = jobFileSystem.GetIntermediateCoverageDirectory();

                RunTests(commandLineParser, testRunner);

                CreateTestResults(commandLineParser, testResultsRepository);
                CreateCoverageResults(commandLineParser, codeCoverageRepository);

                DeleteWorkspace(commandBus, workspace);

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

        private static void DeleteWorkspace(ICommandBus commandBus, IWorkspace workspace)
        {
            var workspaceDeleteCommand = commandBus.CreateCommand<IWorkspaceDeleteCommand>();
            workspaceDeleteCommand.Workspace = workspace;
            commandBus.Execute(workspaceDeleteCommand);
        }

        private static void CreateWorkspace(ICommandBus commandBus, IWorkspace workspace)
        {
            var workspaceCreateCommand = commandBus.CreateCommand<IWorkspaceCreateCommand>();
            workspaceCreateCommand.Workspace = workspace;
            commandBus.Execute(workspaceCreateCommand);
        }

        private static IWorkspace DefineWorkspace(IServiceBus serviceBus)
        {
            var workspaceService = serviceBus.Create<IWorkspaceService>();
            workspaceService.Id = DateTime.Now.ToString(@"yyMMdd_HHmmss");

            var workspace = serviceBus.Execute(workspaceService);
            return workspace;
        }

        private static void RunTests(IOpenCoverWrapperCommandLineParser commandLineParser, TestRunnerCommandHandler testRunnerCommandHandler)
        {
            int consumers = commandLineParser.GetParallelJobs();
            TimeSpan jobTimeOut = commandLineParser.GetJobTimeOut();
            testRunnerCommandHandler.CreateJobConsumers(consumers, jobTimeOut);

            string[] testAssemblies = commandLineParser.GetTestAssemblies();
            int chunkSize = commandLineParser.GetChunkSize();
            testRunnerCommandHandler.CreateJobs(testAssemblies, chunkSize);
            testRunnerCommandHandler.Wait();
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
