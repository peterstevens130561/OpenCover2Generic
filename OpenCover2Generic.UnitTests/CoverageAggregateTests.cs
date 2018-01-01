using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CoverageAggregateTests
    {
        private Mock<IOpenCoverageParserFactory> _coverageParserFactoryMock;
        [TestInitialize]
        public void Initialize()
        {
            _coverageParserFactoryMock = new Mock<IOpenCoverageParserFactory>();
        }

        [TestMethod]
        public void Create_ValidObject()
        {
            string path = "my.xml";
            string key = "key";

            var aggregate = CreateCoverageAggregate(path, key);

            Assert.AreEqual("my.xml", aggregate.Path);
            Assert.AreEqual("key",aggregate.Key);
        }



        [TestMethod]
        public void Modules_NoModules_Modules_NotCalled()
        {
            Mock<Action<IntermediateModel>> intermediateModelMock = new Mock<Action<IntermediateModel>>();
            string path = "my.xml";
            string key = "key";

            var aggregate = CreateCoverageAggregate(path, key);

            aggregate.Modules(intermediateModelMock.Object);
            intermediateModelMock.Verify(p => p(It.IsAny<IntermediateModel>()), Times.Never);
        }


        private ICoverageAggregate CreateCoverageAggregate(string path, string key)
        {
            ICoverageAggregate aggregate = new CoverageAggregate(path, key, _coverageParserFactoryMock.Object);
            return aggregate;
        }
    }
}
