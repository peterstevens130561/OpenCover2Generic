using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IModel
    {
        void AddFile(string fileId, string filePath);
        IList<string> GetCoverage();
    }
}