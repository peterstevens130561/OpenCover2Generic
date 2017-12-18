namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcessAdapter CreateProcess()
        {
            return new ProcessAdapter();
        }
    }
}
