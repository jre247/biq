﻿using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using BrightLine.Web.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace BrightLine.Web.Controllers
{
	[RoutePrefix("api/cms")]
	public partial class CmsApiController : ApiController
	{
		[PascalCase]
		[GET("creatives/{creativeId:int}/models")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetModels([FromUri] int creativeId)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();
			try
			{
				var cmsService = IoC.Resolve<ICmsService>();
				var models = cmsService.GetModelsForCreative(creativeId);

				if(models == null)
					return null;

				return JObject.FromObject(models);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign settings.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[PascalCase]
		[GET("creatives/{creativeId:int}/settings")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetSettings([FromUri] int creativeId)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var cmsService = IoC.Resolve<ICmsService>();
				var settings = cmsService.GetSettingsForCreative(creativeId);

				if (settings == null)
					return null;

				return JObject.FromObject(settings);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign settings.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}
	}
}