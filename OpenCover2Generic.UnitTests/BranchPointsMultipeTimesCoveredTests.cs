using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class BranchPointsMultipeTimesCoveredTests
    {
        [TestMethod]
        public void PointWithPathVisitedExpectOneVisited()
        {
            IBranchPoint point = new BranchPoint(0, true);
            Assert.AreEqual(1, point.PathsVisited);
            Assert.AreEqual(1, point.Paths);
        }

        [TestMethod]
        public void PointWithTwoPathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoint point = new BranchPoint(0, true).Add(new BranchPoint(1, false));
            Assert.AreEqual(1, point.PathsVisited);
            Assert.AreEqual(2, point.Paths);
        }

        [TestMethod]
        public void PointWithThreePathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoint point = new BranchPoint(0, true).Add(new BranchPoint(1, false));
            var newPoint = new BranchPoint(1, true);
            point = point.Add(newPoint); ;
            Assert.AreEqual(2, point.PathsVisited);
            Assert.AreEqual(3, point.Paths);
        }
    }
}
