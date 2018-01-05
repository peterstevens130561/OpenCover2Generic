using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class ModuleCoverageEntity : IModuleCoverageEntity
    {
        private Dictionary<string, ISourceFileCoverageAggregate> _sourceFiles = new Dictionary<string,ISourceFileCoverageAggregate>();

        public string NameId { get; set; }

        public void AddFile(string fileId, string filePath)
        {
            _sourceFiles.Add(fileId,new SourceFileCoverageAggregate(fileId,filePath));
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            _sourceFiles[fileId].AddSequencePoint(sourceLine, visitedCount);
        }

        public IList<ISourceFileCoverageAggregate> GetSourceFiles()
        {
            return _sourceFiles.Values.ToList();
        }


        public void Clear()
        {
            _sourceFiles = new Dictionary<string, ISourceFileCoverageAggregate>();
        }

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            var branchPoint = new BranchPointValue(fileId, sourceLine, path, isVisited);
            _sourceFiles[fileId.ToString()].AddBranchPoint(branchPoint);
        }
    }
}
