using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    [ExcludeFromCodeCoverage]
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

        public void DirectoryDelete(string path,bool recursive)
        {
            Directory.Delete(path,recursive);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }
    }
}