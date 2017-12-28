using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class CreateGenericCoverageFileObserverTests
    {
        private IGenericCoverageWriterObserver observer;
        private Mock<ICoverageWriter> coverageWriterMock;
        private Mock<XmlTextWriter> xmlTextWriterMock;
        [TestInitialize]
        public void Initialize()
        {
            coverageWriterMock=new Mock<ICoverageWriter>();
            xmlTextWriterMock = new Mock<XmlTextWriter>();
            observer = new GenericCoverageWriterObserver(coverageWriterMock.Object);
        }

        [TestMethod]
        public void Begin_WriterSet_Begin_Header()
        {
            observer.Writer = null;
            ((IScannerObserver)observer).OnBeginScan(null,EventArgs.Empty);
            coverageWriterMock.Verify(o => o.WriteBegin(null), Times.Once);
            coverageWriterMock.Verify(o => o.WriteEnd(null), Times.Never);
        }

        [TestMethod]
        public void End_WriterSet_End_Header()
        {
            observer.Writer = null;
            ((IScannerObserver)observer).OnEndScan(null, EventArgs.Empty);
            coverageWriterMock.Verify(o => o.WriteEnd(null), Times.Once);
            coverageWriterMock.Verify(o => o.WriteBegin(null), Times.Never);
        }
    }
}
