using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate
{
    public interface ICreateCoverageResultsCommand : ICommand
    {
        IWorkspace Workspace { get; set; }
        string[] Args { get; set; }
    }
}