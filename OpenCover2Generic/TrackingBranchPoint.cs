using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class TrackingBranchPoint : IBranchPoint
    {
        private readonly IDictionary<int, bool> pathsToCover = new Dictionary<int, bool>();


        public TrackingBranchPoint()
        {
        }
 

        public int Paths
        {
            get
            {
                return pathsToCover.Count;
            }
        }

        public int PathsVisited
        {
            get
            {
                return pathsToCover.Count(p => { return p.Value; });
            }
        }

        public IBranchPoint Add(IBranchPoint branchPoint)
        {
            throw new NotImplementedException("should not be called, you are using the wrong type");
            return this;
        }

        public IBranchPoint Add(int path, bool isVisited)
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