using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class FileCoverageModelTests
    {
        [TestMethod]
        public void CreateSourceFileCoverageInfoCheckValues()
        {
            IFileCoverageModel info = new FileCoverageModel("10", "a/b/c");
            Assert.AreEqual("10", info.Uid);
            Assert.AreEqual("a/b/c", info.FullPath);
        }
    }
}
