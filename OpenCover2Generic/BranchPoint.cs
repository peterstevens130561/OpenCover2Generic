using System;
using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoint : IBranchPoint
    {
        private readonly int path;
        private readonly bool isVisited;
        private readonly int sourceLine;
        private readonly int fileId;


        public BranchPoint(int sourceLine,int path, bool isVisited)
        {
            this.path = path;
            this.isVisited = isVisited;
            this.sourceLine = sourceLine;
        }

        public BranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            this.fileId = fileId;
            this.path = path;
            this.isVisited = isVisited;
            this.sourceLine = sourceLine;
        }

        public int FileId
        {
            get
            {
                return fileId;
            }
        }
        public int SourceLine
        {
            get
            {
                return sourceLine;
            }
        }

        public int Path
        {
            get
            {
                return path;
            }
        }

        public bool IsVisited
        {
            get
            {
                return isVisited;
            }
        }

    }
}