using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic;
using System.IO;
using System.Text;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EmptyModuleOnlyShouldCreateHeaderOnly()
        {
            IConverter converter = new Converter();
            string result = "";
            StreamWriter writer = new StreamWriter(result, true);
            string input = "";
            string expected = @"<coverage version=""1"">
</coverage>";

            StreamReader reader = new StreamReader(input);
            converter.Convert(writer, reader);
            Assert.AreEqual(expected, result);
        }
    }
}
