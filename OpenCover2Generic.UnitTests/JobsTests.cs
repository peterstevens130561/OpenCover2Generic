using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;

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
        public void Take_OneJob_Successful()
        {
            IJobs jobs = new Jobs();
            jobs.Add(new TestJob("a"));
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
