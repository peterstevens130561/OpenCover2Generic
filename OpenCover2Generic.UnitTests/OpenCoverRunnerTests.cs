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

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverRunnerTests
    {
        [TestMethod]
        public void FailedToStart()
        {
            //given a valid runner which will not register on starting

            Mock<ProcessFactory> processFactoryMock = new Mock<ProcessFactory>();

            processFactoryMock.Setup(p => p.CreateProcess()).Returns(new ProcessStub());
            ProcessFactory processFactory = processFactoryMock.Object;
            var testRunner = new OpenCoverRunner.OpenCoverRunner(processFactory);
            ProcessStartInfo info = new ProcessStartInfo();
            using (StreamWriter writer = new StreamWriter(new MemoryStream()))
            {
                testRunner.Run(info, writer);
            }
            //when starting the runner

            //then the registration failure will be caught, and the process will be restarted
        }

        private class ProcessStub : Process
        {
            public new event DataReceivedEventHandler OutputDataReceived;
            public new void Start()
            {
                //we really do not want to do anything here
            }

            public new void BeginOutputReadLine()
            {
                DataReceivedEventArgs e = CreateMockDataReceivedEventArgs("Failed to register(user:True");
                OutputDataReceived.Invoke(this, e);
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
}
