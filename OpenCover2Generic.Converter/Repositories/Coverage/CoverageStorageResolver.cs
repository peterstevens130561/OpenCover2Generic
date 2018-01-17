using System;
using System.Collections.Generic;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class CoverageStorageResolver : ICoverageStorageResolver
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public CoverageStorageResolver() : this(new FileSystemAdapter())
        {
            
        }
        public CoverageStorageResolver(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        public string Root { get; set; }


        public string GetDirectory()
        {
            if (Root == null)
            {
                throw new InvalidOperationException(@"Property 'Root' not set");
            }
            string path = Path.GetFullPath(Path.Combine(Root, "OpenCoverIntermediate"));
            _fileSystemAdapter.CreateDirectory(path);
            return path;
        }

        public string GetPathForAssembly(string rootPath, string moduleName,string testAssemblyPath)
        {
            string moduleDirectoryPath = Path.Combine(rootPath, moduleName);
            if (!_fileSystemAdapter.DirectoryExists(moduleDirectoryPath))
            {
                _fileSystemAdapter.CreateDirectory(moduleDirectoryPath);
            }
            string moduleFile = Path.Combine(moduleDirectoryPath, Path.GetFileNameWithoutExtension(testAssemblyPath) + ".xml");
            return moduleFile;
        }

        public IEnumerable<string> GetPathsOfAllModules(string rootPath)
        {
            return _fileSystemAdapter.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory)
        {
            return _fileSystemAdapter.EnumerateFiles(moduleDirectory);
        }

    }
}
