using BHGE.SonarQube.OpenCover2Generic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class JobTests
    {
        [TestMethod]
        public void GetAssembly_ValueAtInstantiation()
        {
            ITestJob testJob = new TestTestJob("my assembly");
            Assert.AreEqual("my assembly", testJob.FirstAssembly);
        }
    }
}
