// Copyright 2013 Kim Christensen, Todd Meinershagen, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;
using System.Web;


using Elmah;
using NLog.Targets;

namespace NLog.Elmah
{
	[Target("Elmah")]
	public sealed class ElmahTarget : TargetWithLayout
	{
		private readonly ErrorLog _errorLog;

		public static string ApplicationName { get; set; }
		public bool LogLevelAsType { get; set; }

		public ElmahTarget()
			: this(ErrorLog.GetDefault(null))
		{ }

		public ElmahTarget(ErrorLog errorLog)
		{
			_errorLog = errorLog;
			LogLevelAsType = false;
		}

		protected override void Write(LogEventInfo logEvent)
		{
			var logMessage = Layout.Render(logEvent);
			var httpContext = HttpContext.Current;
			var error = logEvent.Exception == null ? new Error() : httpContext != null && httpContext.Items.Contains("HttpRequestIsAvailable") ? new Error(logEvent.Exception, httpContext) : new Error(logEvent.Exception);
			var type = error.Exception != null
						   ? error.Exception.GetType().FullName
						   : LogLevelAsType ? logEvent.Level.Name : string.Empty;
			error.Type = type;
			error.Message = logMessage;
			error.Time = logEvent.TimeStamp;
			error.HostName = Environment.MachineName;
			error.Detail = logEvent.Exception == null ? logMessage : logEvent.Exception.StackTrace;
			error.Source = logEvent.Exception == null ? "BrightLine.Web" : logEvent.Exception.TargetSite.Name;
			error.User = HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null ?
				HttpContext.Current.User.Identity.Name : "No user";
			_errorLog.Log(error);
		}
	}
}