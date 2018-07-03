using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.TestResultsCreate
{
    public interface ITestResultsCreateCommand : ICommand
    {
        string[] Args { get; set; }
        IWorkspace Workspace { get; set; }
    }
}