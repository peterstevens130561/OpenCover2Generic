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
            IBranchPoint point = new TrackingBranchPoint().Add(0, true).Add(1, false);
            Assert.AreEqual(1, point.PathsVisited);
            Assert.AreEqual(2, point.Paths);
        }

        [TestMethod]
        public void PointWithThreeathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoint point = new TrackingBranchPoint().Add(0, true).Add(1, false);
            point = point.Add(2,true); ;
            Assert.AreEqual(2, point.PathsVisited);
            Assert.AreEqual(3, point.Paths);
        }

        [TestMethod]
        public void SamePointReportedTwice()
        {
            IBranchPoint firstPoint = new TrackingBranchPoint().Add(0, true);
            IBranchPoint resultPoint = firstPoint.Add(0,true);
            Assert.AreEqual(1, resultPoint.PathsVisited,"same path covered twice, should count as one");
            Assert.AreEqual(1, resultPoint.Paths,"same path, so expect one path");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceSame()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoint branchPoints = new TrackingBranchPoint().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(2, branchPoints.PathsVisited, "2 branches covered");
            Assert.AreEqual(4, branchPoints.Paths, "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeMissingCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoint branchPoints = new TrackingBranchPoint().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, false).Add(1, true).Add(2, false).Add(3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.PathsVisited, "4 branches covered");
            Assert.AreEqual(4, branchPoints.Paths, "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeAllCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoint branchPoints = new TrackingBranchPoint().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, true).Add(1, true).Add(2, true).Add(3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.PathsVisited, "4 branches covered");
            Assert.AreEqual(4, branchPoints.Paths, "4 branches");
        }
    }
}
