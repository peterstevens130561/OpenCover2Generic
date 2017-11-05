using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class ModuleCoverageModel : IModuleCoverageModel
    {
        private Dictionary<string, ISourceFileCoverageModel> sourceFiles = new Dictionary<string,ISourceFileCoverageModel>();

        public void AddFile(string fileId, string filePath)
        {
            sourceFiles.Add(fileId,new SourceFileCoverageModel(fileId,filePath));
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            sourceFiles[fileId].AddSequencePoint(sourceLine, visitedCount);
        }

        public IList<ISourceFileCoverageModel> GetCoverage()
        {
            return sourceFiles.Values.ToList();
        }

        public void AddBranchPoint(IBranchPoint branchPoint)
        {
            string fileId = branchPoint.FileId.ToString();
            sourceFiles[fileId].AddBranchPoint( branchPoint);
        }

        public void Clear()
        {
            sourceFiles = new Dictionary<string, ISourceFileCoverageModel>();
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            var branchPoint = new BranchPoint(fileId, sourceLine, path, isVisited);
            sourceFiles[fileId.ToString()].AddBranchPoint(branchPoint);
        }
    }
}
