using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverManagerFactoryTests
    {
        [TestMethod]
        public void Create_ShouldCreateOpenCoverManagerInstance()
        {
            
            IOpenCoverManagerFactory factory = new OpenCoverManagerFactory(new ProcessFactory());
            OpenCoverRunnerManager openCoverManager = factory.CreateManager();
            Assert.IsNotNull(openCoverManager);
        }
    }
}
