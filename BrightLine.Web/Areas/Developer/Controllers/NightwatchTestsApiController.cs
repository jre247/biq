
using AttributeRouting;
using AttributeRouting.Web.Http;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Nightwatch;
using BrightLine.Common.ViewModels.Developer;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Utility;
using BrightLine.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	/// <summary>
	///	Process for running Nightwatch tests:
	//	1) IQ web server creates a nightwatch transaction record in sql server.
	//	2) IQ web server then makes web request using basic auth to the iq-services api endpoint on the build server
	//	3) iq-services then runs the nightwatch tests and when done it uses mssql npm package to execute a stored procedure in sql server that will change the success property to true based on a NightwatchTransaction record that matches on TransactionId.
	//	4) Client will be polling every 5 seconds to IQ web server to check if there's a NightwatchTransaction record in sql server, matching on TransactionId, that has success equal to true.
	//	5) When Success is true for a transaction then the client will then make another call to IQ web api endpoint to get report results html.
	//	6) This web api endpoint will then make a web request to the Nightwatch service api on build server to retrieve the reports html.
	//	7) This web api endpoint then filters out the head tag content and then the anchor tag that toggles tests.
	//	8) This web api endpoint then returns the formatted html back to the client
	//	9) Client then injects this reports html into a div
	//	10) Spinner on client stops.
	/// </summary>
	/// <returns></returns>
	[RoutePrefix("api/NightwatchTests")]
	[CamelCase]
	public class NightwatchTestsApiController : ApiController
    {
		private ISettingsService Settings { get;set;}
		private ICrudService<NightwatchTransaction> NightwatchTransactions { get;set;}
		private IFlashMessageExtensions FlashMessageExtensions { get; set; }

		public NightwatchTestsApiController()
		{
			Settings = IoC.Resolve<ISettingsService>();
			NightwatchTransactions = IoC.Resolve<ICrudService<NightwatchTransaction>>();
			FlashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();
		}

		[POST("Run")]
		[AcceptVerbs("POST")]
		[HttpPost]
		public JObject RunTests(NightwatchTestsViewModel viewModel)
		{
			string environment = null;

			try
			{
				environment = GetCurrentEnvironment(environment);

				var transactionId = CreateNightwatchTransaction(viewModel.BuildVersion, viewModel.BuildCommitHash);

				SendRequestToRunTests(environment, transactionId);

				var vm = new NightwatchTestsViewModel();
				vm.TransactionId = transactionId;

				return JObject.FromObject(vm);   
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve Nightwatch test results.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		

		[GET("Status/{transactionId}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public bool GetTransactionStatus([FromUri] Guid transactionId)
		{
			var isCompleted = false;

			if(transactionId == null)
				IoC.Log.Info("Transaction Id is null.");

			var transactions = NightwatchTransactions.Where(c => c.TransactionId == transactionId);
			if (transactions == null)
				IoC.Log.Info("Transaction does not exist for End to End Test: " + transactionId);

			var transaction = transactions.SingleOrDefault();
			if(transaction == null)
				return false;

			if (transaction.Status == NightwatchConstants.Status.COMPLETED)
			{
				transaction.DateCompleted = DateTime.UtcNow;
				isCompleted = true;
				NightwatchTransactions.Update(transaction);
			}

			return isCompleted;
		}

		[GET("Report")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public string GetTestsReport()
		{
			var buildServerIp = Settings.BuildServerIp;
			var nightwatchServicePort = Settings.NightwatchServicePort;
			var url = string.Format("http://{0}:{1}/nightwatch-report-results", buildServerIp, nightwatchServicePort);
			var username = Settings.IntegrationServiceUsername;
			var password = Settings.IntegrationServicePassword;

			using (var client = new WebClient())
			{
				WebAuthenticationHelper.SetBasicAuthHeader(client, username, password);

				string html = client.DownloadString(url);
				var htmlParsed = Regex.Replace(html, @"(<head>).*(</head>)", "", RegexOptions.Singleline);
				htmlParsed = Regex.Replace(htmlParsed, @"<a href=""#""(.*)</a>", "");

				return htmlParsed;
			}
		}

		#region Private Methods

		private string GetCurrentEnvironment(string environment)
		{
			if (Settings.CurrentEnvironment == EnvironmentType.PRO)
				environment = "production";
			else if (Settings.CurrentEnvironment == EnvironmentType.UAT)
				environment = "uat";
			else if (Settings.CurrentEnvironment == EnvironmentType.DEV)
				environment = "develop";
			else
				environment = "local";
			return environment;
		}

		private Guid CreateNightwatchTransaction(string buildVersion, string buildCommitHash)
		{
			//create End to End test transaction
			var transactionId = Guid.NewGuid();
			var nightwatchTransaction = new NightwatchTransaction
			{
				TransactionId = transactionId,
				BuildVersion = buildVersion,
				BuildCommitHash = buildCommitHash,
				Status = NightwatchConstants.Status.RUNNING
			};
			NightwatchTransactions.Create(nightwatchTransaction);
			return transactionId;
		}

		private void SendRequestToRunTests(string environment, Guid transactionId)
		{
			var buildServerIp = Settings.BuildServerIp;
			var integrationServicePort = Settings.IntegrationServicePort;
			var requestUrl = string.Format("http://{0}:{1}/nightwatchTests?environment={2}&transactionId={3}", buildServerIp, integrationServicePort, environment, transactionId);

			var request = WebRequest.Create(requestUrl);
			var username = Settings.IntegrationServiceUsername;
			var password = Settings.IntegrationServicePassword;
			WebAuthenticationHelper.SetBasicAuthHeader(request, username, password);

			request.Method = "GET";

			string text;
			var response = (HttpWebResponse)request.GetResponse();

			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				text = sr.ReadToEnd();
			}
		}

		#endregion

    }
}
