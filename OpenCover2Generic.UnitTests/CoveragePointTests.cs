using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoveragePointTests
    {
        [TestMethod]
        public void WeirdOrderShouldBeSorted()
        {
            var model = new FileCoverageModel("a/b");
            model.AddSequencePoint("3", "0");
            model.AddSequencePoint("2", "1");
            model.AddSequencePoint("5", "2");
            var points = model.SequencePoints;
            Assert.AreEqual(2, points[0].SourceLine);
            Assert.AreEqual(3, points[1].SourceLine);
            Assert.AreEqual(5, points[2].SourceLine);

        }

        [TestMethod]
        public void LinesReportedTwiceCovered()
        {
            var point = new CoveragePoint("10", false);
            point.Add(true);
            Assert.AreEqual(true, point.Covered);
        }

        [TestMethod]
        public void LinesReportedTwiceNotCovered()
        {
            var point = new CoveragePoint("10", false);
            point.Add(false);
            Assert.AreEqual(false, point.Covered);
        }
    }
}
