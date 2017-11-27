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

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverProcessTests
    {
        private Mock<IProcess> _processMock;
        private IOpenCoverProcess _openCoverProcess;

        [TestInitialize]
        public void Initialize()
        {
            _processMock = new Mock<IProcess>();
            _openCoverProcess = new OpenCoverProcess(_processMock.Object);
        }
        [TestMethod]
        public void Start_OnFailedRegistration_RecoverableErrorIsTrue()
        {

            SetupForRegistrationFailure(_processMock);
            _openCoverProcess.Start();
            Assert.IsTrue(_openCoverProcess.RecoverableError);
        }

        [TestMethod]
        public void Start_OnFailedRegistration_StartedIsFalse()
        {
            SetupForRegistrationFailure(_processMock);
            _openCoverProcess.Start();
            Assert.IsFalse(_openCoverProcess.Started);
        }

    
        public void Start_OnStarted_StartedIsTrue()
        {
            SetupForStart(_processMock);
            _openCoverProcess.Start();
            Assert.IsTrue(_openCoverProcess.Started);
        }

        public void Start_OnStarted_RecoverableErrorIsFalse()
        {
            SetupForStart(_processMock);
            _openCoverProcess.Start();
            Assert.IsFalse(_openCoverProcess.RecoverableError);
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
