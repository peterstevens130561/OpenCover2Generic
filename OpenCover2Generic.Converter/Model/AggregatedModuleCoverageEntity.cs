using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    /// <summary>
    /// Used to create a entity from multiple coverage files. Though they pertain to the same module, each one may have different
    /// numbering for the files. Therefore,  there is small intermediate step
    /// </summary>
    public class AggregatedModuleCoverageEntity : IModuleCoverageEntity
    {
        private readonly IModuleCoverageEntity _moduleEntity = new ModuleCoverageEntity();
        private readonly Dictionary<string, string> _sourceFilePathToGlobalId = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _localFileIdToGlobalFileId = new Dictionary<string, string>();

        public string NameId
        {
            get
            {
                return _moduleEntity.NameId;
            }

            set
            {
                _moduleEntity.NameId = value;
            }
        }

        public void AddFile(string fileId, string filePath)
        {
            if(!_sourceFilePathToGlobalId.ContainsKey(filePath))
            {
                _sourceFilePathToGlobalId[filePath] = fileId;
                _moduleEntity.AddFile(fileId, filePath);
            }
            string globalFileId = _sourceFilePathToGlobalId[filePath];
            _localFileIdToGlobalFileId[fileId] = globalFileId;
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            string globalFileId = _localFileIdToGlobalFileId[fileId];
            _moduleEntity.AddSequencePoint(globalFileId, sourceLine, visitedCount);
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            string globalFileId = _localFileIdToGlobalFileId[fileId.ToString()];
            _moduleEntity.AddBranchPoint(int.Parse(globalFileId),sourceLine,path,isVisited);
        }

        public void Clear()
        {
            _sourceFilePathToGlobalId.Clear();
            _localFileIdToGlobalFileId.Clear();
            _moduleEntity.Clear();
        }

        public IList<ISourceFileCoverageAggregate> GetSourceFiles()
        {
            return _moduleEntity.GetSourceFiles();
        }
    }
}
