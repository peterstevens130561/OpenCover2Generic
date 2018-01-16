using System;
using System.IO;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{


    public class TestResultsRepository :   ITestResultsRepository
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly ITestResultsPathResolver _pathResolver;

        public TestResultsRepository() : this(new TestResultsPathResolver(), new FileSystemAdapter())
        {
            
        }

        public TestResultsRepository(ITestResultsPathResolver jobFileSystem) : this(jobFileSystem, new FileSystemAdapter())
        {
            
        }
        public TestResultsRepository(ITestResultsPathResolver jobFileSystem,IFileSystemAdapter fileSystemAdapter)
        {
            _pathResolver= jobFileSystem;
            _fileSystemAdapter = fileSystemAdapter;
        }

        public void SetWorkspace(IWorkspace workspace)
        {
            _pathResolver.Root = workspace.Path;
        }

        public void Add(string path)
        {
            string name = Path.GetFileName(path);
            string destinationFilePath = Path.Combine(_pathResolver.GetDirectory(), name);
            _fileSystemAdapter.CopyFile(path,destinationFilePath);
        }

        public void Write(StreamWriter streamWriter)
        {
            var testResultsConcatenator = new TestResultsConcatenator();
            using (var writer = new XmlTextWriter(streamWriter))
            {
                testResultsConcatenator.Writer = writer;
                testResultsConcatenator.Begin();
                var files = _pathResolver.GetTestResultsFiles();
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

        public void Write(string testResultsPath)
        {
            using (var writer = new StreamWriter(testResultsPath))
            {
                Write(writer);
            }
        }
    }
}
