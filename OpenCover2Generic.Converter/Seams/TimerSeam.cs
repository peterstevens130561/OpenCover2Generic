using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BHGE.SonarQube.OpenCover2Generic.Seams
{
    public class TimerSeam : ITimerSeam
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
    }
}
