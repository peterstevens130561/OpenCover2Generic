using System.Text;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public class XmlAdapter : IXmlAdapter
    {
        public XmlReader CreateReader(string path)
        {
            return XmlReader.Create(path);
        }

        public XmlTextWriter CreateTextWriter(string path)
        {
            return new XmlTextWriter(path, Encoding.UTF8);
        }
    }
}
