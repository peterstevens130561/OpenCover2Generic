using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
