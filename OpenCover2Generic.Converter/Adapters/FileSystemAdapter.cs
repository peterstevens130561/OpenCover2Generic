using System.Collections.Generic;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public class FileSystemAdapter : IFileSystemAdapter
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        public void CopyFile(string sourceFileName,string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        public string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }
    }
}