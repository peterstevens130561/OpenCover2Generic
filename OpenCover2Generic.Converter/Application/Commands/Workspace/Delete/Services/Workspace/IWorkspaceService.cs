using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete.Services.Workspace
{
    public interface IWorkspaceService: IServiceBase<IWorkspace,IWorkspaceService>
    {
        string Id { get; set; }

    }
}
