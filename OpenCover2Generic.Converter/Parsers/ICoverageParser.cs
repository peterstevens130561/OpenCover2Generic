using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public interface ICoverageParser
    {
        string ModuleName { get; }

        bool ParseModule(IModuleCoverageEntity entity, XmlReader xmlReader);
        void ParseFile(IntermediateEntity entity, string coveragePath);
    }
}