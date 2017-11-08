using BHGE.SonarQube.OpenCover2Generic.Model.TestResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class TestClassesTests
    {

        [TestMethod]
        public void CreateClass()
        {
            var testClasses = new TestClasses();
            testClasses.AddTestClass("class1.cs");
            Assert.AreEqual(1, testClasses.Values.Count);
            Assert.AreEqual("class1.cs", testClasses.Values.First().Path);
        }

        [TestMethod]
        public void CreateSameClass()
        {
            var testClasses = new TestClasses();
            testClasses.AddTestClass("class1.cs");
            testClasses.AddTestClass("class1.cs");
            Assert.AreEqual(1, testClasses.Values.Count);
            Assert.AreEqual("class1.cs", testClasses.Values.First().Path);
        }
    }
}
