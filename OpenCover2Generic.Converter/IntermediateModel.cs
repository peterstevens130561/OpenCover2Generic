using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    /// <summary>
    /// Used to create a model from multiple coverage files. Though they pertain to the same module, each one may have different
    /// numbering for the files. Therefore,  there is small intermediate step
    /// </summary>
    class IntermediateModel : IModel
    {
        private readonly IModel moduleModel = new Model();
        private Dictionary<string, string> sourceFilePathToGlobalId = new Dictionary<string, string>();
        private Dictionary<string, string> localFileIdToGlobalFileId = new Dictionary<string, string>();


        public void AddFile(string fileId, string filePath)
        {
            if(!sourceFilePathToGlobalId.ContainsKey(filePath))
            {
                sourceFilePathToGlobalId[filePath] = fileId;
                moduleModel.AddFile(fileId, filePath);
            }
            string globalFileId = sourceFilePathToGlobalId[filePath];
            localFileIdToGlobalFileId[fileId] = globalFileId;
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            string globalFileId = localFileIdToGlobalFileId[fileId];
            moduleModel.AddSequencePoint(globalFileId, sourceLine, visitedCount);
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            string globalFileId = localFileIdToGlobalFileId[fileId.ToString()];
            moduleModel.AddBranchPoint(int.Parse(globalFileId),sourceLine,path,isVisited);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IList<ISourceFileCoverageModel> GetCoverage()
        {
            return moduleModel.GetCoverage();
        }
    }
}
