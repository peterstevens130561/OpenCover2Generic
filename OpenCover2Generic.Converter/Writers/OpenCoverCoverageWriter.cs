

using BHGE.SonarQube.OpenCover2Generic.Model;
using System;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class OpenCoverCoverageWriter : ICoverageWriter
    {
        private XmlWriter _xmlWriter;

        public void GenerateCoverage(IModuleCoverageModel model, XmlWriter xmlWriter)
        {
            _xmlWriter = xmlWriter;
            if(model.GetCoverage().Count ==0)
            {
                return;
            }
            xmlWriter.WriteStartElement("Modules");
            xmlWriter.WriteStartElement("Module");
            WriteSourceFiles(model);
            WriteCoverageData(model, xmlWriter);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        private void WriteCoverageData(IModuleCoverageModel model, XmlWriter xmlWriter)
        {
            _xmlWriter = xmlWriter;
            foreach(ISourceFileCoverageModel sourceFile in model.GetCoverage())
            {
                WriteCoverageDataForSourceFile(sourceFile);
            }
        }

        private void WriteCoverageDataForSourceFile( ISourceFileCoverageModel sourceFile)
        {
            foreach (ISequencePoint sequencePoint in sourceFile.SequencePoints)
            {
                string sourceLineNr = WriteSequencePoint(sourceFile, sequencePoint);
                WriteBranchPointsForLine(sourceFile, sourceLineNr);
            }
        }

        private string WriteSequencePoint(ISourceFileCoverageModel sourceFile, ISequencePoint sequencePoint)
        {
            _xmlWriter.WriteStartElement("SequencePoint");
            string sourceLineNr = sequencePoint.SourceLine.ToString();
            string visited = sequencePoint.Covered ? "1" : "0";
            _xmlWriter.WriteAttributeString("vc", visited);
            _xmlWriter.WriteAttributeString("sl", sourceLineNr);
            _xmlWriter.WriteAttributeString("fileid", sourceFile.Uid);
            _xmlWriter.WriteEndElement();
            return sourceLineNr;
        }

        private void WriteBranchPointsForLine( ISourceFileCoverageModel sourceFile, string sourceLineNr)
        {
            var aggregator = sourceFile.GetBranchPointsByLine(sourceLineNr);
            if (aggregator != null)
            {
                foreach (IBranchPoint branchPoint in aggregator.GetBranchPoints())

                {   // <BranchPoint vc=""0"" uspid=""3137"" ordinal=""11"" offset=""687"" sl=""27"" path=""0"" offsetend=""689"" fileid=""1"" />
                    _xmlWriter.WriteStartElement("BranchPoint");
                    _xmlWriter.WriteAttributeString("vc", branchPoint.IsVisited ? "1" : "0");
                    _xmlWriter.WriteAttributeString("sl", branchPoint.SourceLine.ToString());
                    _xmlWriter.WriteAttributeString("path", branchPoint.Path.ToString());
                    _xmlWriter.WriteAttributeString("fileid", sourceFile.Uid);
                    _xmlWriter.WriteEndElement();

                }
            }
        }

        private void WriteSourceFiles(IModuleCoverageModel model)
        {
            _xmlWriter.WriteStartElement("Files");
            foreach (ISourceFileCoverageModel fileCoverage in model.GetCoverage())
            {
                _xmlWriter.WriteStartElement("File");
                _xmlWriter.WriteAttributeString("uid", fileCoverage.Uid);
                _xmlWriter.WriteAttributeString("fullPath", fileCoverage.FullPath);
                _xmlWriter.WriteEndElement();
            }
            _xmlWriter.WriteEndElement();
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
