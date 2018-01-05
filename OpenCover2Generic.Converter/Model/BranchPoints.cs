using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Immutable branchpoint
    /// </summary>
    internal class BranchPoints : IBranchPoints
    {
        private readonly SortedDictionary<int, IBranchPointValue> pathsToCover = new SortedDictionary<int, IBranchPointValue>();



        public BranchPoints()
        {
        }

        public IList<IBranchPointValue> GetBranchPoints()
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
            IBranchPointValue branchPointValue = new BranchPointValueValue(sourceLine, path, isVisited);
            Add(branchPointValue);
            return this;
        }



        public IBranchPoints Add(IBranchPointValue branchPointValue)
        {
            int path = branchPointValue.Path;
            if (!pathsToCover.ContainsKey(path) || !pathsToCover[path].IsVisited)
            {
                pathsToCover[path] = branchPointValue;
            }
            return this;
        }

    }
}