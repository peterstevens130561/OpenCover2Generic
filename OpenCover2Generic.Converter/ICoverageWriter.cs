using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICoverageWriter
    {
        void WriteBegin(XmlTextWriter xmlWriter);
    }
}