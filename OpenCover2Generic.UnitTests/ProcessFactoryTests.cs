using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class ProcessFactoryTests
    {
        [TestMethod]
        public void CreateProcess_Instantiation_Valid() {
            IProcessAdapter processAdapter = new ProcessFactory().CreateProcess();
            Assert.IsNotNull(processAdapter,"Expect valid process");
        }
    }
}
