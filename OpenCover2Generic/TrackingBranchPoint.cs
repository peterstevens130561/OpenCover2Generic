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
        private readonly IDictionary<int, bool> pathsToCover = new Dictionary<int, bool>();


        public BranchPointAggregator()
        {
        }
 

        public int PathsToCover()
        {
                return pathsToCover.Count;
        }

        public int CoveredPaths()
        {
                return pathsToCover.Count(p => { return p.Value; });
        }


        public IBranchPointAggregator Add(int path, bool isVisited)
        {
            AddPoint(path, isVisited);
            return this;
        }

        private void AddPoint(int path, bool isVisited)
        {
            pathsToCover[path] = pathsToCover.ContainsKey(path) ? pathsToCover[path] || isVisited : isVisited;
        }
    }
}