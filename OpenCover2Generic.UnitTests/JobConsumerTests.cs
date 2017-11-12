using BHGE.SonarQube.OpenCover2Generic.Consumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    class JobConsumerTests
    {
        private  IJobConsumer _jobConsumer;
        [TestInitialize]
        public void Initialize()
        {
            _jobConsumer = new JobConsumer();
            
        }

        [TestMethod]
        public void 
    }
}
