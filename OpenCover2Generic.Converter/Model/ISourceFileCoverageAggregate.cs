using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ISourceFileCoverageAggregate
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ISequencePointEntity> SequencePoints { get; }
        string Uid { get; }

        void AddBranchPoint(IBranchPointValue branchPointValue);
        IBranchPoints GetBranchPointsByLine(string sourceLine);
    }
}