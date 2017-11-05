using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface IBranchPoints
    {
        IList<IBranchPoint> GetBranchPoints();

        IBranchPoints Add(int sourceLine,int path, bool isVisited);

        IBranchPoints Add(IBranchPoint branchPoint);
        int PathsToCover();
        int CoveredPaths();
    }
}