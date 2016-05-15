using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Utility;
using BrightLine.Web.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace BrightLine.Web.Controllers
{
	[RoutePrefix("api/creatives")]
    public class CreativesApiController : ApiController
    {
		[GET("promotional/{id:int}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetPromotionalCreative([FromUri] int id)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var creatives = IoC.Resolve<ICreativeService>();
				var creative = creatives.GetPromotionalCreative(id);
				if(creative == null)
					return null;

				var json = PromotionalCreativeViewModel.ToJObject(creative);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("destination/{id:int}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetDestinationCreative([FromUri] int id)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var creatives = IoC.Resolve<ICreativeService>();

				var creative = creatives.GetDestinationCreative(id);
				if (creative == null)
					return null;

				var json = DestinationCreativeViewModel.ToJObject(creative);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[POST("destination/save")]
		[AcceptVerbs("POST")]
		[HttpPost]
		public int? DestinationSave(DestinationCreativeSaveViewModel viewModel)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var creatives = IoC.Resolve<ICreativeService>();

				var creative = creatives.Save(viewModel);
				if (creative == null)
					return null;

				return creative.Id;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

    }
}
