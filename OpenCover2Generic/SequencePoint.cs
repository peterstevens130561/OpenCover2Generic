namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class SequencePoint : ISequencePoint
    {
        private readonly int sourceLine;
        private bool isVisited;

        public SequencePoint(string sourceLine)
        {
            this.sourceLine = int.Parse(sourceLine);
        }

        public void AddVisit(bool isVisited)
        {
            this.isVisited |= isVisited;
        }
        public int SourceLine { get { return sourceLine; } }
        public bool Covered { get { return isVisited; } }
            
    
    }
}