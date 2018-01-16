using System.IO;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{
    public interface ITestResultsRepository
    {
        void Write(StreamWriter streamWriter);
        void Add(string v);

        void SetWorkspace(IWorkspace workspace);
    }
}