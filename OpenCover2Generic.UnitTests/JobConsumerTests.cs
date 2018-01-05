using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobConsumerTests
    {
        private  IJobConsumer _jobConsumer;
        private Mock<IJobFileSystem> _jobFileSystemMock;
        private Mock<IOpenCoverManagerFactory> _openCoverManagerFactoryMock;
        private Mock<IOpenCoverCommandLineBuilder> _openCoverCommandLineBuilder;
        private Mock<ITestResultsRepository> _testResultsRepositoryMock;
        private Mock<ICoverageAggregateFactory> _coverageAggregateFactoryMock;
      
        private readonly IJobs _jobs = new Jobs();
        private TimeSpan _jobTimeOut;
        private Mock<ICodeCoverageRepository> _codeCoverageRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _jobFileSystemMock = new Mock<IJobFileSystem>();
            _jobFileSystemMock.Setup(m => m.GetOpenCoverLogPath(It.IsAny<string>())).Returns(Path.GetTempFileName());
            _openCoverManagerFactoryMock = new Mock<IOpenCoverManagerFactory>();
            _openCoverManagerFactoryMock.Setup(o => o.CreateManager()).Returns(new Mock<IOpenCoverManager>().Object);
            _openCoverCommandLineBuilder = new Mock<IOpenCoverCommandLineBuilder>();
            _testResultsRepositoryMock = new Mock<ITestResultsRepository>();
            _codeCoverageRepositoryMock = new Mock<ICodeCoverageRepository>();
            _coverageAggregateFactoryMock = new Mock<ICoverageAggregateFactory>();

            _jobConsumer = new JobConsumer(_openCoverCommandLineBuilder.Object,
                _jobFileSystemMock.Object,_openCoverManagerFactoryMock.Object,
                _testResultsRepositoryMock.Object,
                _codeCoverageRepositoryMock.Object,
                _coverageAggregateFactoryMock.Object);
            _jobTimeOut = new TimeSpan(0);
        }

        [TestMethod]
        public void ConsumeJobs_TwoInQueue_ExpectOnejobTakenTwoTimes()
        {
            _jobs.Add(new TestJob(@"a"));
            _jobs.Add(new TestJob(@"b"));
            _jobs.CompleteAdding();

            WhenConsumingJobs();

            _openCoverCommandLineBuilder.Verify(v => v.Build(@"a", null), Times.Exactly(1));
            _openCoverCommandLineBuilder.Verify(v => v.Build(@"b", null), Times.Exactly(1));
        }



        [TestMethod]
        public void ConsumeJobs_TwoChunksOfTwoInQueue_ExpectOnejobTakenTwoTimes()
        {
            _jobs.Add(new TestJob("a b"));
            _jobs.Add(new TestJob("c d"));
            _jobs.CompleteAdding();

            WhenConsumingJobs();

            _openCoverCommandLineBuilder.Verify(v => v.Build("a b", null), Times.Exactly(1));
            _openCoverCommandLineBuilder.Verify(v => v.Build("c d", null), Times.Exactly(1));
        }

        private void WhenConsumingJobs()
        {
            _jobConsumer.ConsumeTestJobs(_jobs, _jobTimeOut);
        }
    }
}
