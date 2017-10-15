namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPointAggregator
    {
        IBranchPointAggregator Add(int path, bool isVisited);

        IBranchPointAggregator Add(IBranchPoint branchPoint);
        int PathsToCover();
        int CoveredPaths();
    }
}