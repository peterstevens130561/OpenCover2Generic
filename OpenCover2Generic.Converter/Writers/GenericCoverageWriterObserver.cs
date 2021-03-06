﻿using System;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class GenericCoverageWriterObserver : IGenericCoverageWriterObserver
    {
        private readonly ICoverageWriter _coverageWriter;

        public GenericCoverageWriterObserver(ICoverageWriter coverageWriter)
        {
           _coverageWriter = coverageWriter;
        }

        public string Path { get; set; }


        public XmlTextWriter Writer { get; set; }

        public void OnBeginModule(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void OnBeginScan(object sender, EventArgs eventArgs)
        {
            _coverageWriter.WriteBegin(Writer);
        }

        public void OnEndModule(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void OnEndScan(object sender, EventArgs eventArgs)
        {
            _coverageWriter.WriteEnd(Writer);
        }

        public void OnModule(object v, ModuleEventArgs moduleEventArgs)
        {
            var model = moduleEventArgs.Entity;
            _coverageWriter.GenerateCoverage(model,Writer);
        }
    }
}
