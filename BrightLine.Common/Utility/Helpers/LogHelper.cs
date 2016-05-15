using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;
using BrightLine.Utility;

namespace BrightLine.Common.Utility
{
	public class LogHelper : ILogHelper
	{
		private ILog Logger { get;set;}

		public LogHelper()
		{
			Logger = IoC.Container.GetInstance<ILog>();
		}

		public ILog GetLogger()
		{
			Log.SetCustomLoggingFields();
			return Logger; 
		}

		
	}
}
