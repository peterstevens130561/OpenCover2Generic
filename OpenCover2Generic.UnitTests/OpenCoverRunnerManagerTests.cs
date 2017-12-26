using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System.Reflection;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverRunnerManagerTests
    {
        private Mock<IOpenCoverProcessFactory> _processFactoryMock;
        private Mock<IOpenCoverProcess> _openCoverProcessMock = new Mock<IOpenCoverProcess>();
        private IOpenCoverRunnerManager _openCoverRunnerManager;

        [TestInitialize]
        public void Initialize()
        {
            _processFactoryMock = new Mock<IOpenCoverProcessFactory>();
            _openCoverProcessMock = new Mock<IOpenCoverProcess>();
            _processFactoryMock.Setup(p => p.Create()).Returns(_openCoverProcessMock.Object);
            _openCoverRunnerManager = new OpenCoverRunnerManager(_processFactoryMock.Object);
            // make sure the wait is 1ms, i.s.o 1 second
            ((OpenCoverRunnerManager) _openCoverRunnerManager).WaitTimeSpan = new TimeSpan(0, 0, 0, 0, 1); 
        }

        [TestMethod]
        public void Run_JobWithTests_Run_Done()
        {
            GivenProcessRunsOnce();
            GivenState(ProcessState.Done);
            WhenRun(new ProcessStartInfo());
        }


        [TestMethod]
        public void Run_JobWithoutTests_Run_Done()
        {
            GivenProcessRunsOnce();
            GivenState(ProcessState.NoTests);
            WhenRun(new ProcessStartInfo());
        }

        [TestMethod]
        public void Run_RegistrationFailureThenOk_Run_Done()
        {
            GivenProcessRunsTwice();
            GivenFirstStateThenOkOnSecond(ProcessState.CouldNotRegister);
            WhenRun(new ProcessStartInfo());
        }

        [TestMethod]
        public void Run_JobWithoutResultsFirstThenOk_Run_Done()
        {
            GivenProcessRunsTwice();
            GivenFirstStateThenOkOnSecond(ProcessState.NoResults);
            WhenRun(new ProcessStartInfo());
        }

        [TestMethod]
        public void Run_JobCrashedFirstFirstThenOk_Run_Done()
        {
            GivenProcessRunsTwice();
            GivenFirstStateThenOkOnSecond(ProcessState.Starting);
            WhenRun(new ProcessStartInfo());
        }

        [TestMethod]
        public void Run_JobCrashedDuringRunningFirstFirstThenOk_Run_Done()
        {
            GivenProcessRunsTwice();
            GivenFirstStateThenOkOnSecond(ProcessState.RunningTests);
            WhenRun(new ProcessStartInfo());
        }

        [TestMethod]
        public void Run_LoggerNotInstalled_Run_LoggerNotInstalledExceptionThrown()
        {
            GivenProcessRunsOnce();
            GivenState(ProcessState.LoggerNotInstalled);
            try
            {
                WhenRun(new ProcessStartInfo());
            }
            catch (LoggerNotInstalledException)
            {
                return;
            }
            Assert.Fail(@"expected LoggerNotInstalledException");
        }
        [TestMethod]
        public void Run_JobTimeout_Run_JobTimeoutException()
        {
            GivenProcessRunsOnce();
            GivenState(ProcessState.TimedOut);
            try
            {
                WhenRun(new ProcessStartInfo());
            }
            catch (JobTimeOutException)
            {
                return;
            }
            Assert.Fail(@"expected JobTimeOutException");
        }
        #region BuildingBlocks
        private void GivenProcessRunsOnce()
        {
            _openCoverProcessMock.SetupSequence(o => o.HasExited).Returns(false).Returns(true);
        }
        private void GivenProcessRunsTwice()
        {
            _openCoverProcessMock.SetupSequence(o => o.HasExited).Returns(false).Returns(true);
            _openCoverProcessMock.SetupSequence(o => o.HasExited).Returns(false).Returns(true);
        }

        public void GivenState(ProcessState state)
        {
            _openCoverProcessMock.SetupSequence(o => o.State)
                .Returns(ProcessState.Starting)
                .Returns(ProcessState.Run)
                .Returns(state)
                .Returns(state)
                .Returns(state);
        }
        public void GivenFirstStateThenOkOnSecond(ProcessState firstState)
        {
            _openCoverProcessMock.SetupSequence(o => o.State)
                .Returns(ProcessState.Starting)
                .Returns(ProcessState.Run)
                .Returns(firstState)
                .Returns(firstState)
                .Returns(ProcessState.Starting)
                .Returns(ProcessState.Run)
                .Returns(ProcessState.Done)
                .Returns(ProcessState.Done)
                .Returns(ProcessState.Done);
        }

        [TestMethod]
        public void Run_DeploymentIssue_ErrorLog()
        {
            _openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Warning: Test Run deployment issue"));
        }

        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string testData)
        {

            if (String.IsNullOrEmpty(testData))
                throw new ArgumentException("Data is null or empty.", "testData");

            DataReceivedEventArgs mockEventArgs =
                (DataReceivedEventArgs) System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(DataReceivedEventArgs));

            FieldInfo[] eventFields = typeof(DataReceivedEventArgs)
                .GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);

            if (eventFields.Any())
            {
                eventFields[0].SetValue(mockEventArgs, testData);
            }
            else
            {
                throw new InvalidOperationException(
                    "Failed to find _data field!");
            }

            return mockEventArgs;
        }


        private void WhenRun(ProcessStartInfo info)
        {
            using (StreamWriter writer = new StreamWriter(new MemoryStream()))
            {
                _openCoverRunnerManager.Run(info, writer, "bla");
            }
        }
#endregion
    }
}