using System;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class StateMachine : IStateMachine
    {
        private const string StartingLine = @"Starting test execution, please wait..";
        private const string FailedToRegisterLine = @"Failed to register(user:True";
        private const string NoTestAvailableLine = @"No test is available in";
        private const string NoResultsLine = @"No results, this could be for a number of reasons";
        private const string EndOfOutputLine = "Visited Classes";
        private const string LoggerNotInstalled = @"Error: Could not find a test logger with URI or FriendlyName 'VsTestSonarQubeLogger'.";
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
                    if (line.Contains(FailedToRegisterLine))
                    {
                        resultState = ProcessState.CouldNotRegister;
                    }
                    if (line.Contains(LoggerNotInstalled))
                    {
                        resultState = ProcessState.LoggerNotInstalled;
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
                case ProcessState.LoggerNotInstalled:
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
