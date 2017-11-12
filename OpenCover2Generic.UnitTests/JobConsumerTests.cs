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

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    class JobConsumerTests
    {
        private  IJobConsumer _jobConsumer;
        private Mock<JobFileSystem> _jobFileSystemMock;
        private Mock<ProcessFactory> _processFactoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _jobFileSystemMock = new Mock<JobFileSystem>();
            _processFactoryMock = new Mock<ProcessFactory>();
            _jobConsumer = new JobConsumer(_jobFileSystemMock.Object,_processFactoryMock.Object);
            
        }

        [TestMethod]
        public void Consume_Chunk2ThreeInQueue_ExpectTwoJobsTaken()
        {

        }
    }
}
