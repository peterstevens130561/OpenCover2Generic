namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoint : IBranchPoint
    {
        public BranchPoint(int sourceLine,int path, bool isVisited)
        {
            Path = path;
            IsVisited = isVisited;
            SourceLine = sourceLine;
        }

        public BranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            FileId = fileId;
            Path = path;
            IsVisited = isVisited;
            SourceLine = sourceLine;
        }

        public int FileId { get; }

        public int SourceLine { get; }

        public int Path { get; }

        public bool IsVisited { get; }
    }
}