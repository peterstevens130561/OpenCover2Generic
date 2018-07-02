using System;
using System.Diagnostics;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.TestJobConsumer
{
    /// <summary>
    /// builds the commandline for OpenCover, taking the commandline supplied args, and supplemental from the
    /// application. 
    /// </summary>
    public class OpenCoverCommandLineBuilder : IOpenCoverCommandLineBuilder
    {

        private readonly ICommandLineParser _commandLineParser;

        public OpenCoverCommandLineBuilder() : this(new CommandLineParser())
        {
            
        }
        public OpenCoverCommandLineBuilder(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }

        public string[] Args
        {
            get
            {
                return _commandLineParser.Args;
            }
            set
            {
                if(value==null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _commandLineParser.Args = value;
            }
        }

        ProcessStartInfo IOpenCoverCommandLineBuilder.Build(string assemblyPath, string outputPath)
        {
            string openCoverExePath = _commandLineParser.GetArgument("-opencover");
            string targetPath = _commandLineParser.GetArgument("-target");
            string targetArgs = _commandLineParser.GetArgument("-targetargs");
            string arguments = $"-register:user \"-output:{outputPath}\" \"-target:{targetPath}\" \"-targetargs:{targetArgs} {assemblyPath}\"";
            return new ProcessStartInfo(openCoverExePath, arguments);
        }
    }
}
