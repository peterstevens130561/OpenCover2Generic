using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IFileCoverageModel
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ISequencePoint> SequencePoints { get; }
        void AddBranchPoint(string sourceLine, IBranchPoint branchPoint);

        void AddBranchPoint(IBranchPoint branchPoint);
        IBranchPointAggregator BranchPointAggregator(string sourceLine);
    }
}