using System.Collections.Generic;

namespace OpenCover2Generic.Converter
{
    // Implements all required actions on the fileSystem. 
    public interface IFileSystemAdapter
    {
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        string GetTempPath();
        IEnumerable<string> EnumerateFiles(string path);
        void CopyFile(string path, string v);
    }
}