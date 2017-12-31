using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System.Xml;
using System.IO;

namespace VsTestSonarQubeLogger
{
    internal class SonarQubeXmlWriter
    {
        private readonly List<TestResult> testResults;
        private XmlWriter xmlWriter;
        public SonarQubeXmlWriter(List<TestResult> testResults, TestRunCompleteEventArgs args, DateTime testRunStarted)
        {
            this.testResults = testResults;
  
        }

        internal void Write(string resultsPath)
        {

            xmlWriter = XmlWriter.Create(resultsPath);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("unitTest");
            xmlWriter.WriteAttributeString("version", "1");
            Dictionary<String, List<TestResult>> testResultsBySource = OrganizeTestResultsBySource();
            WriteTestResults(testResultsBySource);
        }

        private void WriteTestResults(Dictionary<string, List<TestResult>> testResultsBySource)
        {
            foreach (List<TestResult> source in testResultsBySource.Values)
            {
                WriteFile(source);

            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        private void WriteFile(List<TestResult> source)
        {
            xmlWriter.WriteStartElement("file");
            xmlWriter.WriteAttributeString("path", source[0].TestCase.CodeFilePath);
            foreach (TestResult testResult in source)
            {
                WriteTestCase(testResult);
            }
            xmlWriter.WriteEndElement();
        }

        private void WriteTestCase(TestResult testResult)
        {
            xmlWriter.WriteStartElement("testCase");
            xmlWriter.WriteAttributeString("name", testResult.TestCase.DisplayName);
            xmlWriter.WriteAttributeString("duration", testResult.Duration.Milliseconds.ToString());
            switch (testResult.Outcome)
            {
                case TestOutcome.Failed:

                    //<failure message="sort message">long stacktrace</failure>
                    //<error message="sort message">long stacktrace</error>
                    xmlWriter.WriteStartElement("error");
                    xmlWriter.WriteAttributeString("message", testResult.ErrorMessage);
                    xmlWriter.WriteValue(testResult.ErrorStackTrace);
                    xmlWriter.WriteEndElement();
                    break;
                case TestOutcome.Skipped:
                    xmlWriter.WriteStartElement("skipped");
                    xmlWriter.WriteAttributeString("message", testResult.ErrorMessage);
                    xmlWriter.WriteEndElement();
                    break;
            }
            xmlWriter.WriteEndElement();
        }

        private Dictionary<String, List<TestResult>> OrganizeTestResultsBySource()
        {
            int ignored = 0;
           var testResultsBySource = new Dictionary<String, List<TestResult>>(testResults.Count * 7);
            testResults.ForEach(result =>
            {
                string sourceFile = result.TestCase.CodeFilePath;
                if (sourceFile == null)
                {
                    Console.WriteLine($"Ignoring {result.TestCase.DisplayName} of {result.TestCase.Source} with {result.TestCase.FullyQualifiedName} on {result.TestCase.LineNumber}");
                    ++ignored;
                }
                else
                {
                    List<TestResult> resultsOfSource = GetSourceFilesTestResults(testResultsBySource, sourceFile);
                    EnsureResultIsUnique(resultsOfSource, result.TestCase);
                    resultsOfSource.Add(result);
                }
            });
            Console.WriteLine($"Ignored {ignored}");
            
            return testResultsBySource;
        }

        /// <summary>
        /// Data driven tests will come with the same name, so we need to make them unique
        /// </summary>
        /// <param name="resultsOfSource"></param>
        /// <param name="testCase"></param>
        private void EnsureResultIsUnique(List<TestResult> testResults, TestCase testCase)
        {
            TestResult duplicate;
            int count = 0;
            String name = testCase.DisplayName;
            duplicate = testResults.Find(t => t.TestCase.DisplayName.Equals(name));

            while (duplicate != null)
            {
                ++count;
                name = testCase.DisplayName + "_" + count;
                duplicate = testResults.Find(t => t.TestCase.DisplayName.Equals(name));
            }
            testCase.DisplayName = name;
        }

        private static List<TestResult> GetSourceFilesTestResults(Dictionary<string, List<TestResult>> testResultsBySource, string sourceFile)
        {
            if (!testResultsBySource.ContainsKey(sourceFile))
            {
                testResultsBySource.Add(sourceFile, new List<TestResult>());
            }
            var resultsOfSource = testResultsBySource[sourceFile];
            return resultsOfSource;
        }
    }
}