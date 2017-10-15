using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class Model : IModel
    {
        readonly Dictionary<string, IFileCoverageModel> sourceFiles = new Dictionary<string,IFileCoverageModel>();

        public void AddFile(string fileId, string filePath)
        {
            sourceFiles.Add(fileId,new FileCoverageModel(filePath));
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            sourceFiles[fileId].AddSequencePoint(sourceLine, visitedCount);
        }

        public IList<IFileCoverageModel> GetCoverage()
        {
            return sourceFiles.Values.ToList();
        }

        public void AddBranchPoint(string fileId, IBranchPoint branchPoint)
        {
            sourceFiles[fileId].AddBranchPoint( branchPoint);
        }
    }
}
