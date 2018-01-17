using System;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CodeCoverageRepositoryTests
    {
        private ICodeCoverageRepository _repository;
        private Mock<ICoverageStorageResolver> _coverageStorageResolverMock;
        private ICoverageParser _coverageParser;
        private Mock<ICoverageAggregate> _aggregateMock;
        private Module _module;
        private Mock<IXmlAdapter> _xmlAdapterMock;
        private Mock<ICoverageWriterFactory> _coverageWriterFactoryMock;
        private Mock<ICoverageWriter> _coverageWriterMock;

        [TestInitialize]
        public void Initialize()
        {
            _coverageWriterFactoryMock = new Mock<ICoverageWriterFactory>();
            _coverageWriterMock=new Mock<ICoverageWriter>();
            _aggregateMock = new Mock<ICoverageAggregate>();
            _xmlAdapterMock = new Mock<IXmlAdapter>();
            _coverageStorageResolverMock = new Mock<ICoverageStorageResolver>();
            _repository = new CodeCoverageRepository(_coverageStorageResolverMock.Object,
                _coverageParser,
                _xmlAdapterMock.Object,
                _coverageWriterFactoryMock.Object);

            _module = new Module();
            _module.NameId = "module";
            _repository.Workspace =new Workspace("workspace");
            _repository.Directory = @"workspace\repository";

            _aggregateMock
                .Setup(p => p.Modules(It.IsAny<Action<IModule>>()))
                .Callback<Action<IModule>>(q =>
                {
                    q.Invoke(_module);
                });
            _coverageStorageResolverMock.Setup(c => c.GetPathForAssembly("root", "module", It.IsAny<string>())).Returns("bla");
            _coverageWriterFactoryMock.Setup(c => c.CreateOpenCoverCoverageWriter()).Returns(_coverageWriterMock.Object);
        }
        [TestMethod]
        public void Add_EmptyModel_Add_Skipped()
        {

           _repository.Save(_aggregateMock.Object);
            _coverageStorageResolverMock.Verify(c => c.GetPathForAssembly(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _coverageStorageResolverMock.VerifySet(c => c.Root = "workspace", Times.Once);
        }

        [TestMethod]
        public void Add_EmptyModelWithOneSourceFile_Add_DefinedPath()
        {
            _module.AddFile("10","b");

            _repository.Save(_aggregateMock.Object);

            _coverageStorageResolverMock.Verify(c => c.GetPathForAssembly( It.IsAny<string>(), It.IsAny<string>()), Times.Once);
           _coverageWriterMock.Verify(c => c.GenerateCoverage(_module,null),Times.Once);
        }
    }
}
