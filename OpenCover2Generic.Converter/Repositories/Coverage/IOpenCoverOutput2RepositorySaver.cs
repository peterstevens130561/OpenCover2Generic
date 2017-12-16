using System.IO;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface IOpenCoverOutput2RepositorySaver
    {
        void ConvertCoverageFileIntoIntermediate(string rootPath, string testAssemblyName, StreamReader reader);
        void ReadIntermediateFile(string assemblyPath);
        void BeginCoverageFile(XmlTextWriter xmlWriter);
        void BeginModule();
        void EndCoverageFile(XmlWriter xmlWriter);
        void AppendModuleToCoverageFile(XmlWriter xmlWriter);
    }
}