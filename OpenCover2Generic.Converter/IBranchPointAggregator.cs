using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IBranchPointAggregator
    {
        IList<IBranchPoint> GetBranchPoints();

        IBranchPointAggregator Add(int sourceLine,int path, bool isVisited);

        IBranchPointAggregator Add(IBranchPoint branchPoint);
        int PathsToCover();
        int CoveredPaths();
    }
}