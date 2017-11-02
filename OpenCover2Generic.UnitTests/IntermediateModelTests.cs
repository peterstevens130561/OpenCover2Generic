using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class IntermediateModelTests
    {
        [TestMethod]
        public void AddOneFile()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            Assert.AreEqual("a", intermediateModel.GetCoverage()[0].FullPath);
        }

        [TestMethod]
        public void AddSecondFileWithPathAlreadyAdded()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.AddFile("2", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            Assert.AreEqual("a", intermediateModel.GetCoverage()[0].FullPath);
            Assert.AreEqual("1", intermediateModel.GetCoverage()[0].Uid);
        }

    }
}
