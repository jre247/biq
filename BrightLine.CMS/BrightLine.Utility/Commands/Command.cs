using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightLine.Utility;

namespace BrightLine.Utility.Commands
{
	/// <summary>
	/// The command pattern ( Currently used in the CMS )
	/// 
	/// 1. Tracks duration of execution ( time )
	/// 2. Success/fail
	/// 3. Logging of error
	/// 4. Auditing of action.
	/// </summary>
	public class Command : ICommand
	{
		protected CommandState _state;
		protected CommandResult _lastResult;
		protected bool _audit;
		protected bool _canRun;
		protected string _name;
		protected string _action;
		protected string _source;
		protected Func<object[], object> _executionCallback;


		/// <summary>
		/// Made static and as a lambda to keep this simple and avoid dependencies on AuditEventService and Ioc.
		/// </summary>
		public static Action<string, string, string> Auditor;


		/// <summary>
		/// Initialize
		/// </summary>
		public Command(string name, bool audit)
			: this(name, audit, null)
		{
		}


		/// <summary>
		/// Initialize
		/// </summary>
		public Command(string name, bool audit, Func<object[], object> callback)
		{
			var action = this.GetType().Name.Replace("Command", "");
			_name = name;
			_action = action;
			_audit = audit;
			_state = new CommandState
				{
					Name = name,
					Action = action
				};
			_executionCallback = callback;
		}


		public string Name { get { return _name; } }


		/// <summary>
		/// The last command result.
		/// </summary>
		public CommandResult LastResult { get { return _lastResult; } }


		/// <summary>
		/// Execute without arguments.
		/// </summary>
		public CommandResult Execute()
		{
			return ExecuteWithArgs(null);
		}


		/// <summary>
		/// Execute with arguments.
		/// </summary>
		/// <param name="args"></param>
		public CommandResult ExecuteWithArgs(object[] args)
		{
			// 1. Used to track duration of command execution.
			DateTime start = DateTime.Now;
			DateTime end = start;

			// 2. Used to track success/failure
			bool success = false;
			string message = "";

			// 3. Execute the command.
			object executionResult = null;
			double totalTimeInMilliseconds = 0;
			try
			{
				_state.LastRunTime = start;

				// Using the callback mode ? 
				if (_executionCallback != null)
				{
					executionResult = _executionCallback(args);
				}
				else
				{
					executionResult = ExecuteInternal(args);
				}
				end = DateTime.Now;
				success = true;
			}
			catch (Exception ex)
			{
				// Regardless of success/failure - track the duration.
				end = DateTime.Now;
				message = ex.Message;

				// Need to wrap the exception into another exception for the current logger.
				var newEx = new Exception(this._name + ":" + _action + ". Message: " + message, ex);
				Log.Error(newEx);

				// Throw the error back
				throw newEx;
			}

			// 4. Get the duration of the command.
			TimeSpan diff = end - start;
			totalTimeInMilliseconds = diff.TotalMilliseconds;

			// 5. Store this as the last result and return it.
			_lastResult = new CommandResult(success, message, _name, _action, (int)totalTimeInMilliseconds, start, end, _state.RunCount, executionResult);

			// 6. Audit the event ?
			if (_audit) Audit(_action, _name, _source);

			// 7 .Keep track of state : how many times the command has been run ( used for commands that can be called / run more than once ).
			_state.RunCount++;
			_state.HasRun = true;
			_state.LastResult = _lastResult;
			_state.LastRunTime = start;
			if (!success) _state.ErrorCount++;

			return _lastResult;
		}


		/// <summary>
		/// Execute method that actually does work.
		/// </summary>
		/// <param name="args"></param>
		protected virtual object ExecuteInternal(object[] args)
		{
			return null;
		}


		protected virtual void Audit(string action, string group, string source)
		{
			if (_audit && Auditor != null)
			{
				try
				{
					Auditor(action, group, source);
				}
				catch (Exception ex)
				{
					Log.ErrorFormat("Unable to audit : " + group + ":" + action, null);
				}
			}
		}
	}
}
