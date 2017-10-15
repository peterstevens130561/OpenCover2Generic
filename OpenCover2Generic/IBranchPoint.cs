namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPoint
    {
        int Path { get; }
        bool IsVisited { get; }
        int SourceLine { get; }
    }
}