using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Commands
{
    public class CommandResult
    {
        public CommandResult(bool success, String message, string name, string action, int totalMilliseconds,
            DateTime start, DateTime end, int runCount, Object result)
        {
            Success = success;
            Message = message;
            Name = name;
            Action = action;
            TotalMilliseconds = totalMilliseconds;
            Start = start;
            End = end;
            RunCount = runCount;
            Result = result;
        }


        public readonly bool Success;
        public readonly string Message;
        public readonly string Name;
        public readonly string Action;
        public readonly int TotalMilliseconds;
        public readonly DateTime Start;
        public readonly DateTime End;
        public readonly int RunCount;
        public readonly object Result;


        public override string ToString()
        {
            string format = "Name : {0}, Success : {1}, Action : {2}, Start : {3}, End : {4}, TotalMilliseconds : {5}, Run Count: {6}, Message {7}";
            var message = string.Format(format, Name, Success, Action, Start, End, TotalMilliseconds, RunCount, Message);
            return message;
        }
    }
}
