using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Core;
using BrightLine.Web.Controllers;
using BrightLine.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	public class LogController : BaseController
	{
		private IRepository<LogEntry> LogEntries { get;set;}

		public LogController()
		{
			LogEntries = IoC.Resolve<IRepository<LogEntry>>();
		}

		public ActionResult Index()
		{
			return View();
		}

		public ContentResult Logs(int iDisplayStart, int iDisplayLength, string sSearch, bool includeDeleted = false)
		{
			var dbLogs = LogEntries.GetAll(includeDeleted);
			Func<LogEntry, bool> filter;
			if (string.IsNullOrWhiteSpace(sSearch))
				filter = (le => true);
			else
			{
				sSearch = sSearch.ToLowerInvariant();
				filter = (le => (le.Level.ToLowerInvariant().Contains(sSearch) ||
								 le.Message.ToLowerInvariant().Contains(sSearch) ||
								 le.Logger.ToLowerInvariant().Contains(sSearch) ||
								 le.Display.ToLowerInvariant().Contains(sSearch) ||
								 le.DateCreated.ToString().ToLowerInvariant().Contains(sSearch)));
			}
			var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
			Func<LogEntry, string> order;
			if (sortColumnIndex == 0)
				order = (le => le.Level);
			else if (sortColumnIndex == 1)
				order = (le => le.DateCreated.ToString("yyyyMMddhhmmssffff"));
			else if (sortColumnIndex == 2)
				order = (le => le.Message);
			else if (sortColumnIndex == 3)
				order = (le => le.Logger);
			else if (sortColumnIndex == 4)
				order = (le => le.Id.ToString("00000"));
			else if (sortColumnIndex == 5)
				order = (le => le.Display);
			else
				order = (le => "");

			var data =
				((Request["sSortDir_0"] == "desc")
					 ? dbLogs.Where(filter).OrderByDescending(order)
					 : dbLogs.Where(filter).OrderBy(order)).Skip(iDisplayStart).Take(iDisplayLength);
			var response = new
			{
				iTotalRecords = dbLogs.Count(),
				aaData = data,
				iTotalDisplayRecords = dbLogs.Count()
			};
			var settings = new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Include,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize
			};
			return Content(JsonConvert.SerializeObject(response, settings), "application/json");
		}

		public void Archive()
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var ids = LogEntries.GetAll().Select(le => le.Id).ToArray();
				LogEntries.Delete((e) => ids.Contains(e.Id), DeleteTypes.Soft); // soft delete all entries
			}
			catch (Exception ex)
			{
				flashMessageExtensions.Error("Could not archive the current entries.", exception: ex);
			}
		}

		public ActionResult Elmah()
		{
			return View();
		}
	}
}
