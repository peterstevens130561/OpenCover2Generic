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
            IBranchPointAggregator point = new BranchPointAggregator().Add(0, true);
            Assert.AreEqual(1, point.CoveredPaths());
            Assert.AreEqual(1, point.PathsToCover());
        }

        [TestMethod]
        public void PointWithTwoPathsTwoVisitedExpectTwoVisited()
        {
            IBranchPointAggregator point = new BranchPointAggregator().Add(0, true).Add(1, false);
            Assert.AreEqual(1, point.CoveredPaths());
            Assert.AreEqual(2, point.PathsToCover());
        }

        [TestMethod]
        public void PointWithThreeathsTwoVisitedExpectTwoVisited()
        {
            IBranchPointAggregator point = new BranchPointAggregator().Add(0, true).Add(1, false);
            point = point.Add(2,true); ;
            Assert.AreEqual(2, point.CoveredPaths());
            Assert.AreEqual(3, point.PathsToCover());
        }

        [TestMethod]
        public void SamePointReportedTwice()
        {
            IBranchPointAggregator firstPoint = new BranchPointAggregator().Add(0, true);
            IBranchPointAggregator resultPoint = firstPoint.Add(0,true);
            Assert.AreEqual(1, resultPoint.CoveredPaths(),"same path covered twice, should count as one");
            Assert.AreEqual(1, resultPoint.PathsToCover(),"same path, so expect one path");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceSame()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPointAggregator branchPoints = new BranchPointAggregator().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(2, branchPoints.CoveredPaths(), "2 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeMissingCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPointAggregator branchPoints = new BranchPointAggregator().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, false).Add(1, true).Add(2, false).Add(3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.CoveredPaths(), "4 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeAllCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPointAggregator branchPoints = new BranchPointAggregator().Add(0, true).Add(1, false).Add(2, true).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, true).Add(1, true).Add(2, true).Add(3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.CoveredPaths(), "4 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichNoneCoveredSeenTwiceScondTimeNoneCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPointAggregator branchPoints = new BranchPointAggregator().Add(0, false).Add(1, false).Add(2, false).Add(3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(0, false).Add(1, false).Add(2, false).Add(3, false);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(0, branchPoints.CoveredPaths(), "no branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void AddBranchPointToAggregator()
        {
            var branchPoint = new BranchPoint(1,1, false);
            var aggregator = new BranchPointAggregator();
            aggregator.Add(branchPoint);
            Assert.AreEqual(1, aggregator.PathsToCover());
            Assert.AreEqual(0, aggregator.CoveredPaths());

        }
    }
}
