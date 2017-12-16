using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{
    public interface ITestResultsRepository
    {
        void Write(StreamWriter streamWriter);
        void Add(string v);
    }
}