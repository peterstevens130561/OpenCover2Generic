using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create
{
    public class WorkspaceCreateCommand : IWorkspaceCreateCommand
    {
        public IWorkspace Workspace{ get; set; }
}
}