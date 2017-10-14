using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class BranchPointsMultipeTimesCoveredTests
    {
        [TestMethod]
        public void TheUltimate()
        {
            IBranchPoint point = new BranchPoint(0, true);
            Assert.AreEqual(1, point.PathsVisited);

        }
    }
}
