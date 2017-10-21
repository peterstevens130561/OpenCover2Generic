using BHGE.SonarQube.OpenCover2Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new OpenCoverWrapperCommandLineParser(new CommandLineParser());
            var model = new Model();
            var converter = new Converter(model);

            commandLineParser.Args = args;
            
            string outputPath = commandLineParser.GetOutputPath();
            string targetPath = commandLineParser.GetTargetPath();
            string targetArgsPath = commandLineParser.GetTargetArgs();

        }
    }
}
