using BHGE.SonarQube.OpenCover2Generic.Model;
using log4net;
using System;
using System.Net.Sockets;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public class OpenCoverCoverageParser : ICoverageParser
    {
        private IModuleCoverageModel _model;
        private string _moduleName;

        private enum ParserHuntState
        {
            None,
            Hunt,
            InModule,
        }


        public string ModuleName
        {
            get
            {
                return _moduleName;
            }
        }

        public void ParseFile(IntermediateModel model,string assemblyPath)
        {
            using (XmlReader tempFileReader = XmlReader.Create(assemblyPath))
            {
                tempFileReader.MoveToContent();
                while (ParseModule(model, tempFileReader)) ;
            }
        }

        public bool ParseModule(IModuleCoverageModel model,XmlReader xmlReader)
        {
            _model = model;
            _moduleName = null;
            ParserHuntState state = ParserHuntState.Hunt;

            while (xmlReader.Read())
            {
                switch (state)
                {
                    case ParserHuntState.Hunt:
                        if (AtStartOfNotSkippedModule(xmlReader))
                        {
                            state = ParserHuntState.InModule;
                        }
                        break;
                    case ParserHuntState.InModule:
                        ParseElementInModule(xmlReader);
                        if (AtEndElementOfModule(xmlReader))
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        private static bool AtStartOfNotSkippedModule(XmlReader xmlReader)
        {
            return xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Module" && xmlReader.GetAttribute("skippedDueTo") == null;
        }

        private static bool AtEndElementOfModule(XmlReader xmlReader)
        {
            return xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "Module";
        }

        private void ParseElementInModule(XmlReader xmlReader)
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
                    default:
                        // just ignore all of the other elements
                        break;
                }
            }
        }

        private void ReadModuleName(XmlReader xmlReader)
        {
            _moduleName = xmlReader.ReadElementContentAsString();
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
            _model.AddBranchPoint(fileId, sourceLine, path, isVisited);
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