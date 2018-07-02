using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public class OpenCoverCoverageParser : ICoverageParser
    {
        private IModule _module;

        private enum ParserHuntState
        {
            None,
            Hunt,
            InModule,
        }



        public void ParseFile(AggregatedModule entity,string assemblyPath)
        {
            using (XmlReader tempFileReader = XmlReader.Create(assemblyPath))
            {
                tempFileReader.MoveToContent();
                while (ParseModule(entity, tempFileReader)) ;
            }
        }

        public bool ParseModule(IModule entity,XmlReader xmlReader)
        {
            _module = entity;
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
                    case "BranchPointValue":
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
            string name = xmlReader.ReadElementContentAsString();
            _module.NameId = name;
        }

        private void AddFile(XmlReader xmlReader)
        {
            string fileId = xmlReader.GetAttribute("uid");
            string filePath = xmlReader.GetAttribute("fullPath");
            _module.AddFile(fileId, filePath);
        }

        private void AddBranchPoint(XmlReader xmlReader)
        {
            int sourceLine = int.Parse(xmlReader.GetAttribute("sl"));
            bool isVisited = int.Parse(xmlReader.GetAttribute("vc")) > 0;
            int fileId = int.Parse(xmlReader.GetAttribute("fileid"));
            int path = int.Parse(xmlReader.GetAttribute("path"));
            _module.AddBranchPoint(fileId, sourceLine, path, isVisited);
        }

        private void AddSequencePoint(XmlReader xmlReader)
        {
            string sourceLine = xmlReader.GetAttribute("sl");
            string visitedCount = xmlReader.GetAttribute("vc");
            string fileId = xmlReader.GetAttribute("fileid");
            _module.AddSequencePoint(fileId, sourceLine, visitedCount);
        }


    }
}