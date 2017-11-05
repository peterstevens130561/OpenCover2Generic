using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCover2Generic.Converter
{
    public class JobFileSystem
    {
        private readonly Object _lock = new object();
        private readonly Dictionary<string, int> lookupTable = new Dictionary<string, int>();
        private string _openCoverOutputDir;
        private string _testResultsDir;
        private string _openCoverIntermediateDir;
        private string _openCoverLogDir;
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public JobFileSystem(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        /// <summary>
        /// Creates the root structure for the temporary files
        /// </summary>
        public void CreateRoot(string key)
        {
            string rootPath = Path.GetFullPath(Path.Combine(_fileSystemAdapter.GetTempPath(), "opencover_" + key));
            _fileSystemAdapter.CreateDirectory(rootPath);

            _openCoverOutputDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverOutput"));
            _fileSystemAdapter.CreateDirectory(_openCoverOutputDir);

            _testResultsDir = Path.GetFullPath(Path.Combine(rootPath, "TestResults"));
            _fileSystemAdapter.CreateDirectory(_testResultsDir);

            _openCoverIntermediateDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverIntermediate"));
            _fileSystemAdapter.CreateDirectory(_openCoverIntermediateDir);

            _openCoverLogDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverLogs"));
            _fileSystemAdapter.CreateDirectory(_openCoverLogDir);
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
                if (!lookupTable.ContainsKey(key)) {
                    lookupTable.Add(key, lookupTable.Count + 1);
                }
                index = lookupTable[key].ToString();
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

        public string GetTestResultsPath(string assemblyPath)
        {
            return GetFileForAssembly(_testResultsDir, assemblyPath, "xml");
        }

        /// <summary>
        /// Gets the directory where all individual test results are stored.
        /// </summary>
        /// <returns></returns>
        public string GetTestResultsDirectory()
        {
            return _testResultsDir;
        }

        public IEnumerable<string> GetTestResultsPaths()
        {
            return _fileSystemAdapter.EnumerateFiles(_testResultsDir);
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