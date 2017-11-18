using System;
using System.Timers;

namespace BHGE.SonarQube.OpenCover2Generic.Seams
{
    public interface ITimerSeam
    {
        bool AutoReset { get; set; }
        event ElapsedEventHandler Elapsed;
        double Interval { get; set; }

        void Start();
        void Stop();
    }
}