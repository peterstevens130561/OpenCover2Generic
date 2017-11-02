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

        public void AddBranchPoint(int fileId, int sourceLine, int path, bool isVisited)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string fileId, string filePath)
        {
            throw new NotImplementedException();
        }

        public void AddSequencePoint(string fileId, string sourceLine, string visitedCount)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IList<ISourceFileCoverageModel> GetCoverage()
        {
            throw new NotImplementedException();
        }
    }
}
