using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class FileCoverageModelTests
    {
        [TestMethod]
        public void CreateSourceFileCoverageInfoCheckValues()
        {
            ISourceFileCoverageModel info = new SourceFileCoverageEntity("10", "a/b/c");
            Assert.AreEqual("10", info.Uid);
            Assert.AreEqual("a/b/c", info.FullPath);
        }
    }
}
