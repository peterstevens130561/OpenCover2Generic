namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPointValueValue : IBranchPointValue
    {
        public BranchPointValueValue(int sourceLine,int path, bool isVisited)
        {
            Path = path;
            IsVisited = isVisited;
            SourceLine = sourceLine;
        }

        public BranchPointValueValue(int fileId, int sourceLine, int path, bool isVisited)
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