namespace BHGE.SonarQube.OpenCover2Generic.Seams
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess()
        {
            return new SimpleProcess();
        }
    }
}
