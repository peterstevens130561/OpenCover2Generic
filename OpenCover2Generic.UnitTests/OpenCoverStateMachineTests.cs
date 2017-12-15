using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverStateMachineTests
    {


        private const string WithTests = @"Executing: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe
Microsoft (R) Test Execution Command Line Tool Version 14.0.25420.1
Copyright (c) Microsoft Corporation.  All rights reserved.

Initializing VsTestSonarQubeLogger
testRunDirectory E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults
Starting test execution, please wait...
Passed   ShouldCreateCorrectlyFormattedString
Passed   ShouldThrowNotSupportedException
Passed   Ctor_UsesCorrectDataPoints
Passed   SetActualValues_WrongIndexThrowsException
Passed   SetActualValues_UsesCorrectRawData
Passed   InitializeConstructor_ShouldInitializePropertiesCorrectly

Ignored 0
VsTestSonarQubeLogger.TestResults=E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults\c5ce11f8-273c-477a-8196-fd3e40dae809.xml

Total tests: 366. Passed: 365. Failed: 0. Skipped: 1.
Test Run Successful.
Test execution time: 14.4802 Seconds
Committing...
An System.IO.DirectoryNotFoundException occured: Could not find a part of the path 'C:\projects\fluentassertions-vf06b\Src\JetBrainsAnnotations.cs'. 

Visited Classes 519 of 1551 (33.46)
Visited Methods 2971 of 9566 (31.06)
Visited Points 8833 of 32561 (27.13)
Visited Branches 3927 of 18565 (21.15)

==== Alternative Results (includes all methods including those without corresponding source) ====
Alternative Visited Classes 595 of 1706 (34.88)
Alternative Visited Methods 3490 of 11195 (31.17)"
            ;

        private readonly StateMachine _stateMachine = new StateMachine();
        [TestMethod]
        public void CheckFinalState_NormalSequence_ExpectDone()
        {
            Assert.AreEqual(ProcessState.Done, RunSequence(WithTests));
        }

   
        [TestMethod]
        public void CheckFinalState_NoTests_ExpectNoTests()
        {
            const string NoTests = @"Executing: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe
Microsoft (R) Test Execution Command Line Tool Version 14.0.25420.1
Copyright (c) Microsoft Corporation.  All rights reserved.

Initializing VsTestSonarQubeLogger
testRunDirectory E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults
Starting test execution, please wait...
No test is available in E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.CadenceDataManager.UnitTest\bin\Release\Bhi.Esie.Services.CadenceDataManager.UnitTest.dll. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.
Warning: No test is available in E:\Cadence\ESIETooLink\Main\Services\Bhi.Esie.Services.CadenceDataManager.UnitTest\bin\Release\Bhi.Esie.Services.CadenceDataManager.UnitTest.dll. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.

Ignored 0
VsTestSonarQubeLogger.TestResults=E:\Cadence\ESIETooLink\Main\Deliverables\bin\Release\TestResults\1d643de0-bd38-4d45-890a-6b32ca42109c.xml

Information: Additionally, you can try specifying '/UseVsixExtensions' command if the test discoverer & executor is installed on the machine as vsix extensions and your installation supports vsix extensions. Example: vstest.console.exe myTests.dll /UseVsixExtensions:true

Committing...
Visited Classes 0 of 411 (0)
Visited Methods 0 of 2823 (0)
Visited Points 0 of 11184 (0)
Visited Branches 0 of 6514 (0)

==== Alternative Results (includes all methods including those without corresponding source) ====
Alternative Visited Classes 0 of 469 (0)
Alternative Visited Methods 0 of 3355 (0)";

        Assert.AreEqual(ProcessState.NoTests, RunSequence(NoTests));
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
