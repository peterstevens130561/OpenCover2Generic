using System;
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
        private GenericCoverageWriter genericCoverageWriter;

        public Converter(IModel model,ICoverageWriter coverageWriter)
        {
            _model = model;
            _coverageWriter = coverageWriter;
        }

        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                _coverageWriter.WriteBegin(xmlWriter);
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    ParseStream(xmlWriter, xmlReader);
                }

                _coverageWriter.WriteEnd(xmlWriter);
            }
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
                            _coverageWriter.GenerateCoverage(_model,xmlWriter);
                            _model.Clear();
                            break;
                        default:
                            break;
                    }
                }
            }
            _coverageWriter.GenerateCoverage(_model, xmlWriter);
            _model.Clear();
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

    }
}
