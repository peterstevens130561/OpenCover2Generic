using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public interface ICoverageAggregate
    {
        string Path { get; }

        void Modules(Action<IModule> action);
    }
}