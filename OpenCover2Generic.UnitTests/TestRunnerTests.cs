﻿using System;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestRunnerTests
    {
        private  Mock<IJobConsumerFactory> _jobConsumerFactoryMock;
        private Mock<IOpenCoverWrapperCommandLineParser> _commandLineParserMock;
        private Mock<IJobFileSystem> _jobFileSystemMock;
        
        [TestInitialize]
        public void Initialize()
        {
            _jobConsumerFactoryMock = new Mock<IJobConsumerFactory>();
            _commandLineParserMock= new Mock<IOpenCoverWrapperCommandLineParser>();
            _jobFileSystemMock = new Mock<IJobFileSystem>();

        }

        [TestMethod]
        public void RunTests_OneAssemblyFiveJobs()
        {
            var testRunner = CreateTestRunner();
            string[] testAssemblies = { "one" };
            ITestRunnerCommand command = new TestRunnerCommand();
            command.Workspace = new Workspace("bogus");
            _commandLineParserMock.Setup(c => c.GetTestAssemblies()).Returns(testAssemblies);
            _commandLineParserMock.Setup(c => c.GetParallelJobs()).Returns(5);
            _commandLineParserMock.Setup(c => c.GetChunkSize()).Returns(1);
            testRunner.Execute(command);
            _jobConsumerFactoryMock.Verify(f => f.Create(), Times.Exactly(5));
        }

        [TestMethod]
        public void CreateJobs_ChunkSize1_SameList()
        {
            var testRunner = CreateTestRunner();
            string[] testAssemblies = { "one","two","three" };
            testRunner.CreateJobs(testAssemblies, 1,null,new Workspace("a"));
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one", jobs.Take().FirstAssembly);
            Assert.AreEqual("two", jobs.Take().FirstAssembly);
            Assert.AreEqual("three", jobs.Take().FirstAssembly);
        }

        [TestMethod]
        public void CreateJobs_ChunkSize2_SameList()
        {
            var testRunner = CreateTestRunner();
            string[] testAssemblies = { "one", "two", "three", "four" , "five" };
            testRunner.CreateJobs(testAssemblies, 2,null, new Workspace("a"));
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one two", jobs.Take().Assemblies);
            Assert.AreEqual("three four", jobs.Take().Assemblies);
            Assert.AreEqual("five", jobs.Take().Assemblies);
        }

        [TestMethod]
        public void FirstAssembly_ChunkSize2_FirstInChunk()
        {
            var testRunner = CreateTestRunner();
            string[] testAssemblies = { "one", "two", "three", "four", "five" };
            testRunner.CreateJobs(testAssemblies, 2,null, new Workspace("a"));
            var jobs = testRunner.Jobs;
            Assert.AreEqual(3, jobs.Count());
            Assert.AreEqual("one", jobs.Take().FirstAssembly);
            Assert.AreEqual("three", jobs.Take().FirstAssembly);
            Assert.AreEqual("five", jobs.Take().FirstAssembly);
        }

        private TestRunnerCommandHandler CreateTestRunner()
        {
            _jobConsumerFactoryMock.Setup(j => j.Create()).Returns(new Mock<IJobConsumer>().Object);
            var testRunner = new TestRunnerCommandHandler(_jobFileSystemMock.Object,_jobConsumerFactoryMock.Object,_commandLineParserMock.Object);
            return testRunner;
        }
    }
}
