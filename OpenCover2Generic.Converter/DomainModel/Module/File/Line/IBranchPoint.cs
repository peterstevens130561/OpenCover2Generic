namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
{
    public interface IBranchPoint
    {
        int Path { get; }
        bool IsVisited { get; }
        int SourceLine { get; }

        int FileId { get; }
    }
}