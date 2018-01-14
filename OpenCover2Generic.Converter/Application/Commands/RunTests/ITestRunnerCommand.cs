using System;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public interface ITestRunnerCommand : ICommand
    {
        string[] Args { get; set; }

    }
}