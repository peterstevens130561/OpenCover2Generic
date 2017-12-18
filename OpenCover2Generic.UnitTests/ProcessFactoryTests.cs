using BHGE.SonarQube.OpenCover2Generic.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

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
