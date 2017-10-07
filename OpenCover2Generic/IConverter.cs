using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface IConverter
    {
        void Convert(StreamWriter writer, StreamReader reader);
    }
}