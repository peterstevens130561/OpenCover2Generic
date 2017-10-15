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



        public BranchPoint(int path, bool isVisited) 
        {
            this.path = path;
            this.isVisited = isVisited;
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