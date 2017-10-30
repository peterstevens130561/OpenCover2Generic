﻿namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICommandLineParser
    {
        string[] Args { get; set; }

        /// <summary>
        /// Get the value of a command line option
        /// </summary>
        /// <param name="key">like -option</param>
        /// <returns></returns>
        string GetArgument(string key);
        /// <summary>
        /// Get the values of an array of values, either multiple time -option:value -option:value2 or -option:value1,value2
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] GetArgumentArray(string key);
    }
}