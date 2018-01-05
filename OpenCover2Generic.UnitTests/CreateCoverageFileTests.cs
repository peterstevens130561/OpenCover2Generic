using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CreateCoverageFileTests
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GenerateCoverage_EmptyModule_GenerateCoverage_OnlyShouldCreateHeaderOnly()
        {

            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();
            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GenerateCoverage_SkippedModule_GenerateCoverage_OnlyShouldBeIgnored()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
  <Modules>
    <Module skippedDueTo=""Filter"" hash=""CF-B4-AA-F9-F3-33-2A-2E-60-04-2A-3E-24-D0-0D-04-B8-49-54-84"">
      <ModulePath>C:\windows\Microsoft.Net\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll</ModulePath>
      <ModuleTime>2016-07-14T12:43:42Z</ModuleTime>
      <ModuleName>mscorlib</ModuleName>
      <Classes />
    </Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MGenerateCoverage_ValidModule_GenerateCoverage_ShouldGenerateFiles()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module hash=""AF-9C-7F-A4-DD-C2-F3-98-52-99-5F-75-22-1D-C1-5F-2A-5D-BE-62"">
      <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
      <ModulePath>E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest\bin\Debug\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.dll</ModulePath>
      <ModuleTime>2017-07-12T06:36:07.5940095Z</ModuleTime>
      <ModuleName>Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest</ModuleName>
      <Files>
        <File uid=""1"" fullPath=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"" />
      </Files>
      <Classes>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"">
    <file path=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"" />
</coverage>";
            TestUtils.AssertStringsSame(expected, result);
        }

        [TestMethod]
        public void GenerateCoverage_ValidModuleShouldBeParses()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module hash=""AF-9C-7F-A4-DD-C2-F3-98-52-99-5F-75-22-1D-C1-5F-2A-5D-BE-62"">
      <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
      <ModulePath>E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest\bin\Debug\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.dll</ModulePath>
      <ModuleTime>2017-07-12T06:36:07.5940095Z</ModuleTime>
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
                <SequencePoint vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""1"" uspid=""2"" ordinal=""1"" offset=""1"" sl=""28"" sc=""17"" el=""28"" ec=""52"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""2"" uspid=""3"" ordinal=""2"" offset=""10"" sl=""29"" sc=""13"" el=""29"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
              </SequencePoints>
              <BranchPoints />
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"">
    <file path=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"">
        <lineToCover lineNumber=""27"" covered=""false"" />
        <lineToCover lineNumber=""28"" covered=""true"" />
        <lineToCover lineNumber=""29"" covered=""true"" />
    </file>
</coverage>";
            TestUtils.AssertStringsSame(expected, result);
        }


        [TestMethod]
        public void GenerateCoverage_BranchPoints_GenerateCoverage_ShouldBeCovered()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module hash=""AF-9C-7F-A4-DD-C2-F3-98-52-99-5F-75-22-1D-C1-5F-2A-5D-BE-62"">
      <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
      <ModulePath>E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest\bin\Debug\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.dll</ModulePath>
      <ModuleTime>2017-07-12T06:36:07.5940095Z</ModuleTime>
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
                <SequencePoint vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""1"" uspid=""2"" ordinal=""1"" offset=""1"" sl=""28"" sc=""17"" el=""28"" ec=""52"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""2"" uspid=""3"" ordinal=""2"" offset=""10"" sl=""29"" sc=""13"" el=""29"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
              </SequencePoints>
              <BranchPoints>
                <BranchPointValue vc=""0"" uspid=""3137"" ordinal=""11"" offset=""687"" sl=""27"" path=""0"" offsetend=""689"" fileid=""1"" />
                <BranchPointValue vc=""1"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"">
    <file path=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"">
        <lineToCover lineNumber=""27"" covered=""false"" branchesToCover=""2"" coveredBranches=""1"" />
        <lineToCover lineNumber=""28"" covered=""true"" />
        <lineToCover lineNumber=""29"" covered=""true"" />
    </file>
