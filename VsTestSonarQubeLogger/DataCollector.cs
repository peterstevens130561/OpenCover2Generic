using System.IO;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;

namespace VsTestSonarQubeLogger
{
    [DataCollectorFriendlyName("NewDataCollector")]
    [DataCollectorTypeUri("my://new/datacollector")]
    public class NewDataCollector : DataCollector
    {
        private string logFileName;
        private DataCollectionEnvironmentContext context;
        private DataCollectionSink _dataSink;
        private DataCollectionLogger _logger;

        public override void Initialize(
            System.Xml.XmlElement configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext environmentContext)
        {
            _dataSink = dataSink;
            _logger = logger;
            events.SessionStart += this.SessionStarted_Handler;
            events.TestCaseStart += this.Events_TestCaseStart;
            events.TestCaseEnd += this.Events_TestCaseEnd;

            logFileName = configurationElement["LogFileName"]?.InnerText;
        }

        private void SessionStarted_Handler(object sender, SessionStartEventArgs args)
        {

            _logger.LogWarning(this.context.SessionDataCollectionContext,"SessionStarted" + args.Context.SessionId.Id);
        }


        private void Events_TestCaseStart(object sender, TestCaseStartEventArgs e)
        {
            _logger.LogWarning(this.context.SessionDataCollectionContext, "TestCaseStarted " + e.TestCaseName);
        }

        private void Events_TestCaseEnd(object sender, TestCaseEndEventArgs e)
        {
            _logger.LogWarning(this.context.SessionDataCollectionContext, "TestCaseEnded " + e.TestCaseName);
        }
    }

}
