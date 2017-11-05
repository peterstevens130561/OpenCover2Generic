using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public interface ICoverageParser
    {
        string ModuleName { get; }

        bool ParseModule(IModuleCoverageModel model, XmlReader xmlReader);
    }
}