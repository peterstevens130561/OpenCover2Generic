using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public interface IXmlAdapter
    {
        XmlReader CreateReader(string path);

        /// <summary>
        /// Creates an XmlTextWriter with UTF8 encoding
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        XmlTextWriter CreateTextWriter(string path);
    }
}