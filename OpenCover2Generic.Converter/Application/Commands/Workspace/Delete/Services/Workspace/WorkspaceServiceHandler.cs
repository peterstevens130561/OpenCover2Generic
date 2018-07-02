using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete.Services.Workspace
{
    class WorkspaceServiceHandler : IServiceHandler<IWorkspace, IWorkspaceService>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public WorkspaceServiceHandler() : this(new FileSystemAdapter())
        {
            
        }

        internal WorkspaceServiceHandler(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        public IWorkspace Execute(IWorkspaceService service)
        {
            string path = Path.GetFullPath(Path.Combine(_fileSystemAdapter.GetTempPath(), "opencover_" + service.Id));
            return new DomainModel.Workspace.Workspace(path);
        }
    }
}
