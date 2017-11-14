using BHGE.SonarQube.OpenCover2Generic.Consumer;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenCover2Generic.Converter;
using System;
using System.Collections.Generic;
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
        [TestInitialize]
        public void Initialize()
        {
            _jobConsumerFactoryMock = new Mock<IJobConsumerFactory>();
            _jobFileSystemMock = new Mock<IJobFileSystem>();
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
    }
}
