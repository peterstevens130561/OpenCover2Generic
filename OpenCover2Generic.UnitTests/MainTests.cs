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

            MemoryStream resultStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(resultStream);
            string input = "";
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";

            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(""));
            StreamReader reader = new StreamReader(inputStream);
            converter.Convert(writer, reader);
            StreamReader resultReader = new StreamReader(new MemoryStream(resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            Assert.AreEqual(expected, text);
        }
    }
}
