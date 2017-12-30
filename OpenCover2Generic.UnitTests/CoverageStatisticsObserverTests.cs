using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void GetLines_ModelWithThreeLines_GetLines_Three()
        {
            var model = new ModuleCoverageModel();
            model.AddFile("1","a");
            model.AddSequencePoint("1","1","1");
            model.AddFile("2","b");
            model.AddSequencePoint("2","1","1");
            model.AddSequencePoint("2","2","0");

            ModuleEventArgs moduleEventArgs = new ModuleEventArgs(model);
            ((IScannerObserver) observer).OnModule(this, moduleEventArgs);
        }
            
    }
}
