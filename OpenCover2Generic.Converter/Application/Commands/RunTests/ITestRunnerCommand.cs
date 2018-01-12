using System;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public interface ITestRunnerCommand : ICommand
    {
        int ChunkSize { get; set; }
        TimeSpan JobTimeOut { get; set; }
        int ParallelJobs { get; set; }
        string[] TestAssemblies { get; set; }
    }
}