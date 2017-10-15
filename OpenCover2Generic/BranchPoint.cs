using System;
using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoint : IBranchPoint
    {
        private readonly int paths;
        private int v;
        private readonly int visitedCount;
        private readonly IDictionary<int, bool> pathsToCover = new Dictionary<int, bool>();

        public BranchPoint(Boolean isVisited) : this(isVisited ? 1 : 0, 1)
        { }

        public BranchPoint(int path, bool isVisited) : this(isVisited)
        {
            pathsToCover[path]=pathsToCover.ContainsKey(path)?pathsToCover[path]||isVisited:isVisited; 
        }
 
        /// <summary>
        /// visitedCount = the number of times a path is covered
        /// </summary>
        /// <param name="branchesVisited"></param>
        /// <param name="pathId"></param>
        private  BranchPoint(int branchesVisited, int pathId)
        {
            this.visitedCount = branchesVisited;
            this.paths = pathId;
        }

        public int Paths
        {
            get
            {
                return paths;
            }
        }

        public int PathsVisited
        {
            get
            {
                return visitedCount;
            }
        }

        public IBranchPoint Add(IBranchPoint branchPoint)
        {
            return new BranchPoint(visitedCount + branchPoint.PathsVisited, branchPoint.Paths+Paths);
        }

        public IBranchPoint Add(int path, bool isVisited)
        {
            throw new NotImplementedException();
        }
    }
}