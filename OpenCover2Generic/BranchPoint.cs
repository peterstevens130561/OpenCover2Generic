using System;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoint : IBranchPoint
    {
        private readonly int path;
        private readonly int visitedCount;


        public BranchPoint(int visitedCount, int path)
        {
            this.visitedCount = visitedCount;
            this.path = path;
        }

        public int Path
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int VisitedCount
        {
            get
            {
                return visitedCount;
            }
        }

        public IBranchPoint Add(IBranchPoint branchPoint)
        {
            return new BranchPoint(visitedCount + branchPoint.VisitedCount, System.Math.Max(path, branchPoint.Path));
        }
    }
}