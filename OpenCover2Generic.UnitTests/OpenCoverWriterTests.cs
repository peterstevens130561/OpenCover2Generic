using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;
using BHGE.SonarQube.OpenCoverWrapper;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    class OpenCoverWriterTests
    {
        [TestMethod]
        public void CheckWriteEmptyFile()
        {
            ICoverageWriter writer = new OpenCoverCoverageWriter();
        }
    }
}
