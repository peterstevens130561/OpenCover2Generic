namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public interface IStateMachine
    {
        ProcessState State { get; set; }
        void Transition(string text);
    }
}