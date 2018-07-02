using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module
{
    public interface IModule
    {
        string NameId { get; set; }

        void AddFile(string fileId, string filePath);
        IList<ISourceFile> GetSourceFiles();
        void AddSequencePoint(string fileId, string sourceLine, string visitedCount);
        void Clear();
        void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited);
    }
}