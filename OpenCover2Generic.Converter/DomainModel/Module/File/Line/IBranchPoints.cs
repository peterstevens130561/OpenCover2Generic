using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
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