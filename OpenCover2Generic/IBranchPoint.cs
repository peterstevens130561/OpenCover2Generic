namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPoint
    {
        int Paths { get; }
        int VisitedCount { get; }
        IBranchPoint Add(IBranchPoint branchPoint);
    }
}