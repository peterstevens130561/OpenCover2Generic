using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    interface ITestResultsPathResolver
    {
    }

    class TestResultsPathResolver : ITestResultsPathResolver
    {
        private IFileSystemAdapter _fileSystemAdapter;

        public TestResultsPathResolver(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }
    }
}
