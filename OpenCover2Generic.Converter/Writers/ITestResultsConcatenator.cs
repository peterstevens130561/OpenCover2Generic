using System.IO;
using System.Xml;

namespace OpenCover2Generic.Converter
{
    internal interface ITestResultsConcatenator
    {
        XmlTextWriter Writer { get; set; }
        int TestCases { get; }

        void Begin();
        void End();
        void Concatenate(XmlReader xmlReader);
    }
}