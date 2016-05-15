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
		[GET("settingInstances/{settingInstanceId:int}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetSettingInstance([FromUri] int settingInstanceId)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();
			try
			{
				var cmsService = IoC.Resolve<ICmsService>();
				var modelInstance = cmsService.GetSettingInstance(settingInstanceId);
				return modelInstance;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign model instances.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}


		[GET("{settingId:int}/settingInstances")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject GetSettingInstances([FromUri] int settingId)
		{
			try
			{
				var cmsService = IoC.Resolve<ICmsService>();
				var settingInstances = cmsService.GetSettingInstancesForSetting(settingId);
				return settingInstances != null ? JObject.FromObject(settingInstances) : null;
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[POST("settingInstances/save")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public JObject SaveInstanceInstance(ModelInstanceSaveViewModel model)
		{
			try
			{
				BoolMessageItem modelBoolMessage = null;
				if (model == null)
				{
					modelBoolMessage = new BoolMessageItem(false, "Setting Instance is null.");
					return new JObject(BoolMessageItem.ToJObject(modelBoolMessage));
				}


				var cmsService = IoC.Resolve<ICmsService>();
				modelBoolMessage = cmsService.SaveSettingInstance(model);
				return new JObject(BoolMessageItem.ToJObject(modelBoolMessage));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}


	}
}
