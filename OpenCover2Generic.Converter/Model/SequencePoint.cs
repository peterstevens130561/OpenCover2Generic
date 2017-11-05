namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Entity holding information on the coverage of a line
    /// </summary>
    public class SequencePoint : ISequencePoint
    {
        private readonly int _sourceLine;
        private bool _isVisited;

        public SequencePoint(string sourceLine)
        {
            _sourceLine = int.Parse(sourceLine);
        }

        public void AddVisit(bool isVisited)
        {
            _isVisited |= isVisited;
        }
        public int SourceLine { get { return _sourceLine; } }
        public bool Covered { get { return _isVisited; } }
            
    
    }
}