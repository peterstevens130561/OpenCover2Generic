using System.Collections.Generic;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    // Implements all required actions on the fileSystem. 
    public interface IFileSystemAdapter
    {
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        string GetTempPath();
        IEnumerable<string> EnumerateFiles(string path);
        void CopyFile(string path, string v);
        IEnumerable<string> EnumerateDirectories(
            string path,
            string searchPattern,
            SearchOption searchOption);
    }
}