using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IModel
    {
        void AddFile(string fileId, string filePath);
        IList<IFileCoverageModel> GetCoverage();
        void AddSequencePoint(string fileId, string sourceLine, string visitedCount);
        void AddBranchPoint(string fileId, string path,string sourceLine, string visitedCount);
    }
}