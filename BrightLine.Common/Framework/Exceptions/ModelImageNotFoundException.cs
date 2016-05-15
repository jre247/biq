﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Framework.Exceptions
{
	public class ModelImageNotFoundException : Exception
	{
		public ModelImageNotFoundException()
		{
		}


		public ModelImageNotFoundException(string message, Exception exception)
		{
			Errors = new List<string>();
			Errors.Add(message);
			Exception = exception;
		}


		public List<string> Errors;
		public Exception  Exception { get; set; }
	}
	
}
