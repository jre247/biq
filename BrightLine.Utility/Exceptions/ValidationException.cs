using System;
using System.Collections.Generic;

namespace BrightLine.Common.Framework.Exceptions
{
	public class ValidationException : BrightLineException
	{
		private List<string> _errors;

		public ValidationException()
			: this(null, null)
		{ }

		public ValidationException(string message) :
			this(message, null)
		{ }

		public ValidationException(string message, Exception innerException) :
			base(null, innerException)
		{
			 _errors = string.IsNullOrEmpty(message) ?  new List<string>() {  } :  new List<string>() { message };
		}

		public void Add(string message)
		{
			_errors.Add(message);
		}

		public override string Message { get { return string.Join(", ", Errors); } }

		public bool HasErrors { get { return _errors.Count > 0; } }

		public string[] Errors { get { return _errors.ToArray(); } }
	}
}
