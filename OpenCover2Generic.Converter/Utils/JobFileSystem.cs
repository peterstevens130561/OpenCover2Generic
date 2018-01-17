using System;
using System.Collections.Generic;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public class JobFileSystem : IJobFileSystem
    {
        private readonly Object _lock = new object();
        private readonly Dictionary<string, int> _assemblyUsageLookupTable = new Dictionary<string, int>();
        private string _openCoverOutputDir;
        private string _openCoverIntermediateDir;
        private string _openCoverLogDir;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private string _rootPath;

        public IWorkspace Workspace { get; set; }

        public JobFileSystem() : this(new FileSystemAdapter())
        {
            
        }
        public JobFileSystem(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        public void CreateRoot(string key)
        {
            string path= Path.GetFullPath(Path.Combine(_fileSystemAdapter.GetTempPath(), "opencover_" + key));
            var workspace = new Workspace(path);
            CreateRoot(workspace);
        }
        /// <summary>
        /// Creates the root structure for the temporary files
        /// </summary>
        public void CreateRoot(IWorkspace workspace)
        {
            _rootPath = workspace.Path;
            if (!_fileSystemAdapter.DirectoryExists(_rootPath))
            {
                _fileSystemAdapter.CreateDirectory(_rootPath);
            }

            _openCoverOutputDir = CreateChildDir("OpenCoverOutput");

            _openCoverIntermediateDir = CreateChildDir("OpenCoverIntermediate");
            _openCoverLogDir = CreateChildDir("OpenCoverLogs");

        }

        private string CreateChildDir(string name)
        {
            string path = Path.GetFullPath(Path.Combine(_rootPath, name));
            if (!_fileSystemAdapter.DirectoryExists(path))
            {
                _fileSystemAdapter.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// Gets a path for an opencover output file
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>

        public string GetOpenCoverOutputPath(string assemblyPath)
        {
            string index = GetIndex(assemblyPath);
            return Path.Combine(_openCoverOutputDir, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + ".xml");
        }
        private string GetIndex(string key)
        {
            string index;
            lock (_lock)
            {
                if (!_assemblyUsageLookupTable.ContainsKey(key)) {
                    _assemblyUsageLookupTable.Add(key, _assemblyUsageLookupTable.Count + 1);
                }
                index = _assemblyUsageLookupTable[key].ToString();
            }
            return index;
        }
        public string GetIntermediateCoverageOutputPath(string assemblyPath, string moduleName)
        {

            string moduleDirectory = Path.Combine(_openCoverIntermediateDir, moduleName);
            lock (_lock)
            {
                if (!_fileSystemAdapter.DirectoryExists(moduleDirectory))
                {
                    _fileSystemAdapter.CreateDirectory(moduleDirectory);
                }
            }
            return GetFileForAssembly(moduleDirectory, assemblyPath, "xml");
        }


        public void RemoveAll()
        {
            _fileSystemAdapter.DirectoryDelete(_rootPath,true);
        }


        public string GetOpenCoverLogPath(string assemblyPath)
        {
            return GetFileForAssembly(_openCoverLogDir, assemblyPath, "log");
        }

        private string GetFileForAssembly(string basePath,string assemblyPath, string extension)
        {
            string index = GetIndex(assemblyPath);
            return Path.Combine(basePath, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + "." + extension);
        }
        public string GetIntermediateCoverageDirectory()
        {
            return _openCoverIntermediateDir;
        }


    }

}