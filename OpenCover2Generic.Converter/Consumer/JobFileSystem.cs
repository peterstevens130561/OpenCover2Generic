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

        public JobFileSystem()
        {
        }

        /// <summary>
        /// Creates the root structure for the temporary files
        /// </summary>
        public void CreateRoot()
        {
            string key = DateTime.Now.ToString("yyMMdd_HHmmss");

            string rootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "opencover_" + key));
            Directory.CreateDirectory(rootPath);

            _openCoverOutputDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverOutput"));
            Directory.CreateDirectory(_openCoverOutputDir);

            _testResultsDir = Path.GetFullPath(Path.Combine(rootPath, "TestResults"));
            Directory.CreateDirectory(_testResultsDir);

            _openCoverIntermediateDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverIntermediate"));
            Directory.CreateDirectory(_openCoverIntermediateDir);

            _openCoverLogDir = Path.GetFullPath(Path.Combine(rootPath, "OpenCoverLogs"));
            Directory.CreateDirectory(_openCoverLogDir);
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
                if (!Directory.Exists(moduleDirectory))
                {
                    Directory.CreateDirectory(moduleDirectory);
                }
            }
            string index = GetIndex(assemblyPath);
            return Path.Combine(moduleDirectory, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + ".xml");
        }

        public string GetTestResultsPath(string assemblyPath)
        {
            string index = GetIndex(assemblyPath);
            return Path.Combine(_testResultsDir, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + "_results.xml");
        }

        /// <summary>
        /// Gets the directory where all individual test results are stored.
        /// </summary>
        /// <returns></returns>
        public string GetTestResultsDirectory()
        {
            return _testResultsDir;
        }

        public string GetOpenCoverLogPath(string assemblyPath)
        {
            string index = GetIndex(assemblyPath);
            return Path.Combine(_openCoverLogDir, index + "_" + Path.GetFileNameWithoutExtension(assemblyPath) + ".log");
        }

        public string GetIntermediateCoverageDirectory()
        {
            return _openCoverIntermediateDir;
        }
    }

}