using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface ICoverageWriter
    {
        /// <summary>
        /// This is where you create all of the header stuff + root element
        /// </summary>
        /// <param name="xmlWriter"></param>
        void WriteBegin(XmlTextWriter xmlWriter);

        /// <summary>
        /// Write the end element & flush
        /// </summary>
        /// <param name="xmlWriter"></param>
        void WriteEnd(XmlWriter xmlWriter);

        void GenerateCoverage(IModule entity, XmlWriter xmlWriter);
    }
}