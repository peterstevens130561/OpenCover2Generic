using System;
using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface IProcess : IDisposable
    {

        /// <summary>
        /// register both stdout and stderr
        /// </summary>
        event DataReceivedEventHandler DataReceived;

        bool HasExited { get; }
        ProcessStartInfo StartInfo {get;set; }


        void Start();

        void Kill();
    }
}