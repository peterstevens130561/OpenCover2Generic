using System.Timers;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public interface ITimerAdapter
    {
        bool AutoReset { get; set; }
        event ElapsedEventHandler Elapsed;
        double Interval { get; set; }

        void Start();
        void Stop();
    }
}