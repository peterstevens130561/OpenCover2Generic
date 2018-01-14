using System.Collections.Generic;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CoverageStorageResolver : ICoverageStorageResolver
    {
        private readonly IFileSystemAdapter _fileSystem;

        public CoverageStorageResolver() : this(new FileSystemAdapter())
        {
            
        }
        public CoverageStorageResolver(IFileSystemAdapter fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public string GetPathForAssembly(string rootPath, string moduleName,string testAssemblyPath)
        {
            string moduleDirectoryPath = Path.Combine(rootPath, moduleName);
            if (!_fileSystem.DirectoryExists(moduleDirectoryPath))
            {
                _fileSystem.CreateDirectory(moduleDirectoryPath);
            }
            string moduleFile = Path.Combine(moduleDirectoryPath, Path.GetFileNameWithoutExtension(testAssemblyPath) + ".xml");
            return moduleFile;
        }

        public IEnumerable<string> GetPathsOfAllModules(string rootPath)
        {
            return _fileSystem.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory)
        {
            return _fileSystem.EnumerateFiles(moduleDirectory);
        }

    }
}
