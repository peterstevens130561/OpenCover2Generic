using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public class TestRunnerCommandHandler : ITestRunner, ICommandHandler<ITestRunnerCommand>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestRunnerCommandHandler));
        private readonly IJobConsumerFactory _jobConsumerFactory;
        private readonly List<Task> _tasks = new List<Task>();
        private readonly IJobs _jobs = new Jobs();
        public TestRunnerCommandHandler( IJobConsumerFactory jobConsumerFactory)
        {
            _jobConsumerFactory = jobConsumerFactory;

        }

        public void Execute(ITestRunnerCommand command)
        {
  
            CreateJobs(command.TestAssemblies, command.ChunkSize);
            CreateJobConsumers(command.ParallelJobs, command.JobTimeOut);
            Wait();
        }


        public void CreateJobs(string[] testAssemblies, int chunkSize)
        {
            log.Info($"Will run tests for {testAssemblies.Count()} assemblies");
            var list = testAssemblies.ToList();
            int count = testAssemblies.Count();
            int currentChunkSize = chunkSize;
            for (int index=0;index<count;index+=currentChunkSize)
            {
                currentChunkSize = Math.Min(currentChunkSize, count - index);
                var chunk = list.GetRange(index, currentChunkSize);
                _jobs.Add(new TestJob(chunk));
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
                throw e.InnerException;
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
