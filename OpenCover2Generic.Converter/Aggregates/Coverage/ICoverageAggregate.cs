using System;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public interface ICoverageAggregate
    {
        string Path { get; }

        void Modules(Action<IModuleCoverageEntity> action);
    }
}