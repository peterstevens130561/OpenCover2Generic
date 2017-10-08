using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class Model : IModel
    {
        readonly IList<string> sourceFiles = new List<string>();

        public void AddFile(string fileId, string filePath)
        {
            sourceFiles.Add(filePath);
        }

        public IList<string> GetCoverage()
        {
            return sourceFiles;
        }
    }
}
