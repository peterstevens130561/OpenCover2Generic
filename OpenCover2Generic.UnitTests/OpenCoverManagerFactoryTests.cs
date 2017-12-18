using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverManagerFactoryTests
    {
        [TestMethod]
        public void Create_ShouldCreateOpenCoverManagerInstance()
        {
            
            IOpenCoverManagerFactory factory = new OpenCoverManagerFactory(new OpenCoverProcessFactory(new ProcessFactory()));
            IOpenCoverRunnerManager openCoverManager = factory.CreateManager();
            Assert.IsNotNull(openCoverManager);
        }
    }
}
