using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Framework.Exceptions
{
	public class BrightlineSftpException : Exception
	{
		public BrightlineSftpException()
		{
		}

		public BrightlineSftpException(string message)
		{
			Errors = new List<string>();
			Errors.Add(message);
		}


		public BrightlineSftpException(string message, Exception exception)
		{
			Errors = new List<string>();
			Errors.Add(message);
			Exception = exception;
		}


		public List<string> Errors;
		public Exception  Exception { get; set; }
	}
}
