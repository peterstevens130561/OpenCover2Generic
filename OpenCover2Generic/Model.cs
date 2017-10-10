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
        private Dictionary<string, IFileCoverageModel> sourceFiles;

        public Model()
        {
            Init();
        }
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

        public void AddBranchPoint(string fileId, string sourceLine, string visitedCount)
        {
            sourceFiles[fileId].AddBranchPoint(sourceLine, visitedCount);
        }

        public void Init()
        {
            sourceFiles = new Dictionary<string, IFileCoverageModel>();
        }
    }
}
