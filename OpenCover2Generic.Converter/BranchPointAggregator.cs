using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPointAggregator : IBranchPointAggregator
    {
        private readonly SortedDictionary<int, IBranchPoint> pathsToCover = new SortedDictionary<int, IBranchPoint>();



        public BranchPointAggregator()
        {
        }

        public IList<IBranchPoint> GetBranchPoints()
        {
            return pathsToCover.Values.ToList();
        }

        public int PathsToCover()
        {
                return pathsToCover.Count;
        }

        public int CoveredPaths()
        {
                return pathsToCover.Count(p => { return p.Value.IsVisited; });
        }


        public IBranchPointAggregator Add(int sourceLine, int path, bool isVisited)
        {
            IBranchPoint branchPoint = new BranchPoint(sourceLine, path, isVisited);
            Add(branchPoint);
            return this;
        }



        public IBranchPointAggregator Add(IBranchPoint branchPoint)
        {
            int path = branchPoint.Path;
            if (!pathsToCover.ContainsKey(path) || !pathsToCover[path].IsVisited)
            {
                pathsToCover[path] = branchPoint;
            }
            return this;
        }

    }
}