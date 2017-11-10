using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class ProcessFactoryTests
    {

        [TestMethod]
        public void CreateProcess_Instantiation_Valid() {
            IProcess process = new ProcessFactory().CreateProcess();
            Assert.IsNotNull(process,"Expect valid process");
        }

        [TestMethod]
        public void CreateOpenCoverProcess_Instantiation_Valid()
        {
            IOpenCoverProcess process = new ProcessFactory().CreateOpenCoverProcess();
            Assert.IsNotNull(process, "Expect valid process");
        }

    }
}
