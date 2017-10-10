using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IGenericBuilder
    {
        void Start(XmlTextWriter xmlWriter);
        void End(XmlTextWriter xmlWriter);
    }
}