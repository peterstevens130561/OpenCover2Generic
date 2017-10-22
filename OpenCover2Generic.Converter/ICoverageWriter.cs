using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICoverageWriter
    {
        void WriteBegin(XmlTextWriter xmlWriter);
        void WriteEnd(XmlWriter xmlWriter);

        void GenerateCoverage(IModel model, XmlWriter xmlWriter);
    }
}