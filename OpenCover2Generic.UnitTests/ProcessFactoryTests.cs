using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    class ProcessFactoryTests
    {

        [TestMethod]
        public void InstantiationTest() {
            Process process = new ProcessFactory().CreateProcess();
        }

    }
}
