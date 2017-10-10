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
        private readonly IGenericBuilder genericBuilder;

        public Converter(IModel model,IGenericBuilder genericBuilder)
        {
            this.model = model;
            this.genericBuilder = genericBuilder;
        }
        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                genericBuilder.Start(xmlWriter);
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
                genericBuilder.End(xmlWriter);
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
            model.AddBranchPoint(fileId, sourceLine, visitedCount);
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
                GenerateSequencePoints(xmlWriter, fileCoverage);
            }
        }

        private  void GenerateSequencePoints(XmlWriter xmlWriter, IFileCoverageModel fileCoverage)
        {
            foreach (ICoveragePoint sequencePoint in fileCoverage.SequencePoints)
            {

                xmlWriter.WriteStartElement("lineToCover");
                GenerateLineAttributes(xmlWriter, fileCoverage, sequencePoint);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        private  void GenerateLineAttributes(XmlWriter xmlWriter, IFileCoverageModel fileCoverage, ICoveragePoint sequencePoint)
        {
            string sourceLine = sequencePoint.SourceLine.ToString();
            xmlWriter.WriteAttributeString("lineNumber", sourceLine.ToString());
            xmlWriter.WriteAttributeString("covered", sequencePoint.Covered ? "true" : "false");
            GenerateBranchCoverageAttribute(xmlWriter, fileCoverage, sourceLine);

        }

        private  void GenerateBranchCoverageAttribute(XmlWriter xmlWriter, IFileCoverageModel fileCoverage, string sourceLine)
        {
            IBranchPoint branchPoint = fileCoverage.BranchPoint(sourceLine);
            if (branchPoint != null)
            {
                xmlWriter.WriteAttributeString("branchesToCover", branchPoint.Paths.ToString());
                xmlWriter.WriteAttributeString("coveredBranches", branchPoint.PathsVisited.ToString());
            }
        }
    }
}
