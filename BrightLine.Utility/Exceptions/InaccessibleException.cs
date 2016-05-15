using System;
using System.Runtime.Serialization;

namespace BrightLine.Common.Framework.Exceptions
{
	[Serializable]
	public class InaccessibleException : BrightLineException
	{
		/// <summary>
		/// Initializes a new instance of the InaccessibleException class.
		/// </summary>
		public InaccessibleException(int id)
			: this("Campaign " + id + " is inaccessible.")
		{ }

		/// <summary>
		/// Initializes a new instance of the InaccessibleException class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InaccessibleException(string message)
			: base(message)
		{ }
	}
}
