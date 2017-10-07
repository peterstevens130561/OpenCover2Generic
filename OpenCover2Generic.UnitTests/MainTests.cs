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
        private IConverter converter;
        [TestMethod]
        public void EmptyModuleOnlyShouldCreateHeaderOnly()
        {
            converter = new Converter();

            MemoryStream resultStream = new MemoryStream();

            string result = WhenConverting(resultStream,"");
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<coverage version=""1"" />";
            Assert.AreEqual(expected, result);
        }

        private string WhenConverting(MemoryStream resultStream, string input)
        {
            StreamWriter writer = new StreamWriter(resultStream);
            Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            StreamReader reader = new StreamReader(inputStream);
            converter.Convert(writer, reader);
            StreamReader resultReader = new StreamReader(new MemoryStream(resultStream.ToArray()));
            string text = resultReader.ReadToEnd();
            return text;
        }
    }
}
