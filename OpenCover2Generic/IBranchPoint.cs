namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IBranchPoint
    {
        int Paths { get; }
        int PathsVisited { get; }
        IBranchPoint Add(IBranchPoint branchPoint);
    }
}