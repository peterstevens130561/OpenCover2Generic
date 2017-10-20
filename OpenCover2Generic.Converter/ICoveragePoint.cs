namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ISequencePoint
    {
        int SourceLine { get; }
        bool Covered { get; }

        void AddVisit(bool isVisited);
    }
}