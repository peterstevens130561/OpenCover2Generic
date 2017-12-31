using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CreateGenericCoverageFileObserverTests
    {
        private IGenericCoverageWriterObserver _observer;
        private Mock<ICoverageWriter> _coverageWriterMock;
        [TestInitialize]
        public void Initialize()
        {
            _coverageWriterMock=new Mock<ICoverageWriter>();
            _observer = new GenericCoverageWriterObserver(_coverageWriterMock.Object);
        }

        [TestMethod]
        public void Begin_WriterSet_Begin_WriteBegin()
        {
            _observer.Writer = null;
            ((IQueryAllModulesResultObserver)_observer).OnBeginScan(null,EventArgs.Empty);
            _coverageWriterMock.Verify(o => o.WriteBegin(null), Times.Once);
            _coverageWriterMock.Verify(o => o.WriteEnd(null), Times.Never);
        }

        [TestMethod]
        public void End_WriterSet_End_WriteEnd()
        {
            _observer.Writer = null;
            ((IQueryAllModulesResultObserver)_observer).OnEndScan(null, EventArgs.Empty);
            _coverageWriterMock.Verify(o => o.WriteEnd(null), Times.Once);
            _coverageWriterMock.Verify(o => o.WriteBegin(null), Times.Never);
        }

        [TestMethod]
        public void Module_WriterSet_Module_Header()
        {
            _observer.Writer = null;
            IntermediateModel model = new IntermediateModel();
            ModuleEventArgs eventArgs = new ModuleEventArgs(model);
            ((IQueryAllModulesResultObserver)_observer).OnModule(null, eventArgs);
            _coverageWriterMock.Verify(o => o.GenerateCoverage(model,null), Times.Once);

        }
    }
}
