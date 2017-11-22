using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OpenCover2Generic.Converter;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    public class TestResultsRepository : ITestResultsRepository
    {
        private readonly IJobFileSystem _jobFileSystem;

        public TestResultsRepository(IJobFileSystem jobFileSystem)
        {
            _jobFileSystem = jobFileSystem;
        }

        public void Add(string path)
        {
            throw new NotImplementedException();
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
