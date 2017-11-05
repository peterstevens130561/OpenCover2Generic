namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface IBranchPoint
    {
        int Path { get; }
        bool IsVisited { get; }
        int SourceLine { get; }

        int FileId { get; }
    }
}