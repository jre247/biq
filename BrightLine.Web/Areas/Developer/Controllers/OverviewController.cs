using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	public class OverviewController : Controller
	{
		public ActionResult Index()
		{
			var settings = IoC.Resolve<ISettingsService>();

			try
			{
				var items = ConfigurationManager.AppSettings.AllKeys.ToList();
				var oltpCon = ConfigurationManager.ConnectionStrings["OLTP"].ConnectionString;

				var drive = System.IO.DriveInfo.GetDrives().FirstOrDefault(o => o.IsReady);

				// 1. Envrionment
				ViewBag.IQEnv = ConfigurationManager.AppSettings["Environment"];
				ViewBag.IQVersion = typeof(Campaign).Assembly.GetName().Version.ToString();

				// 2. Connctions
				ViewBag.OLTPConnectionString = LoadDataSourceInfo("OLTP", oltpCon);

				// 3. Errors
				var endOfToday = DateTime.Today.AddDays(1).AddTicks(-1);
				ViewBag.ErrorsTotalTMinus1 = GetTotalErrorsBetween(DateTime.Today.AddDays(-1), DateTime.Today);
				ViewBag.ErrorsTotalTMinus2 = GetTotalErrorsBetween(DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1));
				ViewBag.ErrorsTotalTMinus3 = GetTotalErrorsBetween(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-2));
				ViewBag.ErrorsTotalToday = GetTotalErrorsBetween(DateTime.Today, DateTime.Now);

				// 4. Database migration history
				ViewBag.MigrationHistory = GetMigrationHistory();

				// 5. Server info
				ViewBag.MachineInfo = Diagnostics.GetMachineInfo();

				// 6. IQ Versions in prod/uat
				ViewBag.IQEnvironmentVersions = GetIQEnvironmentVersions();

				// 7. Audit history
				ViewBag.Audits = GetAuditHistory(14, 15);

				ViewBag.FreeSpace = drive.TotalFreeSpace;
				ViewBag.AvailableSpace = drive.AvailableFreeSpace;
				ViewBag.TotalSpace = drive.TotalSize;
				ViewBag.AppSettingsKeys = items;
				ViewBag.SettingsList = settings.AllSettings;
				return View();
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				return View();
			}
		}


		private static List<KeyValuePair<string, string>> GetIQEnvironmentVersions()
		{
			var items = new List<KeyValuePair<string, string>>();
			items.Add(new KeyValuePair<string, string>("PRO", "ver ( not implemented "));
			items.Add(new KeyValuePair<string, string>("UAT", "ver ( not implemented "));
			items.Add(new KeyValuePair<string, string>("CI", "ver ( not implemented "));
			return items;
		}


		private static int GetTotalErrorsBetween(DateTime d1, DateTime d2)
		{
			var total = -1;
			var logEntries = IoC.Resolve<ICrudService<LogEntry>>();

			try
			{
				total = logEntries.Where(le => le.Level.Equals("Error") && le.DateCreated >= d1 && le.DateCreated <= d2).Count();
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				total = -1;
			}
			return total;
		}


		private static string LoadDataSourceInfo(string tag, string connText)
		{
			var text = "";
			try
			{
				var con = new SqlConnectionStringBuilder(connText);
				text += " Datasource : " + con.DataSource;
				text += ", Initial Catalog : " + con.InitialCatalog;
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				text = "Error loading connection information for : " + tag;
			}
			return text;
		}


		private static List<string> GetMigrationHistory()
		{
			var history = new List<string>();
			try
			{
				var db = new DatabaseHelper("OLTP");

				// 1. Get Datatable using sql text
				var table = db.ExecuteDataTableText("select top 15 MigrationId from __MigrationHistory (nolock) order by MigrationId desc");

				for (var ndx = 0; ndx < table.Rows.Count; ndx++)
				{
					var row = table.Rows[ndx];
					var migration = row[0].ToString();
					history.Add(migration);
				}
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				history.Add("Error loading migration history : " + ex.Message);
			}
			return history;
		}


		private static List<AuditEvent> GetAuditHistory(int daysBack, int maxRecords)
		{
			List<AuditEvent> audits = null;
			var auditEvents = IoC.Resolve<IAuditEventService>();

			try
			{
				var date = DateTime.Today.AddDays(-daysBack);
				audits = auditEvents.Where(a => a.DateCreated >= date)
				   .OrderByDescending(a => a.DateCreated)
				   .Take(maxRecords)
				   .ToList();
			}
			catch (Exception ex )
			{ 
				IoC.Log.Error(ex);	
			}
			return audits;
		}
	}


	class Diagnostics
	{

		/// <summary>
		/// Get the machine level information
		/// </summary>
		/// <returns>Dictionary with machine information.</returns>
		public static List<KeyValuePair<string, string>> GetMachineInfo()
		{
			var items = new List<KeyValuePair<string, string>>();
			try
			{
				// Get all the machine info.
				items.Add(new KeyValuePair<string, string>("Machine Name", Environment.MachineName));
				items.Add(new KeyValuePair<string, string>("Domain", Environment.UserDomainName));
				items.Add(new KeyValuePair<string, string>("User Name", Environment.UserName));
				items.Add(new KeyValuePair<string, string>("CommandLine", Environment.CommandLine));
				items.Add(new KeyValuePair<string, string>("ProcessorCount", Environment.ProcessorCount.ToString()));
				items.Add(new KeyValuePair<string, string>("OS Version ", Environment.OSVersion.Version.ToString()));
				items.Add(new KeyValuePair<string, string>("OS Version Platform", Environment.OSVersion.Platform.ToString()));
				items.Add(new KeyValuePair<string, string>("OS ServicePack", Environment.OSVersion.ServicePack));
				items.Add(new KeyValuePair<string, string>("OS VersionString", Environment.OSVersion.VersionString));
				items.Add(new KeyValuePair<string, string>("System Directory", Environment.SystemDirectory));
				items.Add(new KeyValuePair<string, string>("Memory", Environment.WorkingSet.ToString()));
				items.Add(new KeyValuePair<string, string>("Version", Environment.Version.ToString()));
				items.Add(new KeyValuePair<string, string>("Current Directory", Environment.CurrentDirectory));
			}
			catch (Exception ex)
			{
				items.Add(new KeyValuePair<string, string>("Error loading machine info", ex.Message));
			}
			return items;
		}
	}



}
