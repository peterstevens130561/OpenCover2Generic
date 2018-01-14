using System;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public interface ITestRunnerCommand : ICommand
    {
        string[] Args { get; set; }
        IWorkspace Workspace { get; set; }
    }
}