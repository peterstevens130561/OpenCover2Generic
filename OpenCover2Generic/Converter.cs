using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class Converter : IConverter
    {
        private readonly IModel model;

        public Converter(IModel model)
        {
            this.model = model;
        }
        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("coverage");
                xmlWriter.WriteAttributeString("version", "1");
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            switch(xmlReader.Name)
                            {
                                case "File":
                                    AddFile(xmlReader);
                                    break;
                                case "SequencePoint":        
                                    AddSequencePoint(xmlReader);
                                    break;
                                case "BranchPoint":
                                    AddBranchPoint(xmlReader);
                                    break;
                                case "Module":
                                    GenerateCoverage(xmlWriter, model);
                                    break;
                            }
                        }
                    }
                }
                GenerateCoverage(xmlWriter, model);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }

        private void AddFile(XmlReader xmlReader)
        {
            string fileId = xmlReader.GetAttribute("uid");
            string filePath = xmlReader.GetAttribute("fullPath");
            model.AddFile(fileId, filePath);
        }

        private void AddBranchPoint(XmlReader xmlReader)
        {
            string fileId;
            string sourceLine = xmlReader.GetAttribute("sl");
            string visitedCount = xmlReader.GetAttribute("vc");
            fileId = xmlReader.GetAttribute("fileid");
            string path = xmlReader.GetAttribute("path");
            model.AddBranchPoint(fileId, sourceLine, visitedCount, path);
        }

        private void AddSequencePoint(XmlReader xmlReader)
        {
            string sourceLine = xmlReader.GetAttribute("sl");
            string visitedCount = xmlReader.GetAttribute("vc");
            string fileId = xmlReader.GetAttribute("fileid");
            model.AddSequencePoint(fileId, sourceLine, visitedCount);
        }

        private void GenerateCoverage(XmlWriter xmlWriter, IModel model)
        {
            foreach(IFileCoverageModel fileCoverage in model.GetCoverage())
            {
                xmlWriter.WriteStartElement("file");
                xmlWriter.WriteAttributeString("path", fileCoverage.FullPath);
                foreach(ICoveragePoint sequencePoint in fileCoverage.SequencePoints)
                {
                    xmlWriter.WriteStartElement("lineToCover");
                    string sourceLine = sequencePoint.SourceLine.ToString();
                    xmlWriter.WriteAttributeString("lineNumber", sourceLine.ToString());
                    xmlWriter.WriteAttributeString("covered", sequencePoint.Covered ? "true" : "false");
                    IBranchPoint branchPoint = fileCoverage.BranchPoint(sourceLine);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }
    }
}
