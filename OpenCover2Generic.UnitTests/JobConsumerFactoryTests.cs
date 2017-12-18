using BHGE.SonarQube.OpenCover2Generic.Repositories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCover2Generic.TestJobConsumer;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobConsumerFactoryTests
    {
        [TestMethod]
        public void Create_ExpectInstance()
        {
            IOpenCoverCommandLineBuilder openCoverCommandLineBuilder = null;
            IJobFileSystem jobFileSystem = null;
            IOpenCoverManagerFactory openCoverManagerFactory = null;
            ITestResultsRepository testResultsRepository = null;
            ICodeCoverageRepository codeCoverageRepository = null;
            IJobConsumerFactory factory = new JobConsumerFactory(openCoverCommandLineBuilder, jobFileSystem, 
                openCoverManagerFactory,
                testResultsRepository,
                codeCoverageRepository);

            IJobConsumer jobConsumer = factory.Create();
            Assert.IsNotNull(jobConsumer);
        }
    }
}
