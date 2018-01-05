namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ISequencePointEntity
    {
        int SourceLineId { get; }
        bool Covered { get; }

        void AddVisit(bool isVisited);
    }
}