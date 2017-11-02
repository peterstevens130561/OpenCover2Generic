using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class IntermediateModelTests
    {
        [TestMethod]
        public void AddOneFile()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            Assert.AreEqual("a", intermediateModel.GetCoverage()[0].FullPath);
        }

        [TestMethod]
        public void AddSecondFileWithPathAlreadyAdded()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.AddFile("2", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            Assert.AreEqual("a", intermediateModel.GetCoverage()[0].FullPath);
            Assert.AreEqual("1", intermediateModel.GetCoverage()[0].Uid);
        }

        [TestMethod]
        public void SecondFileWithPathAlreadyAddedAddSequencePoint()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.AddFile("2", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            intermediateModel.AddSequencePoint("2", "10", "1");
            Assert.AreEqual("a", intermediateModel.GetCoverage()[0].FullPath);
            Assert.AreEqual("1", intermediateModel.GetCoverage()[0].Uid);
            //Then the sequence point should be as if added to 1
            Assert.AreEqual(10, intermediateModel.GetCoverage()[0].SequencePoints[0].SourceLine);
            Assert.AreEqual(true, intermediateModel.GetCoverage()[0].SequencePoints[0].Covered);
        }

        [TestMethod]
        public void Swaps()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.AddFile("2", "b");
            intermediateModel.AddFile("2", "a");
            intermediateModel.AddFile("1", "b");
            Assert.AreEqual(2, intermediateModel.GetCoverage().Count);
            Assert.AreEqual("1", intermediateModel.GetCoverage().First(p => p.FullPath.Equals("a")).Uid);
            Assert.AreEqual("2", intermediateModel.GetCoverage().First(p => p.FullPath.Equals("b")).Uid);
        }

        [TestMethod]
        public void SecondFileWithPathAlreadyAddedAddBranchPoint()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.AddFile("2", "a");
            Assert.AreEqual(1, intermediateModel.GetCoverage().Count);
            intermediateModel.AddSequencePoint("2", "10", "1");
            intermediateModel.AddBranchPoint(2, 10, 1, true);
            intermediateModel.AddBranchPoint(2, 10, 2, false);
            //Then the sequence point should be as if added to 1
            Assert.AreEqual(2, intermediateModel.GetCoverage()[0].GetBranchPointAggregatorByLine("10").PathsToCover());
            Assert.AreEqual(1, intermediateModel.GetCoverage()[0].GetBranchPointAggregatorByLine("10").CoveredPaths());
        }
        [TestMethod]
        public void CheckClear()
        {
            var intermediateModel = new IntermediateModel();
            intermediateModel.AddFile("1", "a");
            intermediateModel.Clear();
            Assert.AreEqual(0, intermediateModel.GetCoverage().Count());
        }
    }
}
