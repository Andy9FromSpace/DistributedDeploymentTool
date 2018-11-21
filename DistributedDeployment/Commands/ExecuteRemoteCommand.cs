using System;
using System.Text.RegularExpressions;

namespace DistributedDeployment.Commands
{
    internal class ExecuteRemoteCommand : ICommand
    {
        private readonly string _remoteAddress;
        private readonly string _securityToken;
        private readonly string _command;
        private readonly int _timeToWait;

        public ExecuteRemoteCommand(string remoteAddress, string securityToken, string command, int timeToWait = -1)
        {
            _remoteAddress = remoteAddress;
            _securityToken = securityToken;
            _command = command;
            _timeToWait = timeToWait;
        }

        public string Execute()
        {
            try
            {
                var commandOutput = ServiceProxy.Execute(_remoteAddress, _securityToken, _command, _timeToWait > 0 ? _timeToWait : int.MaxValue);
                if (!Regex.IsMatch(commandOutput, "Exit code 0"))
                {
                    throw new Exception(commandOutput + Environment.NewLine + "Remote command exited with an error code");
                }
                return commandOutput;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            return null;
        }
    }
}
