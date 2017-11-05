using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class BanchPointTests
    {
        [TestMethod]
        public void OneNotVisitedBranchPointShouldHaveOnePath() 
        {
            var point = new BranchPoint(1,1,1,false);
            Assert.AreEqual(1, point.Path, "expected path 1");
            Assert.AreEqual(false, point.IsVisited,"expected not visited");
        }

        [TestMethod]
        public void OneVisitedBranchPointShouldHaveOnePath()
        {
            var point = new BranchPoint(1,1, 2,true);
            Assert.AreEqual(2, point.Path,"expected path 2");
            Assert.AreEqual(true, point.IsVisited,"expected path visited");
        }

    }
}
