using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
{
    internal interface ITestResultsConcatenator
    {
        XmlTextWriter Writer { get; set; }
        int ExecutedTestCases { get; }

        void Begin();
        void End();
        void Concatenate(XmlReader xmlReader);
    }
}