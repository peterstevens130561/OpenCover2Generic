using System;
using System.IO;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using log4net;
using OpenCover2Generic.Converter;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    public class CodeCoverageRepository : ICodeCoverageRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CodeCoverageRepository));

        public string RootDirectory { get; set; }

        public void Add(string path,string key)
        {
             ICoverageParser parser;

            try
            {
                using (var xmlReader = XmlReader.Create(path))
                {
                    xmlReader.MoveToContent();
                    IModuleCoverageModel model = new IntermediateModel();
                    while (parser.ParseModule(model, xmlReader))
                    {
                        WriteModule(RootDirectory, key);
                    }
                    WriteModule(RootDirectory, key);
                }

            }
            catch (Exception)
            {
                _log.Error($"Exception thrown during reading {path}");
                throw;
            }
        }

    }
}
