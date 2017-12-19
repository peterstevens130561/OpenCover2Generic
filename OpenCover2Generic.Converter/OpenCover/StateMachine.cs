using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class StateMachine : IStateMachine
    {
        private const string StartingLine = @"Starting test execution, please wait..";
        private const string FailedToRegisterLine = @"Failed to register(user:True";
        private const string NoTestAvailableLine = @"No test is available in";
        private const string NoResultsLine = @"No results, this could be for a number of reasons";
        private const string EndOfOutputLine = "Visited Classes";
        public ProcessState State { get; set; }

        public void Transition(string line)
        {
            ProcessState resultState = State;
            switch (State)
            {
                case ProcessState.Starting:
                    if (line.Contains(StartingLine))
                    {
                        resultState = ProcessState.RunningTests;
                    }
                    break;
                case ProcessState.RunningTests:
                    if (line.StartsWith(NoTestAvailableLine, StringComparison.Ordinal))
                    {
                        resultState = ProcessState.NoTests;
                    }
                    if (line.Contains(NoResultsLine))
                    {
                        resultState = ProcessState.NoResults;
                    }
                    if (line.Contains(EndOfOutputLine))
                    {
                        resultState = ProcessState.Done;
                    }
                    break;
                case ProcessState.NoTests:
                case ProcessState.NoResults:
                case ProcessState.CouldNotRegister:
                    break;
                default:
                    if (line.Contains(FailedToRegisterLine))
                    {
                        resultState = ProcessState.CouldNotRegister;
                    }
                    break;
            }
            State = resultState;
   
        }
    }
}
