using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public interface ICoverageParser
    {

        bool ParseModule(IModule entity, XmlReader xmlReader);
        void ParseFile(AggregatedModule entity, string coveragePath);
    }
}