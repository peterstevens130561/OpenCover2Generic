using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
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

        void GenerateCoverage(IModuleCoverageModel model, XmlWriter xmlWriter);
    }
}