﻿using BHGE.SonarQube.OpenCover2Generic.Consumer;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using OpenCover2Generic.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    /// <summary>
    /// Creates JobConsumers
    /// </summary>
    public class JobConsumerFactory : IJobConsumerFactory
    {
        private readonly IJobFileSystem _jobFileSystem;
        private readonly IOpenCoverCommandLineBuilder _openCoverCommandLineBuilder;
        private readonly IOpenCoverManagerFactory _openCoverManagerFactory;

        public JobConsumerFactory(IOpenCoverCommandLineBuilder openCoverCommandLineBuilder, 
            IJobFileSystem jobFileSystem, 
            IOpenCoverManagerFactory openCoverManagerFactory)
        {
            _openCoverCommandLineBuilder = openCoverCommandLineBuilder;
            _jobFileSystem = jobFileSystem;
            _openCoverManagerFactory = openCoverManagerFactory;
        }

        public IJobConsumer Create()
        {
            return new JobConsumer(_openCoverCommandLineBuilder, _jobFileSystem, _openCoverManagerFactory);
        }
    }
}
