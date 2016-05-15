using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels.Developer;
using BrightLine.Common.ViewModels.Models;
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
using System.Web.Http;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	[RoutePrefix("api/media-buckets")]
	[CamelCase]
	public class MediaBucketsApiController : ApiController
    {
		[GET("sync")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public string Sync()
		{
			try
			{
				var settings = IoC.Resolve<ISettingsService>();
				string environment = null;

				if(Env.IsDev)
					environment = "Develop";
				else if (Env.IsUat)
					environment = "UAT";
				else
					return null;

				var buildServerIp = settings.BuildServerIp;
				var integrationServicePort = settings.IntegrationServicePort;
				var requestUrl = string.Format("http://{0}:{1}/media-buckets-sync?environment={2}", buildServerIp, integrationServicePort, environment);
				var request = WebRequest.Create(requestUrl);
				var username = settings.IntegrationServiceUsername;
				var password = settings.IntegrationServicePassword;
				WebAuthenticationHelper.SetBasicAuthHeader(request, username, password);

				request.Method = "GET";
				string responseText;
				var response = (HttpWebResponse)request.GetResponse();

				using (var sr = new StreamReader(response.GetResponseStream()))
				{
					responseText = sr.ReadToEnd();
				}

				return responseText;   
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve Media Buckets Sync results.", ex);
				var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

    }
}
