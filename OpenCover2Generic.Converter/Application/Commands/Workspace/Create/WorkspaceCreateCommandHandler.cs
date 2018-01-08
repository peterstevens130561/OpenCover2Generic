using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Bus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create
{
    class WorkspaceCreateCommandHandler : ICommandHandler<IWorkspaceCreateCommand>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public WorkspaceCreateCommandHandler()
        {
            
        }

        public WorkspaceCreateCommandHandler(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }
        public void Execute(IWorkspaceCreateCommand command)
        {
            string path = command.Workspace.Path;
            _fileSystemAdapter.CreateDirectory(path);
        }
    }
}
