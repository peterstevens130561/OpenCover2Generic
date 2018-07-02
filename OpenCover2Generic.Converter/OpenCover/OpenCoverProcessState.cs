namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public enum ProcessState
    {
        None,
        Busy,
        RecoverableFailure,
        IrrecoverableFailure,
        NoResults,
        TimedOut,
        Done,
        CouldNotRegister,
        NoTests,
        Starting,
        Run,
        RunningTests,
        LoggerNotInstalled
    }
}
