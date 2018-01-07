using System.Collections.Generic;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module
{
    public class Module : IModule
    {
        private Dictionary<string, ISourceFile> _sourceFiles = new Dictionary<string,ISourceFile>();

        public string NameId { get; set; }

        public void AddFile(string fileId, string filePath)
        {
            _sourceFiles.Add(fileId,new SourceFile(fileId,filePath));
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            _sourceFiles[fileId].AddSequencePoint(sourceLine, visitedCount);
        }

        public IList<ISourceFile> GetSourceFiles()
        {
            return _sourceFiles.Values.ToList();
        }


        public void Clear()
        {
            _sourceFiles = new Dictionary<string, ISourceFile>();
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            var branchPoint = new BranchPoint(fileId, sourceLine, path, isVisited);
            _sourceFiles[fileId.ToString()].AddBranchPoint(branchPoint);
        }
    }
}
