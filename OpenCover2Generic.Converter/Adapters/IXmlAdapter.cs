using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public interface IXmlAdapter
    {
        XmlReader CreateReader(string path);

        XmlTextWriter CreateTextWriter(string path);
    }
}