using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Commands
{
    /// <summary>
    /// Command state
    /// </summary>
    public class CommandState
    {
        public string Name;
        public string Action;
        public DateTime LastRunTime;
        public bool HasRun;
        public int RunCount;
        public int ErrorCount;
        public CommandResult LastResult;


        public CommandState Copy()
        {
            CommandState state = new CommandState();
            state.Name = Name;
            state.Action = Action;
            state.LastRunTime = LastRunTime;
            state.HasRun = HasRun;
            state.RunCount = RunCount;
            state.ErrorCount = ErrorCount;
            return state;
        }
    }
}
