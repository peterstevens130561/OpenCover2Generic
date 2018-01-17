using System;
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
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.TestResultsCreate;
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
            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser();       


            commandLineParser.Args = args;

            try
            {
                string id = DateTime.Now.ToString(@"yyMMdd_HHmmss");
                var workspaceService = serviceBus.Create<IWorkspaceService>();
                workspaceService.Id = id;
                var workspace = serviceBus.Execute(workspaceService);
                JobFileSystem jobFileSystem = new JobFileSystem();
                jobFileSystem.CreateRoot(workspace);
                //CreateWorkspace(commandBus, workspace);

                ICodeCoverageRepository codeCoverageRepository = new CodeCoverageRepository(
                    new CoverageStorageResolver(),
                    new OpenCoverCoverageParser(),
                    new XmlAdapter(),
                    new CoverageWriterFactory());
                codeCoverageRepository.RootDirectory = jobFileSystem.GetIntermediateCoverageDirectory();

                RunTests(commandBus,args,workspace);

                CreateTestResults(commandBus,workspace,args);
                CreateCoverageResults(commandBus,commandLineParser, codeCoverageRepository);

                DeleteWorkspace(commandBus, workspace);

            }
            catch ( CommandLineArgumentException e)
            {
                Console.Error.WriteLine("Invalid command line argument");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            } catch ( JobTimeOutException e)
            {
                Console.Error.WriteLine("Timeout");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            } catch (Exception e)
            {
                Console.Error.WriteLine("Exception");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                if (e.InnerException != null)
                {
                    Console.Error.WriteLine("Inner exception");
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

        private static void RunTests(ICommandBus commandBus,string[] args,IWorkspace workspace)
        {

            ITestRunnerCommand command = commandBus.CreateCommand<ITestRunnerCommand>();
            command.Args = args;
            command.Workspace = workspace;

            commandBus.Execute(command);
        }

        private static void CreateTestResults(ICommandBus commandBus,IWorkspace workspace,string[] args)
        {
            var command = commandBus.CreateCommand<ITestResultsCreateCommand>();
            command.Workspace = workspace;
            command.Args = args;
            commandBus.Execute(command);
        }

        private static void CreateCoverageResults(ICommandBus commandBus,IOpenCoverWrapperCommandLineParser commandLineParser, ICodeCoverageRepository codeCoverageRepository)
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
