namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface ICoveragePoint
    {
        int SourceLine { get; }
        bool Covered { get; }
    }
}