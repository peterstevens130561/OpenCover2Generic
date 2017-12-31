using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageAggregateTests 
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void Create_ValidObject()
        {
            string path = "my.xml";
            string key = "key";

            ICoverageAggregate aggregate = new CoverageAggregate(path, key);

            Assert.AreEqual("my.xml", aggregate.Path);
            Assert.AreEqual("key",aggregate.Key);
        }

    }
}
