using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class BranchPointsMultipeTimesCoveredTests
    {
        [TestMethod]
        public void PointWithPathVisitedExpectOneVisited()
        {
            IBranchPoints point = new BranchPoints().Add(1,0, true);
            Assert.AreEqual(1, point.CoveredPaths());
            Assert.AreEqual(1, point.PathsToCover());
        }

        [TestMethod]
        public void PointWithTwoPathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoints point = new BranchPoints().Add(1,0, true).Add(1,1, false);
            Assert.AreEqual(1, point.CoveredPaths());
            Assert.AreEqual(2, point.PathsToCover());
        }

        [TestMethod]
        public void PointWithThreeathsTwoVisitedExpectTwoVisited()
        {
            IBranchPoints point = new BranchPoints().Add(1,0, true).Add(1,1, false);
            point = point.Add(1,2,true); ;
            Assert.AreEqual(2, point.CoveredPaths());
            Assert.AreEqual(3, point.PathsToCover());
        }

        [TestMethod]
        public void SamePointReportedTwice()
        {
            IBranchPoints firstPoint = new BranchPoints().Add(1,0, true);
            IBranchPoints resultPoint = firstPoint.Add(1,0,true);
            Assert.AreEqual(1, resultPoint.CoveredPaths(),"same path covered twice, should count as one");
            Assert.AreEqual(1, resultPoint.PathsToCover(),"same path, so expect one path");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceSame()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoints branchPoints = new BranchPoints().Add(1,0, true).Add(1,1, false).Add(1,2, true).Add(1,3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(1,0, true).Add(1,1, false).Add(1,2, true).Add(1,3, false);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(2, branchPoints.CoveredPaths(), "2 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeMissingCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoints branchPoints = new BranchPoints()
                .Add(1,0, true).Add(1,1, false).Add(1,2, true).Add(1,3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(1,0, false).Add(1,1, true).Add(1,2, false).Add(1,3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.CoveredPaths(), "4 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void FourBranchPointsOfWhichTwoCoveredSeenTwiceScondTimeAllCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoints branchPoints = new BranchPoints().Add(1,0, true).Add(1,1, false).Add(1,2, true).Add(1,3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(1,0, true).Add(1,1, true).Add(1,2, true).Add(1,3, true);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(4, branchPoints.CoveredPaths(), "4 branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void Add_FourBranchPointsOfWhichNoneCoveredSeenTwiceScondTimeNoneCovered_FourPathsNotCovered()
        {
            //Given a first pass of 4 points, with 2 covered
            IBranchPoints branchPoints = new BranchPoints().Add(1,0, false).Add(1,1, false).Add(1,2, false).Add(1,3, false);
            // When a second pass, which has same coverage
            branchPoints.Add(1,0, false).Add(1,1, false).Add(1,2, false).Add(1,3, false);
            // Then the coverage should 4 branches, 2 covered
            Assert.AreEqual(0, branchPoints.CoveredPaths(), "no branches covered");
            Assert.AreEqual(4, branchPoints.PathsToCover(), "4 branches");
        }

        [TestMethod]
        public void Add_SinglePoint_SameValue()
        {
            var branchPoint = new BranchPoint(10,1,1, false);
            var aggregator = new BranchPoints();
            aggregator.Add(branchPoint);
            Assert.AreEqual(1, aggregator.PathsToCover());
            Assert.AreEqual(0, aggregator.CoveredPaths());

        }
    }
}
