using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class FileCoverageModelTests
    {
        [TestMethod]
        public void CreateSourceFileCoverageInfoCheckValues()
        {
            ISourceFile info = new SourceFile("10", "a/b/c");
            Assert.AreEqual("10", info.Uid);
            Assert.AreEqual("a/b/c", info.FullPath);
        }
    }
}
