namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface ISequencePoint
    {
        int SourceLine { get; }
        bool Covered { get; }
    }
}