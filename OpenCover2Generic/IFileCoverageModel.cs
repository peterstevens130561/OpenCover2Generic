using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IFileCoverageModel
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ICoveragePoint> SequencePoints { get; }

        void AddBranchPoint(string sourceLine, string path, string visitedCount);
        IBranchPoint BranchPoint(string sourceLine);
    }
}