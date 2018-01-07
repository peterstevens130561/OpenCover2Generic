namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line
{
    /// <summary>
    /// Entity holding information on the coverage of a line
    /// </summary>
    public class SequencePoint : ISequencePoint
    {
        public SequencePoint(string sourceLine)
        {
            SourceLineId = int.Parse(sourceLine);
        }

        public void AddVisit(bool isVisited)
        {
            Covered |= isVisited;
        }
        public int SourceLineId { get; }
        public bool Covered { get; private set; }
    }
}