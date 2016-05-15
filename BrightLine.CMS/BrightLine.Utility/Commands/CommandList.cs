using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Commands
{
    public class CommandList
    {
        private List<ICommand> _commands;
        private ICommand _last;


        /// <summary>
        /// Initialize.
        /// </summary>
        public CommandList()
        {
            _commands = new List<ICommand>();
        }


        /// <summary>
        /// Execute the command and add to list of executed commands.
        /// </summary>
        /// <param name="cmd"></param>
        public void Execute(ICommand cmd)
        {
            _commands.Add(cmd);
            cmd.Execute();
            _last = cmd;
        }


        /// <summary>
        /// Execute the command and add to list of executed commands.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public void ExecuteWithArgs(ICommand cmd, object[] args)
        {
            _commands.Add(cmd);
            cmd.ExecuteWithArgs(args);
            _last = cmd;
        }


        /// <summary>
        /// Gets the last command that was executed.
        /// </summary>
        public ICommand Last { get { return _last; } }


        /// <summary>
        /// Collect all the command results into a string ( for logging )
        /// </summary>
        /// <returns></returns>
        public string GetBenchmarksAsString()
        {
            if (_commands == null || _commands.Count == 0)
                return string.Empty;

            var buffer = new StringBuilder();
            foreach (var command in _commands)
            {
                var result = command.LastResult;
                if (result != null)
                {
                    var info = command.Name + " : " + result.ToString() + Environment.NewLine;
                    buffer.Append(info);
                }
                else
                {
                    buffer.Append("No result info for : " + command.Name+ Environment.NewLine);
                }
            }
            var message = buffer.ToString();
            return message;
        }
    }
}
