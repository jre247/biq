
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Core;
using BrightLine.Core.Attributes;

namespace BrightLine.Service
{
	public class AuditEventService : CrudService<AuditEvent>, IAuditEventService
	{
		public AuditEventService(IRepository<AuditEvent> repo)
			: base(repo)
		{ }

		public void Audit(string operation, string group, string source)
		{
			var item = new AuditEvent
			{
				ActionDate = DateTime.Now,
				Group = group,
				ActionName = operation,
				User = Auth.UserName,
				IPAddress = GetIpAddress(),
				Source = source,
				RequestUrl = GetUrl()
			};

			item.Group = Truncate(item.Group, 50);
			item.User = Truncate(item.User, 50);
			item.ActionName = Truncate(item.ActionName, 50);
			item.Source = Truncate(item.Source, 50);
			item.IPAddress = Truncate(item.IPAddress, 50);
			item.RequestUrl = Truncate(item.RequestUrl, 250);

			base.Create(item);
		}

		private string Truncate(string text, int maxChars)
		{
			if (string.IsNullOrEmpty(text))
				return text;
			if (text.Length < maxChars)
				return text;

			return text.Substring(0, maxChars);
		}
		
		private string GetIpAddress()
		{
			if (!HasRequestContext())
				return string.Empty;

			return HttpContext.Current.Request.UserHostAddress;
		}
		
		private string GetUrl()
		{
			if (!HasRequestContext())
				return string.Empty;

			return HttpContext.Current.Request.Url.AbsoluteUri;
		}

		private bool HasRequestContext()
		{
			return HttpContext.Current != null && HttpContext.Current.Request != null;
		}
	}
}
