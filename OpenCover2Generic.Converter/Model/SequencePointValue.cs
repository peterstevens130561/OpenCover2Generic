namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Entity holding information on the coverage of a line
    /// </summary>
    public class SequencePointValue : ISequencePoint
    {
        public SequencePointValue(string sourceLine)
        {
            SourceLine = int.Parse(sourceLine);
        }

        public void AddVisit(bool isVisited)
        {
            Covered |= isVisited;
        }
        public int SourceLine { get; }
        public bool Covered { get; private set; }
    }
}