using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Commands
{
    public interface ICommand
    {
        string Name { get; }
        CommandResult Execute();
        CommandResult ExecuteWithArgs(object[] args);
        CommandResult LastResult { get; }
    }
}
