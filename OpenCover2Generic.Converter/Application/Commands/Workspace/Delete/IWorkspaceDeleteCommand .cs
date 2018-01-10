using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete
{
    interface IWorkspaceDeleteCommand : ICommand
    {
        IWorkspace Workspace{ get; set; }
    }
}