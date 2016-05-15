using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Framework.Exceptions
{
	public class ResourceValidationException : Exception
	{
		public ResourceValidationException()
		{
		}


		public ResourceValidationException(string message)
		{
			Errors = new List<string>();
			Errors.Add(message);
		}


		public List<string> Errors;
	}
}
