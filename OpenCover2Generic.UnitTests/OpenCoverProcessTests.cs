using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverProcessTests
    {
        [TestMethod]
        public void RecoverableError_OnFailedRegistration_True()
        {

            //given a valid runner which will not register on starting
            Mock<IProcess> processMock = new Mock<IProcess>();
            IOpenCoverProcess openCoverProcess = new OpenCoverProcess(processMock.Object);

            processMock.Setup(p => p.HasExited).Returns(true);
            processMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                CreateMockDataReceivedEventArgs("Failed to register(user:True"),
                CreateMockDataReceivedEventArgs("Starting test execution, please wait.."),
                CreateMockDataReceivedEventArgs("Failed to register(user:True")
                }).Dequeue);

            //when starting the runner
            openCoverProcess.Start();
            // should be called twice
            Assert.IsTrue(openCoverProcess.RecoverableError);
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
