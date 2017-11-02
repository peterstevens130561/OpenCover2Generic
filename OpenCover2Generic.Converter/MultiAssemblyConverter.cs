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
        private static readonly ILog log = LogManager.GetLogger(typeof(MultiAssemblyConverter));
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

        public MultiAssemblyConverter() : this(new Model(), new OpenCoverCoverageParser(),
    new GenericCoverageWriter(),
    new OpenCoverCoverageParser(),
    new OpenCoverCoverageWriter())
        {

        }

        
        public void Convert(StreamWriter writer, StreamReader reader)
        {
            string rootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string testAssemblyName = "bogus";
            ConvertCoverageFileIntoIntermediate(rootPath, testAssemblyName, reader);
            Directory.CreateDirectory(rootPath);

            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {


                var moduleDirectories = Directory.EnumerateDirectories(rootPath,"*",SearchOption.TopDirectoryOnly);
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

        public void ConvertCoverageFileIntoIntermediate(string rootPath,string testAssemblyName,StreamReader reader)
        {
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                xmlReader.MoveToContent();
                while (_parser.ParseModule(_model, xmlReader))
                {
                    WriteModule(rootPath, testAssemblyName);
                };
                WriteModule(rootPath, testAssemblyName);
            }
        }

        private void WriteModule(string rootPath, string testAssemblyPath)
        {
            log.Debug($"WriteModule {rootPath},{testAssemblyPath}");
            if (_model.GetCoverage().Count > 0)
            {
                string moduleFile = GetPathForModule(rootPath,testAssemblyPath);
                WriteModuleToFile(moduleFile);
                _model.Clear();
            }
        }

        private string GetPathForModule(string rootPath, string testAssemblyPath)
        {
            string moduleDirectoryPath = Path.Combine(rootPath, _parser.ModuleName);
            if (!Directory.Exists(moduleDirectoryPath))
            {
                Directory.CreateDirectory(moduleDirectoryPath);
            }
            string moduleFile = Path.Combine(moduleDirectoryPath, Path.GetFileNameWithoutExtension(testAssemblyPath) + ".xml");
            return moduleFile;
        }

        public void BeginModule()
        {
            _model.Clear();
        }

        public void ReadIntermediateFile(string assemblyPath)
        {
            using (XmlReader tempFileReader = XmlReader.Create(assemblyPath))
            {
                tempFileReader.MoveToContent();
                // intentionally
                while (_moduleParser.ParseModule(_model, tempFileReader))
                {
                    // first one is empty. Really a bug
                }
            }
        }

        public void BeginCoverageFile(XmlTextWriter writer) {
            _coverageWriter.WriteBegin(writer);
        }
        public void AppendModuleToCoverageFile(XmlWriter writer)
        {
            _coverageWriter.GenerateCoverage(_model, writer);
        }

        public void EndCoverageFile(XmlWriter writer)
        {
            _coverageWriter.WriteEnd(writer);
        }
        private void WriteModuleToFile(string moduleFile)
        {
            log.Debug($"WriteModuleToFile({moduleFile})");
            using (XmlTextWriter tempFileWriter = new XmlTextWriter(moduleFile, Encoding.UTF8))
            {
                //write it to the temp file
                _moduleWriter.WriteBegin(tempFileWriter);
                _moduleWriter.GenerateCoverage(_model, tempFileWriter);
                _moduleWriter.WriteEnd(tempFileWriter);
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
