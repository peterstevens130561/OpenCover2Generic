using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class GenericCoverageParserTests
    {

        private ICoverageParser _parser;
        private IModel _model;

        [TestMethod]
        public void ParseEmptyFile()
        {
            string simpleFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"">
    <file path=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"">
        <lineToCover lineNumber=""27"" covered=""false"" />
        <lineToCover lineNumber=""28"" covered=""true"" />
        <lineToCover lineNumber=""29"" covered=""true"" />
    </file>
</coverage>";
            _parser = new GenericCoverageParser();
            WhenConverting(simpleFile);
            Assert.AreEqual(3, _model.GetCoverage().Count,"expect 3 lines");
            var file = _model.GetCoverage().First(p => p.FullPath == @"E:\Cadence\EsieTooLinkRepositoryServiceTest.cs");
            Assert.IsNotNull(file);


        }

        private bool WhenConverting(string input)
        {
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            XmlReader reader = XmlReader.Create(inputStream);
            reader.MoveToContent();
            return _parser.ParseModule(_model, reader);
        }

    }
}
