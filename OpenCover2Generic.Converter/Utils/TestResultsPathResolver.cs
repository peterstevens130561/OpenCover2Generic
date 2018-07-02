using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public class TestResultsPathResolver : ITestResultsPathResolver
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public TestResultsPathResolver() : this(new FileSystemAdapter())
        {
            
        }
        public TestResultsPathResolver(IFileSystemAdapter fileSystemAdapter)
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

        public IEnumerable<string> GetTestResultsFiles()
        {
            return _fileSystemAdapter.EnumerateFiles(GetDirectory());
        }
    }
}
