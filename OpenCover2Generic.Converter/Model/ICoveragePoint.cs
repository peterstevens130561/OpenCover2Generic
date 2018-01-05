namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ISequencePointEntity
    {
        int SourceLine { get; }
        bool Covered { get; }

        void AddVisit(bool isVisited);
    }
}