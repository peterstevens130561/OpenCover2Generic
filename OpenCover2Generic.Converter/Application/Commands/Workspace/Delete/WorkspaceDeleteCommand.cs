
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete
{
    public class WorkspaceDeleteCommand : IWorkspaceDeleteCommand
    {
        public IWorkspace Workspace{ get; set; }
}
}