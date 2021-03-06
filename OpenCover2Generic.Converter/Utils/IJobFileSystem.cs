﻿using System;
using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface IJobFileSystem
    {
        string GetOpenCoverLogPath(string assembly);
        string GetOpenCoverOutputPath(string assembly);
        string GetIntermediateCoverageDirectory();


        void CreateRoot(IWorkspace workspace);
    }
}