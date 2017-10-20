using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IModel
    {
        void AddFile(string fileId, string filePath);
        IList<IFileCoverageModel> GetCoverage();
        void AddSequencePoint(string fileId, string sourceLine, string visitedCount);
        void AddBranchPoint( IBranchPoint branchPoint);
        void Clear();
    }
}