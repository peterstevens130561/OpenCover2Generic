using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverProcessTests
    {
        private Mock<IProcessAdapter> _processMock;
        private IOpenCoverProcess _openCoverProcess;
        private Mock<ITimerAdapter> _timerMock;
        private Mock<IStateMachine> _stateMachineMock;


        [TestInitialize]
        public void Initialize()
        {
            _processMock = new Mock<IProcessAdapter>();
            _timerMock = new Mock<ITimerAdapter>();
            _stateMachineMock = new Mock<IStateMachine>();
            _openCoverProcess = new OpenCoverProcess(_processMock.Object,_timerMock.Object,_stateMachineMock.Object);

        }


        [TestMethod]
        public void Start_OnStarted_StateIsStarted()
        {
            SetupForStart(_processMock);
            _openCoverProcess.Start();
            Assert.AreEqual(ProcessState.RunningTests, _openCoverProcess.State);
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



        private void SetupForStart(Mock<IProcessAdapter> processMock)
        {
            processMock.Setup(p => p.HasExited).Returns(true);
            _stateMachineMock.Setup(s => s.State).Returns(ProcessState.RunningTests);
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
