using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverProcessTests
    {
        private Mock<IProcess> _processMock;
        private IOpenCoverProcess _openCoverProcess;
        private Mock<ITimerSeam> _timerMock;

        [TestInitialize]
        public void Initialize()
        {
            _processMock = new Mock<IProcess>();
            _timerMock = new Mock<ITimerSeam>();
            _openCoverProcess = new OpenCoverProcess(_processMock.Object,_timerMock.Object);

        }


        [TestMethod]
        public void Start_OnFailedRegistration_CouldNotRegister()
        {

            SetupForRegistrationFailure(_processMock);
            _openCoverProcess.Start();
            Assert.AreEqual(ProcessState.CouldNotRegister,_openCoverProcess.State);
        }

        [TestMethod]
        public void Start_OnFailedRegistration_StartedIsFalse()
        {
            SetupForRegistrationFailure(_processMock);
            _openCoverProcess.Start();
            Assert.IsFalse(_openCoverProcess.Started);
        }

    
        [TestMethod]
        public void Start_OnStarted_StartedIsTrue()
        {
            SetupForStart(_processMock);
            _openCoverProcess.Start();
            Assert.IsTrue(_openCoverProcess.Started);
        }

        [TestMethod]
        public void Start_OnStarted_StateIsStarted()
        {
            SetupForStart(_processMock);
            _openCoverProcess.Start();
            Assert.AreEqual(ProcessState.Busy, _openCoverProcess.State);
        }


        [TestMethod]
        public void Start_OnNoResults_StateIsNoResults()
        {
            SetupForNoResults(_processMock);
            _openCoverProcess.Start();
            Assert.AreEqual(ProcessState.NoResults, _openCoverProcess.State);
        }

        [TestMethod]
        public void SetTimeOut_OneMinute_ExpectOneMinute()
        {

            _openCoverProcess.SetTimeOut(new TimeSpan(0, 1, 0));
            _timerMock.VerifySet(t => t.Interval = 60000, Times.Exactly(1));

        }

        [TestMethod]
        public void SetTimeOut_ZeroMinute_ExpectNotSet()
        {
            _openCoverProcess.SetTimeOut(new TimeSpan(0, 0, 0));
            _timerMock.VerifySet(t => t.Interval = It.IsAny<double>(), Times.Exactly(0));

        }


        public void Run_TimedOut_TestTimedOutException()
        {
            Boolean killed = false;
            _openCoverProcess.SetTimeOut(new TimeSpan(0, 0, 1));
            _processMock.Setup(p => p.Kill()).Callback(() =>
                killed = true);
            _processMock.Setup(p => p.HasExited).Returns(killed);
            _timerMock.Setup(t => t.Start()).Raises(p => p.Elapsed += null, CreateMockEvent<ElapsedEventArgs,DateTime>(DateTime.Now));

            _openCoverProcess.Start();

            Assert.AreEqual(ProcessState.TimedOut, _openCoverProcess.State);

        }

        [TestMethod]
        public void Run_TimedOut_TestTimedOutException2()
        {
            _openCoverProcess = new OpenCoverProcess(_processMock.Object, new TimerSeam());
            _openCoverProcess.SetTimeOut(new TimeSpan(0, 0, 1));
            _processMock.Setup(p => p.Kill()).Callback(() =>
                _processMock.Setup(p => p.HasExited).Returns(true));

            _openCoverProcess.Start();

            Assert.AreEqual(ProcessState.TimedOut, _openCoverProcess.State);

        }

        [TestMethod]
        public void Run_NoTests_StateIsNoTests()
        {
            _processMock.SetupSequence(p => p.HasExited).Returns(false).Returns(true);
            _processMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                    CreateMockDataReceivedEventArgs(@"Starting test execution, please wait.."),
                    CreateMockDataReceivedEventArgs(@"No test is available in "),
                    CreateMockDataReceivedEventArgs(@"VsTestSonarQubeLogger.TestResults=E:\Cadence\ESIETooLink\Main\TestResults\287d73c4-8c3e-4ecf-b41d-3c29a5cfe375.xml"),
                    CreateMockDataReceivedEventArgs(@"No results, this could be for a number of reasons. The most common reasons are:")

                }).Dequeue);
            _openCoverProcess.Start();
            Assert.AreEqual(ProcessState.NoTests, _openCoverProcess.State);
        }
        private void SetupForStart(Mock<IProcess> processMock)
        {
            processMock.Setup(p => p.HasExited).Returns(true);
            processMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                CreateMockDataReceivedEventArgs("Starting test execution, please wait.."),
                }).Dequeue);
        }

        private void SetupForRegistrationFailure(Mock<IProcess> processMock)
        {
            processMock.Setup(p => p.HasExited).Returns(true);
            processMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                CreateMockDataReceivedEventArgs("Failed to register(user:True"),
                }).Dequeue);
        }

        private void SetupForNoResults(Mock<IProcess> processMock)
        {
            processMock.Setup(p => p.HasExited).Returns(true);
            processMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                    CreateMockDataReceivedEventArgs("No results, this could be for a number of reasons. The most common reasons are:"),
                }).Dequeue);
        }

        private T CreateMockEvent<T,Y>(Y testData)
        {
            T mockEventArgs =
                (T)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(T));

            FieldInfo[] eventFields = typeof(T)
                .GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);

            if (eventFields.Count() > 0)
            {
                eventFields[0].SetValue(mockEventArgs, testData);
            }
            else
            {
                throw new ApplicationException(
                    "Failed to find _data field!");
            }

            return mockEventArgs;
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

    if (eventFields.Count() > 0)
    {
        eventFields[0].SetValue(mockEventArgs, testData);
    }
    else
    {
        throw new ApplicationException(
            "Failed to find _data field!");
    }

    return mockEventArgs;
}
        }

    }
