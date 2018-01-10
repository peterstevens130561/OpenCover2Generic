using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete
{
    class WorkspaceDeleteCommandHandler : ICommandHandler<IWorkspaceDeleteCommand>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public WorkspaceDeleteCommandHandler()
        {
            
        }

        public WorkspaceDeleteCommandHandler(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }
        public void Execute(IWorkspaceDeleteCommand command)
        {
            string path = command.Workspace.Path;
            _fileSystemAdapter.DirectoryDelete(path,true);
        }
    }
}
