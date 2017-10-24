

using System;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class OpenCoverCoverageWriter : ICoverageWriter
    {
        public void GenerateCoverage(IModel model, XmlWriter xmlWriter)
        {
            if(model.GetCoverage().Count ==0)
            {
                return;
            }
            xmlWriter.WriteStartElement("Modules");
            xmlWriter.WriteStartElement("Module");
            WriteFilesElement(model, xmlWriter);
            WriteSequencePoints(model, xmlWriter);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        private void WriteSequencePoints(IModel model, XmlWriter xmlWriter)
        {
            foreach(ISourceFileCoverageModel sourceFile in model.GetCoverage())
            {
                foreach(ISequencePoint sequencePoint in sourceFile.SequencePoints)
                {
                    xmlWriter.WriteStartElement("SequencePoint");
                    string visited = sequencePoint.Covered ? "1" : "0";
                    xmlWriter.WriteAttributeString("vc", visited);
                    xmlWriter.WriteAttributeString("sl", sequencePoint.SourceLine.ToString());
                    xmlWriter.WriteAttributeString("fileid", sourceFile.Uid);
                    xmlWriter.WriteEndElement();
                    var aggregator = sourceFile.GetBranchPointAggregatorByLine(sequencePoint.SourceLine.ToString());
                    if(aggregator!=null)
                    {

                    }
                }
            }
        }

        private static void WriteFilesElement(IModel model, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Files");
            foreach (ISourceFileCoverageModel fileCoverage in model.GetCoverage())
            {
                xmlWriter.WriteStartElement("File");
                xmlWriter.WriteAttributeString("uid", fileCoverage.Uid);
                xmlWriter.WriteAttributeString("fullPath", fileCoverage.FullPath);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }


        public void WriteBegin(XmlTextWriter xmlWriter)
        {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteStartDocument();
            //xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"
            xmlWriter.WriteStartElement("CoverageSession");
            xmlWriter.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        }

        public void WriteEnd(XmlWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
        }
    }
}
