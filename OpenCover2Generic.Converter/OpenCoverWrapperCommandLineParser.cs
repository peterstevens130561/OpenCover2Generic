using System;
using System.Collections.ObjectModel;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.CoverageConverters.Exceptions;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    public class OpenCoverWrapperCommandLineParser : IOpenCoverWrapperCommandLineParser
    {
        private readonly ICommandLineParser _commandLineParser;

        public OpenCoverWrapperCommandLineParser(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }

        public string[] Args
        {
            get { return _commandLineParser.Args; }
            set { _commandLineParser.Args = value; }
        }

        public string GetOpenCoverPath()
        {
            return _commandLineParser.GetArgument("-opencover");
        }

        public string GetOutputPath()
        {
            return _commandLineParser.GetArgument("-output");
        }

        public int GetParallelJobs()
        {
            string value=_commandLineParser.GetOptionalArgument("-parallel","1");
            int jobs;
            if(!int.TryParse(value,out jobs) || jobs <1)
            {
                throw new CommandLineArgumentException($"-parallel:<positive int>, invalid:{value}");
            }
            return jobs;
        }

        public string GetTargetArgs()
        {
            return _commandLineParser.GetArgument("-targetargs");
        }

        public string GetTargetPath()
        {
            return _commandLineParser.GetArgument("-target");
        }

        public string[] GetTestAssemblies()
        {
            var assemblies= _commandLineParser.GetArgumentArray("-testassembly");
            var result = new Collection<String>();
            foreach (string assembly in assemblies)
            {
                if (assembly.Contains(" "))
                {
                    result.Add("\"" + assembly + "\"");
                } else
                {
                    result.Add(assembly);
                }
            }
            return result.ToArray();
        }

        public TimeSpan GetJobTimeOut()
        {
            int timeOut=_commandLineParser.GetOptionalPositiveInt("-jobtimeout", "0",1);
            return new TimeSpan(0, timeOut, 0);

        }

        public string GetTestResultsPath()
        {
            return _commandLineParser.GetArgument("-testresults");
        }

        public int GetChunkSize()
        {
            return _commandLineParser.GetOptionalPositiveInt("-chunksize", "1",1);
        }
    }
}
