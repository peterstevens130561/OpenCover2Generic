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

        [TestMethod]
        public void Add_CanTake()
        {
            IJobs jobs = new Jobs();
            jobs.Add(new Job("a"));
            var assembly = jobs.Take().FirstAssembly;
            Assert.AreEqual("a", assembly);
        }

        [TestMethod]
        public void CompleteAdding_Take_ExpectException()
        {
            IJobs jobs = new Jobs();
            jobs.CompleteAdding();
            try
            {
                var assembly = jobs.Take().FirstAssembly;
            } catch (InvalidOperationException)
            {
                return;
            }
            Assert.Fail("Expected exception");
        }
    }
}
