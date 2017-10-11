using System;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class CoveragePoint : ICoveragePoint
    {
        private readonly int sourceLine;
        private  bool covered;

        public CoveragePoint(string sourceLine, bool visited)
        {
            this.sourceLine = int.Parse(sourceLine);
            this.covered = visited ;
        }

        public int SourceLine { get { return sourceLine; } }
        public bool Covered { get { return covered; } }

        public void Add(bool visited)
        {
            this.covered = covered || visited;
        }
    }
}