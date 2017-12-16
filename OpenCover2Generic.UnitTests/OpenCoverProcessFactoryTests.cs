using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;
using BHGE.SonarQube.OpenCover2Generic.Seams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverProcessFactoryTests
    {
        [TestMethod]
        public void Create_FactoryExists_Create_ValidProcess()
        {
            IOpenCoverProcessFactory factory = new OpenCoverProcessFactory(new ProcessFactory());
            IOpenCoverProcess process = factory.Create();
            Assert.IsNotNull(process);
        }
    }
}
