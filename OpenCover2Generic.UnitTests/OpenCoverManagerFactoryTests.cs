using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            IOpenCoverManager openCoverManager = factory.CreateManager();
            Assert.IsNotNull(openCoverManager);
        }
    }
}
