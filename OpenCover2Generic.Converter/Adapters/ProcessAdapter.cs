﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    [ExcludeFromCodeCoverage]
    public class ProcessAdapter : IProcessAdapter
    {
        private readonly Process process = new Process();

        public event DataReceivedEventHandler DataReceived
        {
            add
            {
                process.OutputDataReceived += value;
                process.ErrorDataReceived += value;
            }
            remove
            {
                process.OutputDataReceived -= value;
                process.OutputDataReceived -= value;
            }
        }


        public bool HasExited
        {
            get
            {
                return process.HasExited;
            }
        }

        public ProcessStartInfo StartInfo
        {
            get { return process.StartInfo; }
            set { process.StartInfo = value; }
        }

        public void Start()
        {
            process.EnableRaisingEvents = true;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        public void Kill()
        {
            process.Kill();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    process.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }



        #endregion
    }
}
