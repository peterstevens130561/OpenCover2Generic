using System;
using TechTalk.SpecFlow;

namespace BHGE.SonarQube.OpenCoverWrapper.IntegrarionTests
{
    [Binding]
    public class SplitOpenCoverCoverageFileIntoGenericCoverageFilesSteps
    {
        [Given(@"I have coverage file ""(.*)""  for assembly ""(.*)""")]
        public void GivenIHaveCoverageFileForAssembly(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have coverage file ""(.*)"" for assembly ""(.*)""")]
        public void GivenIHaveCoverageFileForAssembly(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I convert it")]
        public void WhenIConvertIt()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should have ""(.*)""")]
        public void ThenIShouldHave(string p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
