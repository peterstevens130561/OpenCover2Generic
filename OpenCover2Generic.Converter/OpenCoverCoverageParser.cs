using System;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class OpenCoverCoverageParser : ICoverageParser
    { 
        private IModel _model;
        private string _moduleName;

        public string ModuleName
        {
            get
            {
                return _moduleName;
            }
        }

        public bool ParseModule(IModel model,XmlReader xmlReader)
        {
            _model = model;
            _moduleName = null;
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
                        case "ModuleName":
                            ReadModuleName(xmlReader);
                            break;
                        case "Module":
                            return true;
                        default:
                            break;
                    }
                }
            }
            return false;
        }

        private void ReadModuleName(XmlReader xmlReader)
        {
             _moduleName = xmlReader.Value;
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
            bool isVisited = int.Parse(xmlReader.GetAttribute("vc")) > 0;
            int fileId = int.Parse(xmlReader.GetAttribute("fileid"));
            int path = int.Parse(xmlReader.GetAttribute("path"));
            var branchPoint = new BranchPoint(fileId, sourceLine, path, isVisited);
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