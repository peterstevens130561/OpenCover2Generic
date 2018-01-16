using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    interface ITestResultsPathResolver
    {
        string Root { get; set; }

        string GetDirectory();
        string GetTestResultsDestinationPath(string v);

        IEnumerable<string> GetTestResultsPaths();
    }

    class TestResultsPathResolver : ITestResultsPathResolver
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public TestResultsPathResolver(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        public string Root { get; set; }

        public string GetDirectory()
        {
            string path = Path.GetFullPath(Path.Combine(Root, "TestResults"));
            _fileSystemAdapter.CreateDirectory(path);
            return path;
        }

        private readonly Dictionary<string, int> _assemblyUsageLookupTable = new Dictionary<string, int>();
        private readonly Object _lock = new object();
        private string GetIndex(string key)
        {
            string index;
            lock (_lock)
            {
                if (!_assemblyUsageLookupTable.ContainsKey(key))
                {
                    _assemblyUsageLookupTable.Add(key, _assemblyUsageLookupTable.Count + 1);
                }
                index = _assemblyUsageLookupTable[key].ToString();
            }
            return index;
        }

        private string GetFileForAssembly(string basePath, string assemblyPath, string extension)
        {
            string index = GetIndex(assemblyPath);
            return Path.Combine(basePath, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + "." + extension);
        }
        public string GetTestResultsDestinationPath(string assemblyPath)
        {
            return GetFileForAssembly(GetDirectory(), assemblyPath, "xml");
        }

        public IEnumerable<string> GetTestResultsPaths()
        {
            return _fileSystemAdapter.EnumerateFiles(GetDirectory());
        }
    }
}
