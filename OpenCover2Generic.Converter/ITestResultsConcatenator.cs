using System.IO;
using System.Xml;

namespace OpenCover2Generic.Converter
{
    internal interface ITestResultsConcatenator
    {
        XmlTextWriter Writer { get; set; }

        void Begin();
        void End();
    }
}