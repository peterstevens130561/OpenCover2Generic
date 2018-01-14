
using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public class TestRunnerCommand : ITestRunnerCommand
    {

        public string[] Args { get; set; }
        public IWorkspace Workspace { get; set; }
    }
}
