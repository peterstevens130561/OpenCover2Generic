using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ISourceFileCoverageModel
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ISequencePointEntity> SequencePoints { get; }
        string Uid { get; }

        void AddBranchPoint(IBranchPoint branchPoint);
        IBranchPoints GetBranchPointsByLine(string sourceLine);
    }
}