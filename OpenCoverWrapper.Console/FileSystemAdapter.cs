using System;
using OpenCover2Generic.Converter;
using System.IO;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal class FileSystemAdapter : IFileSystemAdapter
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string GetTempPath()
        {
            return Path.GetTempPath();
        }


    }
}