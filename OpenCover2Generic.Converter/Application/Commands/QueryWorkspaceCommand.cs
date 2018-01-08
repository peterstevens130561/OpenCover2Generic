using BHGE.SonarQube.OpenCover2Generic.Application.Services.Workspace;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands
{
    class WorkspaceService : ServiceBase<IWorkspace, IWorkspaceService>,IWorkspaceService
    {
        public WorkspaceService(IServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        public string Id { get; set; }
    }
}
