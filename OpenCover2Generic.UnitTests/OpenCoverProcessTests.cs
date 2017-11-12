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
        private Mock<IProcess> processMock;
        private IOpenCoverProcess openCoverProcess;

        [TestInitialize]
        public void Initialize()
        {
            processMock = new Mock<IProcess>();
            openCoverProcess = new OpenCoverProcess(processMock.Object);
        }
        [TestMethod]
        public void Start_OnFailedRegistration_RecoverableErrorIsTrue()
        {

            SetupForRegistrationFailure(processMock);
            openCoverProcess.Start();
            Assert.IsTrue(openCoverProcess.RecoverableError);
        }

        [TestMethod]
        public void Start_OnFailedRegistration_StartedIsFalse()
        {
            SetupForRegistrationFailure(processMock);
            openCoverProcess.Start();
            Assert.IsFalse(openCoverProcess.Started);
        }

    
        public void Start_OnStarted_StartedIsTrue()
        {
            SetupForStart(processMock);
            openCoverProcess.Start();
            Assert.IsTrue(openCoverProcess.Started);
        }

        public void Start_OnStarted_RecoverableErrorIsFalse()
        {
            SetupForStart(processMock);
            openCoverProcess.Start();
            Assert.IsFalse(openCoverProcess.RecoverableError);
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

        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string TestData)
{

    if (String.IsNullOrEmpty(TestData))
        throw new ArgumentException("Data is null or empty.", "TestData");

    DataReceivedEventArgs MockEventArgs =
        (DataReceivedEventArgs)System.Runtime.Serialization.FormatterServices
         .GetUninitializedObject(typeof(DataReceivedEventArgs));

    FieldInfo[] EventFields = typeof(DataReceivedEventArgs)
        .GetFields(
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.DeclaredOnly);

    if (EventFields.Count() > 0)
    {
        EventFields[0].SetValue(MockEventArgs, TestData);
    }
    else
    {
        throw new ApplicationException(
            "Failed to find _data field!");
    }

    return MockEventArgs;
}
        }

    }
