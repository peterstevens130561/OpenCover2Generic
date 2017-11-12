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
    }
}
