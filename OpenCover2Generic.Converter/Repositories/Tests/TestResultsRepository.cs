using System.IO;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{
    public class TestResultsRepository : ITestResultsRepository
    {
        private readonly IFileSystemAdapter _fileSystem;
        private readonly IJobFileSystem _jobFileSystem;

        public TestResultsRepository(IJobFileSystem jobFileSystem,IFileSystemAdapter fileSystem)
        {
            _jobFileSystem = jobFileSystem;
            _fileSystem = fileSystem;
        }

        public void Add(string path)
        {
            string name = Path.GetFileName(path);
            string destinationFilePath = Path.Combine(_jobFileSystem.GetTestResultsDirectory(), name);
            _fileSystem.CopyFile(path,destinationFilePath);
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
