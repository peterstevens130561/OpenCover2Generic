using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    public interface ICodeCoverageRepository
    {
        string RootDirectory { get; set; }

        void Add(string path, string jobFirstAssembly);
        void CreateCoverageFile(XmlTextWriter writer);

        void CreateCoverageFile(string outputPath);
    }
}