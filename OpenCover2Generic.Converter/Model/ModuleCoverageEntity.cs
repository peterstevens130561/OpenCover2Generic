using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class ModuleCoverageEntity : IModuleCoverageEntity
    {
        private Dictionary<string, ISourceFileCoverageModel> _sourceFiles = new Dictionary<string,ISourceFileCoverageModel>();

        public string Name { get; set; }

        public void AddFile(string fileId, string filePath)
        {
            _sourceFiles.Add(fileId,new SourceFileCoverageModel(fileId,filePath));
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            _sourceFiles[fileId].AddSequencePoint(sourceLine, visitedCount);
        }

        public IList<ISourceFileCoverageModel> GetSourceFiles()
        {
            return _sourceFiles.Values.ToList();
        }


        public void Clear()
        {
            _sourceFiles = new Dictionary<string, ISourceFileCoverageModel>();
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            var branchPoint = new BranchPointValue(fileId, sourceLine, path, isVisited);
            _sourceFiles[fileId.ToString()].AddBranchPoint(branchPoint);
        }
    }
}
