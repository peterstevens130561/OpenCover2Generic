namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Entity holding information on the coverage of a line
    /// </summary>
    public class SequencePointEntityEntity : ISequencePointEntity
    {
        public SequencePointEntityEntity(string sourceLine)
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