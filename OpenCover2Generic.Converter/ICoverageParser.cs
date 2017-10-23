using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICoverageParser
    {
        string ModuleName { get; }

        bool ParseModule(IModel model, XmlReader xmlReader);
    }
}