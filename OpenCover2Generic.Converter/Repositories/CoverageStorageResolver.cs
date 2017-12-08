using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCover2Generic.Converter;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    class CoverageStorageResolver : ICoverageStorageResolver
    {
        private readonly IFileSystemAdapter _fileSystem;
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
    }
}
