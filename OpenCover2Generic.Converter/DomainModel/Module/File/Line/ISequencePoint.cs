namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
{
    public interface ISequencePoint
    {
        int SourceLineId { get; }
        bool Covered { get; }

        void AddVisit(bool isVisited);
    }
}