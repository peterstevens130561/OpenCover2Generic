using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using System.Reflection;
using System.Timers;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverRunnerTests
    {
        private Mock<IProcessFactory> _processFactoryMock;
        private Mock<TimerSeam> _timerMock = new Mock<TimerSeam>();
        private Mock<IOpenCoverProcess> _openCoverProcessMock = new Mock<IOpenCoverProcess>();
        [TestInitialize]
        public void Initialize()
        {
            _processFactoryMock = new Mock<IProcessFactory>();
            _timerMock = new Mock<TimerSeam>();
            _openCoverProcessMock = new Mock<IOpenCoverProcess>();
            _processFactoryMock.Setup(p => p.CreateOpenCoverProcess()).Returns(_openCoverProcessMock.Object);
        }

        [TestMethod]
        public void Run_CouldNotRegisterOnFirst_OkOnSecond()
        {
            //given a valid runner which will not register on starting, but will on second

            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            _openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Failed to register(user:True"));

            // as the opencoverprocess is mocked, we need to set its property value
            _openCoverProcessMock.SetupSequence(p => p.RecoverableError).Returns(true).Returns(false);
            _openCoverProcessMock.SetupSequence(p => p.Started).Returns(false).Returns(true);
            _openCoverProcessMock.Setup(p => p.Start())
                           .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                                CreateMockDataReceivedEventArgs("Failed to register(user:True"),
                                CreateMockDataReceivedEventArgs("Starting test execution, please wait..")
                           }).Dequeue);

            IProcessFactory processFactory = _processFactoryMock.Object;

            var testRunner = new OpenCoverRunner.OpenCoverRunnerManager(processFactory,_timerMock.Object);
            ProcessStartInfo info = new ProcessStartInfo();
            
            //when starting the runner

                using (StreamWriter writer = new StreamWriter(new MemoryStream()))
                {
                    testRunner.Run(info, writer);
                }
            // should be called twice
            _openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(2));
        }

        [TestMethod]
        public void SetTimeOut_OneMinute_ExpectOneMinute()
        {
            IOpenCoverRunnerManager openCoverRunnerManager = new OpenCoverRunnerManager(null, _timerMock.Object);
            openCoverRunnerManager.SetTimeOut(new TimeSpan(0, 1, 0));
            _timerMock.VerifySet(t => t.Interval=60000,Times.Exactly(1));

        }

        [TestMethod]
        public void SetTimeOut_ZeroMinute_ExpectNotSet()
        {
            IOpenCoverRunnerManager openCoverRunnerManager = new OpenCoverRunnerManager(null, _timerMock.Object);
            openCoverRunnerManager.SetTimeOut(new TimeSpan(0, 0, 0));
            _timerMock.VerifySet(t => t.Interval =It.IsAny<double>(), Times.Exactly(0));

        }
        [TestMethod]
        public void Run_CouldNotRegisterAtAll_InvalidOperationExceptionThrown()
        {
            //given a valid runner which will not register on starting, but will on second
            _openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            _openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Failed to register(user:True"));

            // as the opencoverprocess is mocked, we need to set its property value
            _openCoverProcessMock.SetupSequence(p => p.RecoverableError)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true);

            IProcessFactory processFactory = _processFactoryMock.Object;
            var testRunner = new OpenCoverRunner.OpenCoverRunnerManager(processFactory,_timerMock.Object);
            ProcessStartInfo info = new ProcessStartInfo();

            //when starting the runner
            try
            {
                using (StreamWriter writer = new StreamWriter(new MemoryStream()))
                {
                    testRunner.Run(info, writer);
                }
            } catch (InvalidOperationException )
            {
                _openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(10));
                return;
            }
            Assert.Fail("expected InvalidOperatonException");

        }

        [TestMethod]
        public void Run_DeploymentIssue_ErrorLog()
        {
            
        }
        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string testData)
            {

                if (String.IsNullOrEmpty(testData))
                    throw new ArgumentException("Data is null or empty.", "testData");

                DataReceivedEventArgs mockEventArgs =
                    (DataReceivedEventArgs)System.Runtime.Serialization.FormatterServices
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
        }
    }
