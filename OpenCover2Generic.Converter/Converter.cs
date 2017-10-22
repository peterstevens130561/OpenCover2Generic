﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class Converter : IConverter
    {
        private readonly IModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private Model model;
        private GenericCoverageWriter genericCoverageWriter;

        public Converter(IModel model,ICoverageWriter coverageWriter)
        {
            _model = model;
            _coverageWriter = coverageWriter;
        }

        public Converter(Model model, GenericCoverageWriter genericCoverageWriter)
        {
            this.model = model;
            this.genericCoverageWriter = genericCoverageWriter;
        }

        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                WriteBegin(xmlWriter);
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    ParseStream(xmlWriter, xmlReader);
                }

                WriteEnd(xmlWriter);
            }
        }

        private static void WriteEnd(XmlWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
        }

        private static void WriteBegin(XmlTextWriter xmlWriter)
        {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("coverage");
            xmlWriter.WriteAttributeString("version", "1");
        }

        private void ParseStream(XmlWriter xmlWriter, XmlReader xmlReader)
        {
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
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
                            GenerateCoverage(xmlWriter);
                            break;
                        default:
                            break;
                    }
                }
            }
            GenerateCoverage(xmlWriter);
        }

        private void AddFile(XmlReader xmlReader)
        {
            string fileId = xmlReader.GetAttribute("uid");
            string filePath = xmlReader.GetAttribute("fullPath");
            _model.AddFile(fileId, filePath);
        }

        private void AddBranchPoint(XmlReader xmlReader)
        {
            int sourceLine = int.Parse(xmlReader.GetAttribute("sl"));
            bool isVisited = int.Parse(xmlReader.GetAttribute("vc"))>0;
            int fileId = int.Parse(xmlReader.GetAttribute("fileid"));
            int path = int.Parse( xmlReader.GetAttribute("path"));
            var branchPoint = new BranchPoint(fileId,sourceLine, path, isVisited);
            _model.AddBranchPoint(branchPoint);
        }

        private void AddSequencePoint(XmlReader xmlReader)
        {
            string sourceLine = xmlReader.GetAttribute("sl");
            string visitedCount = xmlReader.GetAttribute("vc");
            string fileId = xmlReader.GetAttribute("fileid");
            _model.AddSequencePoint(fileId, sourceLine, visitedCount);
        }

        private void GenerateCoverage(XmlWriter xmlWriter)
        {
            foreach(IFileCoverageModel fileCoverage in _model.GetCoverage())
            {
                xmlWriter.WriteStartElement("file");
                xmlWriter.WriteAttributeString("path", fileCoverage.FullPath);
                GenerateSequencePoints(xmlWriter, fileCoverage);
            }
            _model.Clear();
        }

        private static void GenerateSequencePoints(XmlWriter xmlWriter, IFileCoverageModel fileCoverage)
        {
            foreach (ISequencePoint sequencePoint in fileCoverage.SequencePoints)
            {
                xmlWriter.WriteStartElement("lineToCover");
                string sourceLine = sequencePoint.SourceLine.ToString();
                xmlWriter.WriteAttributeString("lineNumber", sourceLine);
                xmlWriter.WriteAttributeString("covered", sequencePoint.Covered ? "true" : "false");
                IBranchPointAggregator branchPoint = fileCoverage.BranchPointAggregator(sourceLine);
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
