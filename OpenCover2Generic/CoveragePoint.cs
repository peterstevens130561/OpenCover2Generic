namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class CoveragePoint : ICoveragePoint
    {
        private readonly int sourceLine;
        private readonly string visitedCount;

        public CoveragePoint(string sourceLine, string visitedCount)
        {
            this.sourceLine = int.Parse(sourceLine);
            this.visitedCount = visitedCount;
        }

        public int SourceLine { get { return sourceLine; } }
        public bool Covered { get { return int.Parse(visitedCount) > 0; } }
            
    
    }
}