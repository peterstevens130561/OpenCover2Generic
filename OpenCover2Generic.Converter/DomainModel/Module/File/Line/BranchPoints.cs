using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoints : IBranchPoints
    {
        private readonly SortedDictionary<int, IBranchPoint> pathsToCover = new SortedDictionary<int, IBranchPoint>();



        public BranchPoints()
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


        public IBranchPoints Add(int sourceLine, int path, bool isVisited)
        {
            IBranchPoint branchPoint = new BranchPoint(sourceLine, path, isVisited);
            Add(branchPoint);
            return this;
        }



        public IBranchPoints Add(IBranchPoint branchPoint)
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