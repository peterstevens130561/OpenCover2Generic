namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPointAggregator
    {
        IBranchPointAggregator Add(int path, bool isVisited);
        int PathsToCover();
        int CoveredPaths();
    }
}