using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Commands;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandler
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
