﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public class OpenCoverManagerFactory : IOpenCoverManagerFactory
    {

        private readonly IProcessFactory _processFactory;

        public OpenCoverManagerFactory(IProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public OpenCoverRunnerManager CreateManager()
        {
            return new OpenCoverRunnerManager(_processFactory);
        }
    }
}
