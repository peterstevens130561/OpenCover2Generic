using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class MultiAssemblyConverter 
    {
        private readonly IModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private readonly ICoverageParser _parser;
        private readonly ICoverageParser _moduleParser;
        private readonly ICoverageWriter _moduleWriter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="testCoverageParser"></param>
        /// <param name="resultCoverageWriter"></param>
        public MultiAssemblyConverter(IModel model,
            ICoverageParser testCoverageParser,
            ICoverageWriter resultCoverageWriter,
            ICoverageParser intermediateCoverageParser,
            ICoverageWriter intermediateCoverageWriter)
        {
            _parser = testCoverageParser;
            _model = model;
            _coverageWriter = resultCoverageWriter;
            _moduleParser = intermediateCoverageParser;
            _moduleWriter = intermediateCoverageWriter;
        }

        public void Convert(StreamWriter writer, StreamReader reader)
        {
            string rootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(rootPath);
            string testAssemblyName = "bogus";
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {

                ConvertCoverageFileIntoIntermediate(rootPath, testAssemblyName, reader);
                var moduleDirectories = Directory.EnumerateDirectories(rootPath,"",SearchOption.TopDirectoryOnly);
                BeginCoverageFile(xmlWriter);
                _model.Clear();
                foreach (string moduleDirectory in moduleDirectories)
                {
                    foreach (string assemblyFile in Directory.EnumerateFiles(moduleDirectory))
                    {
                        ReadIntermediateFile(assemblyFile);
                    }
                    AppendModuleToCoverageFile(xmlWriter);
                }
                EndCoverageFile(xmlWriter);
            }
        }

        private void ConvertCoverageFileIntoIntermediate(string rootPath,string testAssemblyName,StreamReader reader)
        {
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                xmlReader.MoveToContent();
                while (_parser.ParseModule(_model, xmlReader))
                {
                    if (_model.GetCoverage().Count == 0)
                    {
                        continue;
                    }
                    string moduleFile = Path.Combine(rootPath, _parser.ModuleName, testAssemblyName);
                    WriteModuleToFile(moduleFile);
                    _model.Clear();
                };
            }
        }

        private void ReadIntermediateFile(string assemblyPath)
        {
            using (XmlReader tempFileReader = XmlReader.Create(assemblyPath))
            {
                tempFileReader.MoveToContent();
                _moduleParser.ParseModule(_model, tempFileReader);
            }
        }

        private void BeginCoverageFile(XmlTextWriter writer) {
            _coverageWriter.WriteBegin(writer);
        }
        private void AppendModuleToCoverageFile(XmlWriter writer)
        {
            _coverageWriter.GenerateCoverage(_model, writer);
        }

        private void EndCoverageFile(XmlWriter writer)
        {
            _coverageWriter.WriteEnd(writer);
        }
        private void WriteModuleToFile(string moduleFile)
        {
            using (XmlWriter tempFileWriter = new XmlTextWriter(moduleFile, Encoding.UTF8))
            {
                //write it to the temp file
                _moduleWriter.GenerateCoverage(_model, tempFileWriter);
            }

        }
        private void ReadModuleFromFile(string moduleFile)
        {
            using (XmlReader tempFileReader = XmlReader.Create(moduleFile))
            {
                tempFileReader.MoveToContent();
                _moduleParser.ParseModule(_model, tempFileReader);
            }
        }

    }
}
