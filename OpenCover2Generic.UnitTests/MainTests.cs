using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EmptyModuleOnlyShouldCreateHeaderOnly()
        {
            IConverter converter = new Converter();
        }
    }
}
