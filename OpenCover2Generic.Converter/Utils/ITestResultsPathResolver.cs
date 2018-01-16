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

        public string GetTestResultsDestinationPath(string v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetTestResultsPaths()
        {
            throw new NotImplementedException();
        }
    }
}
