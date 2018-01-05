using System;
using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPointValue : IBranchPoint
    {
        public BranchPointValue(int sourceLine,int path, bool isVisited)
        {
            Path = path;
            IsVisited = isVisited;
            SourceLine = sourceLine;
        }

        public BranchPointValue(int fileId, int sourceLine, int path, bool isVisited)
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