using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICoverageParser
    {
        string Module { get; }

        bool ParseModule(IModel model, XmlReader xmlReader);
    }
}