using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ISourceFileCoverageModel
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ISequencePoint> SequencePoints { get; }
        string Uid { get; }

        void AddBranchPoint(string sourceLine, IBranchPoint branchPoint);

        void AddBranchPoint(IBranchPoint branchPoint);
        IBranchPointAggregator GetBranchPointAggregatorByLine(string sourceLine);
    }
}