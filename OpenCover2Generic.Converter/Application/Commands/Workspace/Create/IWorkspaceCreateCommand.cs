using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
namespace BHGE.SonarQube.OpenCover2Generic.Commands
{
    interface IWorkspaceCreateCommand : ICommand
    {
        IWorkspace Workspace{ get; set; }
    }
}