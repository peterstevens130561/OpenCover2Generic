﻿using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class OpenCoverCoverageWriter : ICoverageWriter
    {
        private XmlWriter _xmlWriter;

        public void GenerateCoverage(IModule entity, XmlWriter xmlWriter)
        {
            _xmlWriter = xmlWriter;
            if(entity.GetSourceFiles().Count ==0)
            {
                return;
            }
            xmlWriter.WriteStartElement("Modules");
            xmlWriter.WriteStartElement("Module");
            WriteSourceFiles(entity);
            WriteCoverageData(entity, xmlWriter);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        private void WriteCoverageData(IModule entity, XmlWriter xmlWriter)
        {
            _xmlWriter = xmlWriter;
            foreach(ISourceFile sourceFile in entity.GetSourceFiles())
            {
                WriteCoverageDataForSourceFile(sourceFile);
            }
        }

        private void WriteCoverageDataForSourceFile( ISourceFile sourceFile)
        {
            foreach (ISequencePoint sequencePoint in sourceFile.SequencePoints)
            {
                string sourceLineNr = WriteSequencePoint(sourceFile, sequencePoint);
                WriteBranchPointsForLine(sourceFile, sourceLineNr);
            }
        }

        private string WriteSequencePoint(ISourceFile sourceFile, ISequencePoint sequencePoint)
        {
            _xmlWriter.WriteStartElement("SequencePoint");
            string sourceLineNr = sequencePoint.SourceLineId.ToString();
            string visited = sequencePoint.Covered ? "1" : "0";
            _xmlWriter.WriteAttributeString("vc", visited);
            _xmlWriter.WriteAttributeString("sl", sourceLineNr);
            _xmlWriter.WriteAttributeString("fileid", sourceFile.Uid);
            _xmlWriter.WriteEndElement();
            return sourceLineNr;
        }

        private void WriteBranchPointsForLine( ISourceFile sourceFile, string sourceLineNr)
        {
            var aggregator = sourceFile.GetBranchPointsByLine(sourceLineNr);
            if (aggregator != null)
            {
                foreach (IBranchPoint branchPoint in aggregator.GetBranchPoints())

                {   // <BranchPointValue vc=""0"" uspid=""3137"" ordinal=""11"" offset=""687"" sl=""27"" path=""0"" offsetend=""689"" fileid=""1"" />
                    _xmlWriter.WriteStartElement("BranchPointValue");
                    _xmlWriter.WriteAttributeString("vc", branchPoint.IsVisited ? "1" : "0");
                    _xmlWriter.WriteAttributeString("sl", branchPoint.SourceLine.ToString());
                    _xmlWriter.WriteAttributeString("path", branchPoint.Path.ToString());
                    _xmlWriter.WriteAttributeString("fileid", sourceFile.Uid);
                    _xmlWriter.WriteEndElement();

                }
            }
        }

        private void WriteSourceFiles(IModule entity)
        {
            _xmlWriter.WriteStartElement("Files");
            foreach (ISourceFile fileCoverage in entity.GetSourceFiles())
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
