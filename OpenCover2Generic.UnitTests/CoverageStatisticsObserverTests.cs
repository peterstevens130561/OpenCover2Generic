using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageStatisticsObserverTests
    {
        private ICoverageStatisticsAggregator observer;
        [TestInitialize]
        public void Initialize()
        {
            observer = new CoverageStatisticsAggregator();
        }

        [TestMethod]
        public void GetFiles_NoInformation_GetLines_Zero()
        {
            Assert.AreEqual(0,observer.Lines);
        }

        [TestMethod]
        public void Lines_ModelWithThreeLines_Lines_Three()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();

            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);
            Assert.AreEqual(3, observer.Lines);
        }

        [TestMethod]
        public void Lines_ModelWithThreeLinesCalledTwice_Lines_Six()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();
            WhenObservingModule(moduleEventArgs);
            WhenObservingModule(moduleEventArgs);
            Assert.AreEqual(6, observer.Lines);
        }



        [TestMethod]
        public void CoveredLines_ModelWithThreeLines_CoveredLines_Two()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();

            WhenObservingModule(moduleEventArgs);
            Assert.AreEqual(2, observer.CoveredLines);
        }

        [TestMethod]
        public void CoveredLines_ModelWithThreeLinesObservedTiwce_CoveredLines_Two()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();

            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);
            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);

            Assert.AreEqual(4, observer.CoveredLines);
        }

        [TestMethod]
        public void Files_ModelWithThreeLines_Files_Two()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();

            WhenObservingModule(moduleEventArgs);
            Assert.AreEqual(2, observer.Files);
        }

        [TestMethod]
        public void Files_ModelWithThreeLinesObservedTiwce_Files_Four()
        {
            ModuleEventArgs moduleEventArgs = GivenTwoFilesWithThreeLinesAndTwoCovered();

            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);
            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);

            Assert.AreEqual(4, observer.Files);
        }
        private ModuleEventArgs GivenTwoFilesWithThreeLinesAndTwoCovered()
        {
            var model = new ModuleCoverageEntity();
            model.AddFile("1", "a");
            model.AddSequencePoint("1", "1", "1");
            model.AddFile("2", "b");
            model.AddSequencePoint("2", "1", "1");
            model.AddSequencePoint("2", "2", "0");
            ModuleEventArgs moduleEventArgs = new ModuleEventArgs(model);
            return moduleEventArgs;
        }

        private void WhenObservingModule(ModuleEventArgs moduleEventArgs)
        {
            ((IQueryAllModulesResultObserver)observer).OnModule(this, moduleEventArgs);
        }
    }
}
