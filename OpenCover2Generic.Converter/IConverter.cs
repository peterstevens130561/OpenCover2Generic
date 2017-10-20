using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface IConverter
    {
        void Convert(StreamWriter writer, StreamReader reader);
    }
}