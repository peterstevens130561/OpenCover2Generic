using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Used to create a model from multiple coverage files. Though they pertain to the same module, each one may have different
    /// numbering for the files. Therefore,  there is small intermediate step
    /// </summary>
    public class IntermediateModel : IModuleCoverageModel
    {
        private readonly IModuleCoverageModel moduleModel = new ModuleCoverageModel();
        private readonly Dictionary<string, string> sourceFilePathToGlobalId = new Dictionary<string, string>();
        private readonly Dictionary<string, string> localFileIdToGlobalFileId = new Dictionary<string, string>();


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
            sourceFilePathToGlobalId.Clear();
            localFileIdToGlobalFileId.Clear();
            moduleModel.Clear();
        }

        public IList<ISourceFileCoverageModel> GetSourceFiles()
        {
            return moduleModel.GetSourceFiles();
        }
    }
}
