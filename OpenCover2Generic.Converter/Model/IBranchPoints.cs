using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface IBranchPoints
    {
        IList<IBranchPointValue> GetBranchPoints();

        IBranchPoints Add(int sourceLine,int path, bool isVisited);

        IBranchPoints Add(IBranchPointValue branchPointValue);
        int PathsToCover();
        int CoveredPaths();
    }
}