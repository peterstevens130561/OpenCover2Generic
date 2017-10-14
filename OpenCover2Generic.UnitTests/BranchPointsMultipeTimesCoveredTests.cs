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
        }

        [TestMethod]
        public void PointWithThreePathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoint point = new BranchPoint(0, true).Add(new BranchPoint(1, false)).Add(new BranchPoint(2, true));
            Assert.AreEqual(2, point.PathsVisited);
        }
    }
}
