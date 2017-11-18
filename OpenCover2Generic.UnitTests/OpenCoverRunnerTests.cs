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

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverRunnerTests
    {
        [TestMethod]
        public void Run_CouldNotRegisterOnFirst_OkOnSecond()
        {
            //given a valid runner which will not register on starting, but will on second
            Mock<IProcessFactory> processFactoryMock = new Mock<IProcessFactory>();
            Mock<Timer> timerMock = new Mock<Timer>();
            var openCoverProcessMock = new Mock<IOpenCoverProcess>();
            processFactoryMock.Setup(p => p.CreateOpenCoverProcess()).Returns(openCoverProcessMock.Object);
            openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Failed to register(user:True"));

            // as the opencoverprocess is mocked, we need to set its property value
            openCoverProcessMock.SetupSequence(p => p.RecoverableError).Returns(true).Returns(false);
            openCoverProcessMock.SetupSequence(p => p.Started).Returns(false).Returns(true);
            openCoverProcessMock.Setup(p => p.Start())
                           .Raises(p => p.DataReceived += null, new Queue<DataReceivedEventArgs>(new[] {
                                CreateMockDataReceivedEventArgs("Failed to register(user:True"),
                                CreateMockDataReceivedEventArgs("Starting test execution, please wait..")
                           }).Dequeue);

            IProcessFactory processFactory = processFactoryMock.Object;

            var testRunner = new OpenCoverRunner.OpenCoverRunnerManager(processFactory,timerMock.Object);
            ProcessStartInfo info = new ProcessStartInfo();
            
            //when starting the runner

                using (StreamWriter writer = new StreamWriter(new MemoryStream()))
                {
                    testRunner.Run(info, writer);
                }
            // should be called twice
            openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(2));
        }
        [TestMethod]
        public void Run_CouldNotRegisterAtAll_InvalidOperationExceptionThrown()
        {
            //given a valid runner which will not register on starting, but will on second
            Mock<IProcessFactory> processFactoryMock = new Mock<IProcessFactory>();
            var openCoverProcessMock = new Mock<IOpenCoverProcess>();
            processFactoryMock.Setup(p => p.CreateOpenCoverProcess()).Returns(openCoverProcessMock.Object);
            openCoverProcessMock.Setup(p => p.HasExited).Returns(true);
            openCoverProcessMock.Setup(p => p.Start())
                .Raises(p => p.DataReceived += null, CreateMockDataReceivedEventArgs("Failed to register(user:True"));

            // as the opencoverprocess is mocked, we need to set its property value
            openCoverProcessMock.SetupSequence(p => p.RecoverableError)
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

            IProcessFactory processFactory = processFactoryMock.Object;
            var timerMock = new Mock<Timer>();
            var testRunner = new OpenCoverRunner.OpenCoverRunnerManager(processFactory,timerMock.Object);
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
                openCoverProcessMock.Verify(p => p.Start(), Times.Exactly(10));
                return;
            }
            Assert.Fail("expected InvalidOperatonException");

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

                if (EventFields.Any())
                {
                    EventFields[0].SetValue(MockEventArgs, TestData);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Failed to find _data field!");
                }

                return MockEventArgs;
            }
        }
    }
