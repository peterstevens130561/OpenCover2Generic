

using System;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class GenericCoverageParser : ICoverageParser
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

        public bool ParseModule(IModel model, XmlReader xmlReader)
        {
            _model = model;
            _moduleName = null;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "file":
                            AddFile(xmlReader);
                            break;
                        case "lineToCover":
                            AddLine(xmlReader);
                            break;
                            return true;
                        default:
                            break;
                    }
                }
            }
            return false;
        }

        private void AddLine(XmlReader xmlReader)
        {
            throw new NotImplementedException();
        }

        private void AddFile(XmlReader xmlReader)
        {

            //_model.AddFile();
        }
    }
    }
