using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverProcessFactory : IOpenCoverProcessFactory
    {
        private readonly ProcessFactory _processFactory;

        public OpenCoverProcessFactory(ProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public IOpenCoverProcess Create()
        {
            return new OpenCoverProcess(_processFactory.CreateProcess(), new TimerAdapter(), new StateMachine());
        }
    }
}
