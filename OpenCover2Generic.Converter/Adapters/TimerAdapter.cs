using System;
using System.Timers;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public class TimerAdapter : ITimerAdapter, IDisposable
    {
        private readonly Timer _timer = new Timer();
        public bool AutoReset
        {
            get
            {
                return _timer.AutoReset;
            }

            set
            {
                _timer.AutoReset = value;
            }
        }

        public event ElapsedEventHandler Elapsed
        {
            add
            {
                _timer.Elapsed += value;
            }
            remove
            {
                _timer.Elapsed -= value;
            }
        }


        public virtual double Interval
        {
            get
            {
                return _timer.Interval;
            }

            set
            {
                _timer.Interval = value;
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public void Stop()
        {
            _timer.Stop();
        }
        #endregion
    }
}
