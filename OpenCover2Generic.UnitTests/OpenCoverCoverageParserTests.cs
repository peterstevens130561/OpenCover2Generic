using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Xml;
using Moq;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverCoverageParserTests
    {
        private IModuleCoverageModel _model;
        private OpenCoverCoverageParser _parser;
        private Mock<IModuleCoverageModel> _modelMock;
        private readonly string openCoverExample= @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module hash=""AF-9C-7F-A4-DD-C2-F3-98-52-99-5F-75-22-1D-C1-5F-2A-5D-BE-62"">
      <ModuleName>Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest</ModuleName>
      <Files>
        <File uid=""1"" fullPath=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"" />
      </Files>
      <Classes>
        <Class>
          <Summary numSequencePoints=""0"" visitedSequencePoints=""0"" numBranchPoints=""0"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""0"" minCyclomaticComplexity=""0"" visitedClasses=""0"" numClasses=""0"" visitedMethods=""0"" numMethods=""0"" />
          <FullName>&lt;Module&gt;</FullName>
          <Methods />
        </Class>
        <Class>
          <Summary numSequencePoints=""36"" visitedSequencePoints=""0"" numBranchPoints=""9"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""1"" visitedMethods=""0"" numMethods=""9"" />
          <FullName>Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.EsieTooLinkRepositoryServiceTest</FullName>
          <Methods>
            <Method visited=""false"" cyclomaticComplexity=""1"" nPathComplexity=""0"" sequenceCoverage=""0"" branchCoverage=""0"" isConstructor=""false"" isStatic=""false"" isGetter=""true"" isSetter=""false"">
              <Summary numSequencePoints=""3"" visitedSequencePoints=""0"" numBranchPoints=""1"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""1"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""0"" visitedMethods=""0"" numMethods=""1"" />
              <MetadataToken>100663297</MetadataToken>
              <Name>Bhi.Esie.Services.EsieTooLinkRepository.Interfaces.IEsieTooLinkRepositoryService Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.EsieTooLinkRepositoryServiceTest::get_EsieTooLinkRepository()</Name>
              <FileRef uid=""1"" />
              <SequencePoints>
                <SequencePoint vc=""2"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
              </SequencePoints>
              <BranchPoints>
                <BranchPoint vc=""3"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";


        [TestInitialize]
        public void Initialize()
        {
            _modelMock = new Mock<IModuleCoverageModel>();
            _model = _modelMock.Object;
            _parser = new OpenCoverCoverageParser();

        }

        [TestMethod]
        public void parseFile()
        {

            WhenParsing(openCoverExample);
            _modelMock.Verify(parser => parser.AddFile("1", @"E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"));
        }

        [TestMethod]
        public void parseModuleName()
        {

            WhenParsing(openCoverExample);
            Assert.AreEqual("Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest", _parser.ModuleName);
        }

        [TestMethod]
        public void parseSequencePoint()
        {

            WhenParsing(openCoverExample);
            _modelMock.Verify(parser => parser.AddSequencePoint("1","27","2"));
        }

        [TestMethod]
        public void parseBranchPoint()
        {

            WhenParsing(openCoverExample);
            _modelMock.Verify(parser => parser.AddBranchPoint(1,27,1,true));
        }

        private void WhenParsing( string input)
        {
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            StreamReader reader = new StreamReader(inputStream);
            XmlReader xmlReader = XmlReader.Create(reader);
            xmlReader.MoveToContent();
            _parser.ParseModule(_model, xmlReader);
            _parser.ParseModule(_model, xmlReader);

        }
    }
}
