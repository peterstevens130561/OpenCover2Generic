using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    public interface ITestResultsRepository
    {
        void Write(StreamWriter streamWriter);
        void Add(string v);
    }
}