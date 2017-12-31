
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System.IO;

/// <summary>
/// Simple logger which puts the test results in the format used by sonarqube
/// after building, copy the logger into C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\Extensions
/// the name of the resultsfile file is printed on the last line of the log, preceded by
/// VsTestSonarQubeLogger.TestResults=
/// 
/// See https://docs.sonarqube.org/display/PLUG/Generic+Test+Coverage
/// </summary>
namespace VsTestSonarQubeLogger
{
    [ExtensionUri("logger://VsTestSonarQubeLogger/v1")]
    [FriendlyName("VsTestSonarQubeLogger")]
    public class SonarQubeLogger : ITestLogger
    {

        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {

            Console.WriteLine("Initializing VsTestSonarQubeLogger");
            Console.WriteLine("testRunDirectory {0}", testRunDirectory);

            var testRunStarted = DateTime.Now;
            List<TestResult> testResults = new List<TestResult>();

            RegisterTestResult(events, testResults);
            RegisterTestRunMessage(events);
            RegisterTestRunComplete(events, testRunDirectory, testRunStarted, testResults);
        }

        private static void RegisterTestResult(TestLoggerEvents events, List<TestResult> testResults)
        {
            events.TestResult += (sender, eventArgs) =>
            {
                try
                {
                    if (eventArgs.Result.TestCase.CodeFilePath == null)
                    {
                        Console.Error.WriteLine($"Warning: no source found for ${eventArgs.Result.TestCase.DisplayName} in ${eventArgs.Result.TestCase.Source}");
                    }
                    testResults.Add(eventArgs.Result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
            };
        }

        private static void RegisterTestRunMessage(TestLoggerEvents events)
        {
            events.TestRunMessage += (sender, args) =>
            {
                if (args != null)
                {
                    Console.WriteLine(args.Message);
                }
            };
        }

        private static void RegisterTestRunComplete(TestLoggerEvents events, string testRunDirectory, DateTime testRunStarted, List<TestResult> testResults)
        {
            events.TestRunComplete += (sender, args) =>
            {
                try
                {
                    var outputWriter = new SonarQubeXmlWriter(testResults, args, testRunStarted);
                    string resultsPath = Path.Combine(testRunDirectory, $"{Guid.NewGuid()}.xml");
                    outputWriter.Write(resultsPath);
                    Console.WriteLine($"VsTestSonarQubeLogger.TestResults={resultsPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };
        }
    }
}
