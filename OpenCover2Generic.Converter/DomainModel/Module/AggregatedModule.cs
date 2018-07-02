using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module
{
    /// <summary>
    /// Used to create a entity from multiple coverage files. Though they pertain to the same module, each one may have different
    /// numbering for the files. Therefore,  there is small intermediate step
    /// </summary>
    public class AggregatedModule : IModule
    {
        private readonly IModule _module = new Module();
        private readonly Dictionary<string, string> _sourceFilePathToGlobalId = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _localFileIdToGlobalFileId = new Dictionary<string, string>();

        public string NameId
        {
            get
            {
                return _module.NameId;
            }

            set
            {
                _module.NameId = value;
            }
        }

        public void AddFile(string fileId, string filePath)
        {
            if(!_sourceFilePathToGlobalId.ContainsKey(filePath))
            {
                _sourceFilePathToGlobalId[filePath] = fileId;
                _module.AddFile(fileId, filePath);
            }
            string globalFileId = _sourceFilePathToGlobalId[filePath];
            _localFileIdToGlobalFileId[fileId] = globalFileId;
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            string globalFileId = _localFileIdToGlobalFileId[fileId];
            _module.AddSequencePoint(globalFileId, sourceLine, visitedCount);
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            string globalFileId = _localFileIdToGlobalFileId[fileId.ToString()];
            _module.AddBranchPoint(int.Parse(globalFileId),sourceLine,path,isVisited);
        }

        public void Clear()
        {
            _sourceFilePathToGlobalId.Clear();
            _localFileIdToGlobalFileId.Clear();
            _module.Clear();
        }

        public IList<ISourceFile> GetSourceFiles()
        {
            return _module.GetSourceFiles();
        }
    }
}
