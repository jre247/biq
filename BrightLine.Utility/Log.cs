using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Web;

namespace BrightLine.Utility
{
	public static class Log
	{
		public static void Error(Exception exception)
		{
			ErrorFormat("{0}", exception);
		}


		public static void ErrorFormat(string format, params object[] args)
		{
			Exception exception = null;
			if (args != null && args.Length >= 1)
				exception = args[0] as Exception;

			var logger = LogManager.GetLogger("BrightLine.Utility.Log.Error");

			SetCustomLoggingFields();

			if (exception == null)
				logger.Error(format, args);
			else
				logger.Error(format, args, exception);
		}


		public static void Info(string message)
		{
			InfoFormat("{0}", message);
		}

		public static void Warn(string message)
		{
			WarnFormat("{0}", message);

		}
		
		public static void WarnFormat(string format, params object[] args)
		{
			var logger = LogManager.GetLogger("BrightLine.Utility.Log.Warn");


			SetCustomLoggingFields();
			logger.Warn(format, args);
		}

		public static void InfoFormat(string format, params object[] args)
		{
			var logger = LogManager.GetLogger("BrightLine.Utility.Log.Info");
			SetCustomLoggingFields();
			logger.Debug(format, args);
		}


		public static void Debug(string message)
		{
			DebugFormat("{0}", message);
		}


		public static void DebugFormat(string format, params object[] args)
		{
			var logger = LogManager.GetLogger("BrightLine.Utility.Log.Debug");
			SetCustomLoggingFields();
			logger.Debug(format, args);
		}

		public static void SetCustomLoggingFields()
		{
			var host = Environment.MachineName;
			GlobalDiagnosticsContext.Set("host", host);

			if (HttpContext.Current != null && HttpContext.Current.User != null)
			{
				var user = HttpContext.Current.User.Identity.Name;
				GlobalDiagnosticsContext.Set("user", user);
			}
		}
	}
}
