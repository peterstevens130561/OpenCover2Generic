using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.CoverageConverters
{
    public interface ICoverageConverter
    {
        void Convert(StreamWriter writer, StreamReader reader);
    }
}