using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface ICoverageParser
    {
        bool ParseModule(IModel model, XmlReader xmlReader);
    }
}