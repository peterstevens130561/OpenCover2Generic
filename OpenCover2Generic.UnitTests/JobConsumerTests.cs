using BHGE.SonarQube.OpenCover2Generic.Consumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCover2Generic.Converter;
using Moq;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using System.Collections.Concurrent;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobConsumerTests
    {
        private  IJobConsumer _jobConsumer;
        private Mock<IJobFileSystem> _jobFileSystemMock;
        private Mock<IOpenCoverManagerFactory> _openCoverManagerFactoryMock;
        private Mock<IOpenCoverCommandLineBuilder> _openCoverCommandLineBuilder;
        private IJobs jobs = new Jobs();

        [TestInitialize]
        public void Initialize()
        {
            _jobFileSystemMock = new Mock<IJobFileSystem>();
            _jobFileSystemMock.Setup(m => m.GetOpenCoverLogPath(It.IsAny<string>())).Returns(Path.GetTempFileName());
            _openCoverManagerFactoryMock = new Mock<IOpenCoverManagerFactory>();
            _openCoverManagerFactoryMock.Setup(o => o.CreateManager()).Returns(new Mock<IOpenCoverRunnerManager>().Object);
            _openCoverCommandLineBuilder = new Mock<IOpenCoverCommandLineBuilder>();
            _jobConsumer = new JobConsumer(_openCoverCommandLineBuilder.Object,_jobFileSystemMock.Object,_openCoverManagerFactoryMock.Object);
            
        }

        [TestMethod]
        public void ConsumeJobs_TwoInQueue_ExpectOnejobTakenTwoTimes()
        {
            jobs.Add(new Job("a"));
            jobs.Add(new Job("b"));
            jobs.CompleteAdding();
            _jobConsumer.ConsumeJobs(jobs);

            _openCoverCommandLineBuilder.Verify(v => v.Build("a",null),Times.Exactly(1));
            _openCoverCommandLineBuilder.Verify(v => v.Build("b", null), Times.Exactly(1));
        }

        [TestMethod]
        public void ConsumeJobs_TwoChunksOfTwoInQueue_ExpectOnejobTakenTwoTimes()
        {
            jobs.Add(new Job("a b"));
            jobs.Add(new Job("c d"));
            jobs.CompleteAdding();
            _jobConsumer.ConsumeJobs(jobs);

            _openCoverCommandLineBuilder.Verify(v => v.Build("a b", null), Times.Exactly(1));
            _openCoverCommandLineBuilder.Verify(v => v.Build("c d", null), Times.Exactly(1));
        }
    }
}
