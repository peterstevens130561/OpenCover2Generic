using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverStateMachineTests
    {

        private const string BeginningPart =
            @"Executing: C:\Program Files(x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe
Microsoft(R) Test Execution Command Line Tool Version 14.0.25420.1
Copyright(c) Microsoft Corporation.All rights reserved.";
        private const string LoggerInstalledPart =
@"Initializing VsTestSonarQubeLogger
testRunDirectory E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults
Starting test execution, please wait...
";

        private const string RegistrationFailedPart = @"
Failed to register(user:True,register:True,is64:False):5 the profiler assembly; you may want to look into permissions or using the -register:user option instead";

        private const string TestResultsPart = @"
VsTestSonarQubeLogger.TestResults=E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults\1d643de0-bd38-4d45-890a-6b32ca42109c.xml
";
    private const string CommittingPart= @"Committing...
        Visited Classes 0 of 411 (0)
            Visited Methods 0 of 2823 (0)
            Visited Points 0 of 11184 (0)
            Visited Branches 0 of 6514 (0)

        ==== Alternative Results(includes all methods including those without corresponding source) ====
        Alternative Visited Classes 0 of 469 (0)
            Alternative Visited Methods 0 of 3355 (0)";

        private const string LoggerNotInstalled = @"Error: Could not find a test logger with URI or FriendlyName 'VsTestSonarQubeLogger'.";
        private const string NoTestsPart = @"No test is available in E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.CadenceDataManager.UnitTest\bin\Release\Bhi.Esie.Services.CadenceDataManager.UnitTest.dll. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.
Warning: No test is available in E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.CadenceDataManager.UnitTest\bin\Release\Bhi.Esie.Services.CadenceDataManager.UnitTest.dll. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.

Ignored 0
VsTestSonarQubeLogger.TestResults=E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults\1d643de0-bd38-4d45-890a-6b32ca42109c.xml

Information: Additionally, you can try specifying '/UseVsixExtensions' command if the test discoverer & executor is installed on the machine as vsix extensions and your installation supports vsix extensions. Example: vstest.console.exe myTests.dll /UseVsixExtensions:true";

        private const string RunningPart = @"
        Starting test execution, please wait...
        Passed   ShouldCreateCorrectlyFormattedString
        Passed   ShouldThrowNotSupportedException
        Passed   Ctor_UsesCorrectDataPoints
        Passed   SetActualValues_WrongIndexThrowsException
        Passed   SetActualValues_UsesCorrectRawData
        Passed   InitializeConstructor_ShouldInitializePropertiesCorrectly
        Ignored 0";

        private const string NoResultsPart = @"
Committing...
No results, this could be for a number of reasons. The most common reasons are:
    1) missing PDBs for the assemblies that match the filter please review the
    output file and refer to the Usage guide (Usage.rtf) about filters.
    2) the profiler may not be registered correctly, please refer to the Usage
    guide and the -register switch.";




        private readonly StateMachine _stateMachine = new StateMachine();
        [TestMethod]
        public void State_NormalSequence_State_ExpectDone()
        {
            string WithTests = BeginningPart + LoggerInstalledPart + RunningPart + TestResultsPart + CommittingPart;
            Assert.AreEqual(ProcessState.Done, RunSequence(WithTests));
        }

   
        [TestMethod]
        public void State_NoTests_State_ExpectNoTests()
        {
            const string noTests =
                BeginningPart + LoggerInstalledPart+NoTestsPart + TestResultsPart + CommittingPart;

        Assert.AreEqual(ProcessState.NoTests, RunSequence(noTests));
        }

        [TestMethod]
        public void State_NoResults_State_ExpectNoResults()
        {
            const string noResults = BeginningPart + LoggerInstalledPart + RunningPart + TestResultsPart + NoResultsPart;
            Assert.AreEqual(ProcessState.NoResults, RunSequence(noResults));
        }

        [TestMethod]
        public void State_RegistrationFailed_State_ExpectRegistrationFailed()
        {
            const string registrationFailed = RegistrationFailedPart;
            Assert.AreEqual(ProcessState.CouldNotRegister, RunSequence(registrationFailed));
        }

        [TestMethod]
        public void State_LoggerNotInstalled_State_ExpectLoggerNotInstalled()
        {
            const string loggerNotInstalledLog = BeginningPart + LoggerNotInstalled + NoResultsPart;
                Assert.AreEqual(ProcessState.LoggerNotInstalled, RunSequence(loggerNotInstalledLog));
    }
       
        private ProcessState RunSequence(string output)
        {
            string[] parts = output.Split('\n');
            _stateMachine.State = ProcessState.Starting;
            foreach (var part in parts)
            {
                _stateMachine.Transition(part);
            }
            return _stateMachine.State;
        }
    }
}
