using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface IModuleCoverageEntity
    {
        string Name { get; set; }

        void AddFile(string fileId, string filePath);
        IList<ISourceFileCoverageModel> GetSourceFiles();
        void AddSequencePoint(string fileId, string sourceLine, string visitedCount);
        void Clear();
        void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited);
    }
}