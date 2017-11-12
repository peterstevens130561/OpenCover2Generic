using BHGE.SonarQube.OpenCover2Generic.Consumer;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IJobConsumerFactory
    {
        IJobConsumer Create();
    }
}