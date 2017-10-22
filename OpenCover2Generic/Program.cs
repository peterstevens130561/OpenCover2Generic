using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new OpenCover2GenericCommandLineParser(new CommandLineParser());
            var model = new Model();
            var converter = new Converter(model,new GenericCoverageWriter());

            commandLineParser.Args = args;
            string openCoverPath = commandLineParser.OpenCoverPath();
            string genericPath = commandLineParser.GenericPath();
            var fileWriter = new StreamWriter(genericPath);
            var fileReader = new StreamReader(openCoverPath);

            converter.Convert(fileWriter, fileReader);
        }
    }
}
