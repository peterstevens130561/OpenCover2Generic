using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using BHGE.SonarQube.OpenCoverWrapper;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public class TestRunnerCommandHandler : ICommandHandler<ITestRunnerCommand>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestRunnerCommandHandler));
        private readonly IJobConsumerFactory _jobConsumerFactory;
        private readonly List<Task> _tasks = new List<Task>();
        private readonly IJobs _jobs = new Jobs();
        private readonly IOpenCoverWrapperCommandLineParser _commandLineParser;
        private readonly IJobFileSystem _jobFileSystem;

        public TestRunnerCommandHandler()
        {

            _jobFileSystem = new JobFileSystem();
            _jobConsumerFactory = new JobConsumerFactory(_jobFileSystem);
            _commandLineParser = new OpenCoverWrapperCommandLineParser();
        }
        public TestRunnerCommandHandler(IJobConsumerFactory jobConsumerFactory) : this(jobConsumerFactory,
            new OpenCoverWrapperCommandLineParser(new CommandLineParser()))
        {
            
        }
        public TestRunnerCommandHandler( IJobConsumerFactory jobConsumerFactory,IOpenCoverWrapperCommandLineParser commandLineParser)
        {
            _jobConsumerFactory = jobConsumerFactory;
            _commandLineParser = commandLineParser;

        }

        public void Execute(ITestRunnerCommand command)
        {
            var workspace = command.Workspace;
            _jobFileSystem.CreateRoot(workspace);
            _commandLineParser.Args = command.Args;
            CreateJobs(_commandLineParser.GetTestAssemblies(), _commandLineParser.GetChunkSize(),command.Args,workspace);
            CreateJobConsumers(_commandLineParser.GetParallelJobs(), _commandLineParser.GetJobTimeOut());
            Wait();
        }


        public void CreateJobs(string[] testAssemblies, int chunkSize, string[] args,IWorkspace workspace)
        {
            log.Info($"Will run tests for {testAssemblies.Count()} assemblies");
            var list = testAssemblies.ToList();
            int count = testAssemblies.Count();
            int currentChunkSize = chunkSize;
            for (int index=0;index<count;index+=currentChunkSize)
            {
                currentChunkSize = Math.Min(currentChunkSize, count - index);
                var chunk = list.GetRange(index, currentChunkSize);
                _jobs.Add(new TestJob(chunk,args,_jobFileSystem.GetIntermediateCoverageDirectory(),workspace));
            }
            _jobs.CompleteAdding();
        }

        public void Wait()
        {
            try
            {
                _tasks.ForEach(t => t.Wait());
            } catch (AggregateException e)
            {
                log.Error($"Aggregate Exception caught {e.Message}\n{e.StackTrace}");
                throw e.InnerException;
            }
            catch (Exception e)
            {
                log.Error($"Exception caught {e}\n{e.Message}\n{e.StackTrace}");
                throw;
            }
        }
        public IJobs Jobs { get { return _jobs; } }


        public void CreateJobConsumers(int consumers,TimeSpan jobTimeOut)
        {
            for (int i = 1; i <= consumers; i++)
            {
                Task task = Task.Run(() => _jobConsumerFactory.Create().ConsumeTestJobs(_jobs,jobTimeOut));
                _tasks.Add(task);
            }
        }


    }
}
