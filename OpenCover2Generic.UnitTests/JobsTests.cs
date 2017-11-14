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
    public class JobsTests
    {
        [TestMethod]
        public void Create_Valid()
        {
            IJobs jobs = new Jobs();
            Assert.IsNotNull(jobs);
        }
    }
}
