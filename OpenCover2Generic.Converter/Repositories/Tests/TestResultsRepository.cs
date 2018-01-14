using System.IO;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{
    public class TestResultsRepository : ITestResultsRepository
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IJobFileSystem _jobFileSystem;

        public TestResultsRepository(IJobFileSystem jobFileSystem,IFileSystemAdapter fileSystemAdapter)
        {
            _jobFileSystem = jobFileSystem;
            _fileSystemAdapter = fileSystemAdapter;
        }

        public void Add(string path)
        {
            string name = Path.GetFileName(path);
            string destinationFilePath = Path.Combine(_jobFileSystem.GetTestResultsDirectory(), name);
            _fileSystemAdapter.CopyFile(path,destinationFilePath);
        }

        public void Write(StreamWriter streamWriter)
        {
            var testResultsConcatenator = new TestResultsConcatenator();
            using (var writer = new XmlTextWriter(streamWriter))
            {
                testResultsConcatenator.Writer = writer;
                testResultsConcatenator.Begin();
                var files = _jobFileSystem.GetTestResultsFiles();
                foreach (var file in files)
                {
                    using (var reader = XmlReader.Create(file))
                    {
                        testResultsConcatenator.Concatenate(reader);
                    }

                }
                testResultsConcatenator.End();
            }
        }
    }
}
