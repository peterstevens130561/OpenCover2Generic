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
        private IModuleCoverageEntity _entity;
        private OpenCoverCoverageParser _parser;
        private Mock<IModuleCoverageEntity> _modelMock;
        private readonly string _openCoverExample= @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                <BranchPointValue vc=""3"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
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
            _modelMock = new Mock<IModuleCoverageEntity>();
            _entity = _modelMock.Object;
            _parser = new OpenCoverCoverageParser();

        }

        [TestMethod]
        public void ParseModule_Initiated_ValidFile_Expected()
        {

            Assert.IsTrue(WhenParsing(_openCoverExample));
            _modelMock.Verify(parser => parser.AddFile("1", @"E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"));
        }

        [TestMethod]
        public void ParseModule_OneModule_ParseModule_NameValid()
        {

            Assert.IsTrue(WhenParsing(_openCoverExample));
            _modelMock.VerifySet(m=>m.NameId=@"Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest");

        }

        [TestMethod]
        public void ParseModule_Initiated_ValidFile_ExpectModuleName()
        {

            Assert.IsTrue(WhenParsing(_openCoverExample));
            Assert.AreEqual("Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest", _parser.ModuleName);
        }

        [TestMethod]
        public void ParseModule_Initiated_ValidFile_SequencePoint()
        {

            Assert.IsTrue(WhenParsing(_openCoverExample));
            _modelMock.Verify(parser => parser.AddSequencePoint("1","27","2"));
        }

        [TestMethod]
        public void ParseModule_Initiated_ValidFile_BranchPoint()
        {

            Assert.IsTrue(WhenParsing(_openCoverExample));
            _modelMock.Verify(parser => parser.AddBranchPoint(1,27,1,true));
        }

        private readonly string _skippedModules = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Summary numSequencePoints=""172"" visitedSequencePoints=""0"" numBranchPoints=""24"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""3"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""24"" />
  <Modules>
    <Module skippedDueTo=""MissingPdb"" hash=""11-EF-82-96-1B-91-F8-56-89-F6-A5-DA-94-D0-D5-C3-2F-32-98-35"">
      <ModulePath>C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\PrivateAssemblies\Microsoft.Diagnostics.Tracing.EventSource.dll</ModulePath>
      <ModuleTime>2015-06-10T17:13:44Z</ModuleTime>
      <ModuleName>Microsoft.Diagnostics.Tracing.EventSource</ModuleName>
      <Classes />
    </Module>
    <Module skippedDueTo=""MissingPdb"" hash=""3C-84-7A-95-A6-54-7A-A4-99-19-78-9D-7A-0C-B6-ED-76-12-28-49"">
      <ModulePath>C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\PrivateAssemblies\Microsoft.Threading.Tasks.Extensions.dll</ModulePath>
      <ModuleTime>2015-06-10T17:13:44Z</ModuleTime>
      <ModuleName>Microsoft.Threading.Tasks.Extensions</ModuleName>
      <Classes />
    </Module>
  </Modules>
</CoverageSession>
";

        private StreamReader _streamReader;
        private Stream _inputStream;
        private XmlReader _xmlReader;

        [TestMethod]
        public void ParseModule_Initiated_TwoModulesMixedWithSkipped_TwoModulesParsed()
        {
                  string coverage = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module skippedDueTo=""MissingPdb"" hash=""11-EF-82-96-1B-91-F8-56-89-F6-A5-DA-94-D0-D5-C3-2F-32-98-35"">
      <ModulePath>C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\PrivateAssemblies\Microsoft.Diagnostics.Tracing.EventSource.dll</ModulePath>
      <ModuleTime>2015-06-10T17:13:44Z</ModuleTime>
      <ModuleName>Microsoft.Diagnostics.Tracing.EventSource</ModuleName>
      <Classes />
    </Module>
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
                <BranchPointValue vc=""3"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    <Module skippedDueTo=""MissingPdb"" hash=""11-EF-82-96-1B-91-F8-56-89-F6-A5-DA-94-D0-D5-C3-2F-32-98-35"">
      <ModulePath>C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\PrivateAssemblies\Microsoft.Diagnostics.Tracing.EventSource.dll</ModulePath>
      <ModuleTime>2015-06-10T17:13:44Z</ModuleTime>
      <ModuleName>Microsoft.Diagnostics.Tracing.EventSource</ModuleName>
      <Classes />
    </Module>
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
                <BranchPointValue vc=""3"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";
            Assert.IsTrue(WhenParsing(coverage),"parsing");
            Assert.IsTrue(WhenContinueParsing(),"continue");
            Assert.IsFalse(WhenContinueParsing(),"finally");
        }
        [TestMethod]
        public void ParseModule_Initiated_OnlySkipped_NothingParsed()
        {

        }

        private bool WhenParsing( string input)
        {
            _inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            _streamReader = new StreamReader(_inputStream);
            _xmlReader = XmlReader.Create(_streamReader);
            _xmlReader.MoveToContent();
            return _parser.ParseModule(_entity, _xmlReader);
        }

        private bool WhenContinueParsing()
        {
            return _parser.ParseModule(_entity, _xmlReader);

        }
    }
}
