using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic.Seams
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess()
        {
            return new SimpleProcess();
        }


        public IOpenCoverProcess CreateOpenCoverProcess()
        {
            return new OpenCoverProcess(CreateProcess(),new TimerSeam());
        }
    }
}
