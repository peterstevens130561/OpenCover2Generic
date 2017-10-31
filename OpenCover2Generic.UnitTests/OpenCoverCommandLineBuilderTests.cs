using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class OpenCoverCommandLineBuilderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IOpenCoverCommandLineBuilder builder = new OpenCoverCommandLineBuilder(ICommandLineParser commandLineParser);

        }
    }
}
