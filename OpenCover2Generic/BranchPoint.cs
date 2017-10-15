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



        public BranchPoint(int path, bool isVisited) 
        {
            this.path = path;
            this.isVisited = isVisited;
        }

        public BranchPoint(int sourceLine,int path, bool isVisited)
        {
            this.path = path;
            this.isVisited = isVisited;
            this.sourceLine = sourceLine;
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