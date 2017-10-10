using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class BanchPointTests
    {
        [TestMethod]
        public void OneNotVisitedBranchPointShouldHaveOnePath() 
        {
            var point = new BranchPoint(false);
            Assert.AreEqual(1, point.Paths, "expected 1 path");
            Assert.AreEqual(0, point.PathsVisited,"expected 0 paths visited");
        }

        [TestMethod]
        public void OneVisitedBranchPointShouldHaveOnePath()
        {
            var point = new BranchPoint(true);
            Assert.AreEqual(1, point.Paths,"expected 1 path");
            Assert.AreEqual(1, point.PathsVisited,"expected 1 visited path");
        }

        [TestMethod]
        public void TwoBranchPointsOneVisitedShouldHaveTwoPathsOneCovered()
        {
            var point = new BranchPoint(false).Add(new BranchPoint(true));
            Assert.AreEqual(2, point.Paths,"expected 2 paths");
            Assert.AreEqual(1, point.PathsVisited,"expected 1 visited path");
        }

    }
}
