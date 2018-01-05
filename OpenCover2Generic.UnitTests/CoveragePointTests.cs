using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoveragePointTests
    {
        [TestMethod]
        public void WeirdOrderShouldBeSorted()
        {
            var model = new SourceFileCoverageAggregate("10","a/b");
            model.AddSequencePoint("3", "0");
            model.AddSequencePoint("2", "1");
            model.AddSequencePoint("5", "2");
            var points = model.SequencePoints;
            Assert.AreEqual(2, points[0].SourceLineId);
            Assert.AreEqual(3, points[1].SourceLineId);
            Assert.AreEqual(5, points[2].SourceLineId);

        }
    }
}
