using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICodeCoverageRepository
    {
        string RootDirectory { get; set; }

        void Add(string path, string jobFirstAssembly);
        void CreateGenericCoverageFile(XmlTextWriter writer);

        void CreateGenericCoverageFile(string outputPath);
        ICodeCoverageRepositoryObservableScanner Scanner();
    }
}