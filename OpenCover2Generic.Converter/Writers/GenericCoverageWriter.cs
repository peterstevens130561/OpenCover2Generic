using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class GenericCoverageWriter : ICoverageWriter
    {


        public void WriteBegin(XmlTextWriter xmlWriter)
        {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("coverage");
            xmlWriter.WriteAttributeString("version", "1");
        }

        public  void WriteEnd(XmlWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
        }

        public void GenerateCoverage(IModuleCoverageEntity entity,XmlWriter xmlWriter)
        {
            foreach (ISourceFileCoverageAggregate fileCoverage in entity.GetSourceFiles())
            {
                xmlWriter.WriteStartElement("file");
                xmlWriter.WriteAttributeString("path", fileCoverage.FullPath);
                GenerateSequencePoints(xmlWriter, fileCoverage);
            }
        }

        private  void GenerateSequencePoints(XmlWriter xmlWriter, ISourceFileCoverageAggregate fileCoverage)
        { 
            foreach (ISequencePointEntity sequencePoint in fileCoverage.SequencePoints)
            {
                xmlWriter.WriteStartElement("lineToCover");
                string sourceLine = sequencePoint.SourceLineId.ToString();
                xmlWriter.WriteAttributeString("lineNumber", sourceLine);
                xmlWriter.WriteAttributeString("covered", sequencePoint.Covered ? "true" : "false");
                IBranchPoints branchPoint = fileCoverage.GetBranchPointsByLine(sourceLine);
                if (branchPoint != null)
                {
                    xmlWriter.WriteAttributeString("branchesToCover", branchPoint.PathsToCover().ToString());
                    xmlWriter.WriteAttributeString("coveredBranches", branchPoint.CoveredPaths().ToString());
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }


    }
}
