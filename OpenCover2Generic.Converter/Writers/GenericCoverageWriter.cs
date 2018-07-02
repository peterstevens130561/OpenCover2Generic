using BHGE.SonarQube.OpenCover2Generic.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line;

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

        public void GenerateCoverage(IModule entity,XmlWriter xmlWriter)
        {
            foreach (ISourceFile fileCoverage in entity.GetSourceFiles())
            {
                if (fileCoverage.FullPath.EndsWith(".cs"))
                {
                    xmlWriter.WriteStartElement("file");
                    xmlWriter.WriteAttributeString("path", fileCoverage.FullPath);
                    GenerateSequencePoints(xmlWriter, fileCoverage);
                }
            }
        }

        private  void GenerateSequencePoints(XmlWriter xmlWriter, ISourceFile file)
        { 
            foreach (ISequencePoint sequencePoint in file.SequencePoints)
            {
                xmlWriter.WriteStartElement("lineToCover");
                string sourceLine = sequencePoint.SourceLineId.ToString();
                xmlWriter.WriteAttributeString("lineNumber", sourceLine);
                xmlWriter.WriteAttributeString("covered", sequencePoint.Covered ? "true" : "false");
                IBranchPoints branchPoint = file.GetBranchPointsByLine(sourceLine);
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
