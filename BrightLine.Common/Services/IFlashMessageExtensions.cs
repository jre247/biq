using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BrightLine.Common.Services
{
	public interface IFlashMessageExtensions
	{
		void Custom(string message, bool isRedirect = false, Exception exception = null);
		void Success(string message, bool isRedirect = false);
		void Info(string message, bool isRedirect = false);
		void Debug(Exception exception, bool isRedirect = false);
		void Error(string message, bool isRedirect = false, Exception exception = null);

	}
}
