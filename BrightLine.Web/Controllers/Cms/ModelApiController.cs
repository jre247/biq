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
		[GET("models/modelDefinitions")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject GetModelDefinitions()
		{
			try
			{
				var cmsModelDefinitions = IoC.Resolve<ICmsModelDefinitionService>();

				var modelDefinitions = cmsModelDefinitions.GetAll().OrderBy(m => m.Name);
				var mds = new Dictionary<int, object>();
				foreach (var modelDefinition in modelDefinitions)
				{
					var m = new ModelDefinitionViewModel(modelDefinition);
					mds.Add(modelDefinition.Id, m);
				}
				var json = JObject.FromObject(mds);

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("models/settingDefinitions")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject GetSettingDefinitions()
		{
			try
			{
				var cmsSettingDefinitions = IoC.Resolve<IRepository<CmsSettingDefinition>>();

				var settingDefinitions = cmsSettingDefinitions.GetAll().OrderBy(m => m.Name);
				var dict = new Dictionary<int, object>();
				foreach (var settingDefinition in settingDefinitions)
				{
					var m = new SettingDefinitionViewModel(settingDefinition);
					dict.Add(settingDefinition.Id, m);
				}

				var json = JObject.FromObject(dict);

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		

	}
}
