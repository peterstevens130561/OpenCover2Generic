using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface IGenericCoverageWriterObserver
    {
        XmlTextWriter Writer { get; set; }
    }
}
