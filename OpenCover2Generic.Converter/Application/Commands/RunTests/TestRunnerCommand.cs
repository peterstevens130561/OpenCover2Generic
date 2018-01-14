
using System;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public class TestRunnerCommand : ITestRunnerCommand
    {
        public int ParallelJobs { get; set; }

        public string[] TestAssemblies { get; set; }

        public int ChunkSize { get; set; }

        public TimeSpan JobTimeOut { get; set; }

        public string[] Args { get; set; }

    }
}