</coverage>";
            TestUtils.AssertStringsSame(expected, result);
        }

        [TestMethod]
        public void GenerateCoverage_BranchPointsReportedTwiceSecond_GenerateCoverage_TimeAllShouldBeCovered()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <CoverageSession xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<Modules>
    <Module hash=""AF-9C-7F-A4-DD-C2-F3-98-52-99-5F-75-22-1D-C1-5F-2A-5D-BE-62"">
      <Summary numSequencePoints=""37"" visitedSequencePoints=""0"" numBranchPoints=""10"" visitedBranchPoints=""0"" sequenceCoverage=""0"" branchCoverage=""0"" maxCyclomaticComplexity=""2"" minCyclomaticComplexity=""1"" visitedClasses=""0"" numClasses=""2"" visitedMethods=""0"" numMethods=""10"" />
      <ModulePath>E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest\bin\Debug\Bhi.Esie.Services.EsieTooLinkRepository.SqlServer.UnitTest.dll</ModulePath>
      <ModuleTime>2017-07-12T06:36:07.5940095Z</ModuleTime>
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
                <SequencePoint vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""1"" uspid=""2"" ordinal=""1"" offset=""1"" sl=""28"" sc=""17"" el=""28"" ec=""52"" bec=""0"" bev=""0"" fileid=""1"" />
                <SequencePoint vc=""2"" uspid=""3"" ordinal=""2"" offset=""10"" sl=""29"" sc=""13"" el=""29"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
              </SequencePoints>
              <BranchPoints>
                <BranchPointValue vc=""0"" uspid=""3137"" ordinal=""11"" offset=""687"" sl=""27"" path=""0"" offsetend=""689"" fileid=""1"" />
                <BranchPointValue vc=""1"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <BranchPoints>
                <BranchPointValue vc=""1"" uspid=""3137"" ordinal=""11"" offset=""687"" sl=""27"" path=""0"" offsetend=""689"" fileid=""1"" />
                <BranchPointValue vc=""1"" uspid=""3138"" ordinal=""12"" offset=""687"" sl=""27"" path=""1"" offsetend=""714"" fileid=""1"" />
                </BranchPoints>
              <MethodPoint xsi:type=""SequencePoint"" vc=""0"" uspid=""1"" ordinal=""0"" offset=""0"" sl=""27"" sc=""13"" el=""27"" ec=""14"" bec=""0"" bev=""0"" fileid=""1"" />
            </Method>
            </Methods>
            </Class>
            </Classes>
</Module>
    </Modules>
    </CoverageSession>";
            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream, input);
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"">
    <file path=""E:\Cadence\EsieTooLinkRepositoryServiceTest.cs"">
        <lineToCover lineNumber=""27"" covered=""false"" branchesToCover=""2"" coveredBranches=""2"" />
        <lineToCover lineNumber=""28"" covered=""true"" />
        <lineToCover lineNumber=""29"" covered=""true"" />
    </file>
</coverage>";
            TestUtils.AssertStringsSame(expected, result);
        }
        private string WhenConverting(MemoryStream resultStream, string input)
        {
            StreamWriter streamWriter = new StreamWriter(resultStream);
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            StreamReader reader = new StreamReader(inputStream);
            var coverageWriter = new GenericCoverageWriter();
            using (var writer = new XmlTextWriter(streamWriter))
            {
                coverageWriter.WriteBegin(writer);
                var parser = new OpenCoverCoverageParser();
                var model=new AggregatedModuleCoverageEntity();
                parser.ParseModule(model, XmlReader.Create(reader));
                coverageWriter.GenerateCoverage(model, writer);
                coverageWriter.WriteEnd(writer);

            }
            StreamReader resultReader = new StreamReader(new MemoryStream(resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            return text;
        }

    }
}
