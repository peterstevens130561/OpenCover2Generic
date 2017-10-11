using System;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class CoveragePoint : ICoveragePoint
    {
        private readonly int sourceLine;
        private  bool covered;

        public CoveragePoint(string sourceLine, string visitedCount)
        {
            this.sourceLine = int.Parse(sourceLine);
            this.covered = int.Parse(visitedCount) > 0;
        }

        public int SourceLine { get { return sourceLine; } }
        public bool Covered { get { return covered; } }

        public void Add(string visitedCount)
        {
            this.covered = covered || (int.Parse(visitedCount) > 0);
        }
    }
}