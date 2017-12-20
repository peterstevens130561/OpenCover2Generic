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
        public void FirstAssembly_TwoAssemblies_FirstAssembly_ShouldBeFirstValue()
        {
            var assemblies = new List<string>();
            assemblies.Add("\"my assembly\"");
            assemblies.Add("second");
            ITestJob testJob = new TestJob(assemblies);
            Assert.AreEqual("\"my assembly\"", testJob.FirstAssembly);
        }

        public void FirstAssembly_TwoAssemblies_AssembliesIsConcatenatedWithSpace()
        {
            var assemblies = new List<string>();
            assemblies.Add("\"my assembly\"");
            assemblies.Add("second");
            ITestJob testJob = new TestJob(assemblies);
            Assert.AreEqual("\"my assembly\" second", testJob.Assemblies);
        }
    }
}
