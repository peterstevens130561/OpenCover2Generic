using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System.Reflection;
using BHGE.SonarQube.OpenCover2Generic.Consumer;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverRunnerManagerTests
    {
        private Mock<IProcessFactory> _processFactoryMock;
        private Mock<IOpenCoverProcess> _openCoverProcessMock = new Mock<IOpenCoverProcess>();
        private IOpenCoverRunnerManager _openCoverRunnerManager;

        [TestInitialize]
        public void Initialize()
        {
            _processFactoryMock = new Mock<IProcessFactory>();
            _openCoverProcessMock = new Mock<IOpenCoverProcess>();
            _processFactoryMock.Setup(p => p.CreateOpenCoverProcess()).Returns(_openCoverProcessMock.Object);
            _openCoverRunnerManager = new OpenCoverRunnerManager(_processFactoryMock.Object);
        }

        [TestMethod]
        public void Run_CouldNotRegisterOnFirst_OkOnSecond()
        {
            //given a valid runner which will not register on starting, but will on second

            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            _openCoverProcessMock.Setup(p => p.TestResultsPath).Returns("bla");


            // as the opencoverprocess is mocked, we need to set its property value
            _openCoverProcessMock.SetupSequence(p => p.State).Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.Done);
            _openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[]
                {
                    CreateMockDataReceivedEventArgs("Failed to register(user:True"),
                    CreateMockDataReceivedEventArgs("Starting test execution, please wait..")
                }).Dequeue);


            ProcessStartInfo info = new ProcessStartInfo();

            //when starting the runner
            WhenRun(info);
            // should be called twice
            _openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(2));
            Assert.AreEqual("bla", _openCoverRunnerManager.TestResultsPath);
        }


        [TestMethod]
        public void Run_CouldNotRegisterAtAll_InvalidOperationExceptionThrown()
        {
            //given a valid runner which will not register on starting, but will on second
            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            _openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Failed to register(user:True"));

            // as the opencoverprocess is mocked, we need to set its property value
            SetupToFaulToRegister10Times();
            ProcessStartInfo info = new ProcessStartInfo();

            //when starting the runner
            try
            {
                WhenRun(info);
            }
            catch (InvalidOperationException)
            {
                _openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(10));
                return;
            }
            Assert.Fail("expected InvalidOperatonException");

        }



        [TestMethod]
        public void Run_NoResults_InvalidTestConfigurationException()
        {
            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);

            // as the opencoverprocess is mocked, we need to set its property value
            _openCoverProcessMock.SetupSequence(p => p.State).Returns(ProcessState.NoResults);
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                WhenRun(info);
            }
            catch (InvalidTestConfigurationException e)
            {
                return;
            }
            Assert.Fail("Expected exception");
        }

        private Moq.Language.ISetupSequentialResult<ProcessState> SetupToFaulToRegister10Times()
        {
            return _openCoverProcessMock.SetupSequence(p => p.State)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister)
                .Returns(ProcessState.CouldNotRegister);
        }

        [TestMethod]
        public void Run_TimedOut_TestTimedOutException()
        {
            _openCoverRunnerManager.SetTimeOut(new TimeSpan(0, 0, 1));
            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);

            // as the opencoverprocess is mocked, we need to set its property value
            _openCoverProcessMock.SetupSequence(p => p.State).Returns(ProcessState.TimedOut);
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                WhenRun(info);
            }
            catch (JobTimeOutException e)
            {
                return;
            }
            Assert.Fail("Expected exception");

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
    }
}