using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File
{
    public interface ISourceFile
    {
        string FullPath { get; }

        void AddSequencePoint(string sourceLine,string visitedCount);
        IList<ISequencePoint> SequencePoints { get; }
        string Uid { get; }

        void AddBranchPoint(IBranchPoint branchPoint);
        IBranchPoints GetBranchPointsByLine(string sourceLine);
    }
}