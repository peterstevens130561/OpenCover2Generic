using System;
using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public interface IProcessAdapter : IDisposable
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