using System;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class FileCoverageModel : IFileCoverageModel
    {
        private string filePath;

        public FileCoverageModel(string filePath)
        {
            this.filePath = filePath;
        }

        public string FullPath
        {
            get
            {
                return filePath;
            }
        }
    }
}