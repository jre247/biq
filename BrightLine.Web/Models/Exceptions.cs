using System;

namespace BrightLine.Web
{
	public class JavaScriptException : Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{ }
	}
}