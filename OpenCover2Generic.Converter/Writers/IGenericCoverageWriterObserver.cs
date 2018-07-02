using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface IGenericCoverageWriterObserver : IQueryAllModulesResultObserver
    {
        XmlTextWriter Writer { get; set; }
    }
}
