using AttributeRouting;
using AttributeRouting.Web.Http;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using BrightLine.Service;
using BrightLine.Web.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BrightLine.Web.Controllers
{
	[RoutePrefix("api/cms")]
	[PascalCase]
	public partial class CmsApiController : ApiController
	{
		[PascalCase]
		[GET("creatives/{creativeId:int}/pages")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetPages([FromUri] int creativeId)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var creativeService = IoC.Resolve<ICreativeService>();
				var models = creativeService.GetPagesForCreative(creativeId);

				if (models == null)
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
	}
}