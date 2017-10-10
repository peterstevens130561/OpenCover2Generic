namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPoint
    {
        int Path { get; }
        int VisitedCount { get; }
        IBranchPoint Add(IBranchPoint branchPoint);
    }
}