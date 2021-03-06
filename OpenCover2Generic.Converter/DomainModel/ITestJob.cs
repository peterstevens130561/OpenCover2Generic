﻿using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel
{
    public interface ITestJob
    {
        string Assemblies { get; }
        string FirstAssembly { get; }

        string [] Args { get; }
        string RepositoryRootDirectory { get; }
        IWorkspace Workspace { get; }
    }
}