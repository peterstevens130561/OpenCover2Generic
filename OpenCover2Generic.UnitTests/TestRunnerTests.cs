using BHGE.SonarQube.OpenCover2Generic.Consumer;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Repositories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenCover2Generic.Converter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestRunnerTests
    {
        private  Mock<IJobConsumerFactory> _jobConsumerFactoryMock;
        private Mock<IJobFileSystem> _jobFileSystemMock;
        private Mock<IOpenCoverCommandLineBuilder> _openCoverCommandLineBuilderMock;
        
        [TestInitialize]
        public void Initialize()
        {
            _jobConsumerFactoryMock = new Mock<IJobConsumerFactory>();
            _jobFileSystemMock = new Mock<IJobFileSystem>();
            _openCoverCommandLineBuilderMock = new Mock<IOpenCoverCommandLineBuilder>();
        }

        [TestMethod]
        public void RunTests_OneAssemblyFiveJobs()
        {
            _jobConsumerFactoryMock.Setup(j => j.Create()).Returns(new Mock<IJobConsumer>().Object);
            var testRunner = new TestRunner(_jobFileSystemMock.Object, null, _jobConsumerFactoryMock.Object);
            string[] testAssemblies = { "one" };
            testRunner.RunTests(testAssemblies, 5);
            _jobConsumerFactoryMock.Verify(f => f.Create(), Times.Exactly(5));
        }

        [TestMethod]
        public void CreateJobs_ChunkSize1_SameList()
        {
            _jobConsumerFactoryMock.Setup(j => j.Create()).Returns(new Mock<IJobConsumer>().Object);
            var testRunner = new TestRunner(_jobFileSystemMock.Object, null, _jobConsumerFactoryMock.Object);
            string[] testAssemblies = { "one","two","three" };
            testRunner.CreateJobs(testAssemblies, 1);
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one", jobs.Take().FirstAssembly);
            Assert.AreEqual("two", jobs.Take().FirstAssembly);
            Assert.AreEqual("three", jobs.Take().FirstAssembly);
        }

        [TestMethod]
        public void CreateJobs_ChunkSize2_SameList()
        {
            _jobConsumerFactoryMock.Setup(j => j.Create()).Returns(new Mock<IJobConsumer>().Object);
            var testRunner = new TestRunner(_jobFileSystemMock.Object, null, _jobConsumerFactoryMock.Object);
            string[] testAssemblies = { "one", "two", "three", "four" , "five" };
            testRunner.CreateJobs(testAssemblies, 2);
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one two", jobs.Take().Assemblies);
            Assert.AreEqual("three four", jobs.Take().Assemblies);
            Assert.AreEqual("five", jobs.Take().Assemblies);
        }

        [TestMethod]
        public void FirstAssembly_ChunkSize2_FirstInChunk()
        {
            _jobConsumerFactoryMock.Setup(j => j.Create()).Returns(new Mock<IJobConsumer>().Object);
            var testRunner = new TestRunner(_jobFileSystemMock.Object, null, _jobConsumerFactoryMock.Object);
            string[] testAssemblies = { "one", "two", "three", "four", "five" };
            testRunner.CreateJobs(testAssemblies, 2);
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one", jobs.Take().FirstAssembly);
            Assert.AreEqual("three", jobs.Take().FirstAssembly);
            Assert.AreEqual("five", jobs.Take().FirstAssembly);
        }

        [TestMethod]
        public void SetTimeOut_ProcessesRunTooLong_ProcessesKilled()
        {

            var processFactoryMock = new Mock<IProcessFactory>();
            var processMock = new Mock<Factories.IOpenCoverProcess>();
            processMock.Setup(p => p.HasExited).Returns(false);
            processMock.Setup(p => p.RecoverableError).Returns(false);
            processMock.Setup(p => p.TestResultsPath).Returns<string>(null);
            processFactoryMock.Setup(p => p.CreateOpenCoverProcess()).Returns(processMock.Object);

            _jobFileSystemMock.Setup(j => j.GetOpenCoverLogPath(It.IsAny<string>())).Returns(Path.GetTempFileName());
            _openCoverCommandLineBuilderMock.Setup(j => j.Build(It.IsAny<string>(), It.IsAny<string>())).Returns(new ProcessStartInfo());

            IJobConsumerFactory jobConsumerFactory = new JobConsumerFactory(_openCoverCommandLineBuilderMock.Object,
                _jobFileSystemMock.Object,
                new OpenCoverManagerFactory(processFactoryMock.Object),
                new TestResultsRepository(_jobFileSystemMock.Object, null)
                );
            ITestRunner testRunner = new TestRunner(_jobFileSystemMock.Object, null, jobConsumerFactory);

            string[] testAssemblies = { "one" };
            const int MS2TICK = 10000;
            TimeSpan timeOut = new TimeSpan(10 * MS2TICK); // after 10 ms timeout will occur

            testRunner.CreateJobs(testAssemblies, 1);
            testRunner.CreateJobConsumers(1,timeOut);
            try
            {
                testRunner.Wait();
            } catch ( JobTimeOutException e)
            {
                return;
            }
            Assert.Fail("Expected timeout");
        }
    }
}
