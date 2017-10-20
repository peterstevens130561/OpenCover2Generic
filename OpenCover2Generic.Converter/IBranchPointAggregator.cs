namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IBranchPointAggregator
    {
        IBranchPointAggregator Add(int path, bool isVisited);

        IBranchPointAggregator Add(IBranchPoint branchPoint);
        int PathsToCover();
        int CoveredPaths();
    }
}