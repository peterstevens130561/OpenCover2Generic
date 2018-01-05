using BHGE.SonarQube.OpenCover2Generic.Model;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using BHGE.SonarQube.OpenCover2Generic.CoverageConverters;
namespace BHGE.SonarQube.OpenCover2Generic
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new OpenCover2GenericCommandLineParser(new CommandLineParser());
            var model = new ModuleCoverageEntity();
            var converter = new OpenCover2GenericConverter(model,new OpenCoverCoverageParser(),new GenericCoverageWriter());

            commandLineParser.Args = args;
            string openCoverPath = commandLineParser.OpenCoverPath();
            string genericPath = commandLineParser.GenericPath();
            var fileWriter = new StreamWriter(genericPath);
            var fileReader = new StreamReader(openCoverPath);

            converter.Convert(fileWriter, fileReader);
        }
    }
}
