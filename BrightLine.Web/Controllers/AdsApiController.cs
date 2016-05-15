using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
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
	[RoutePrefix("api/ads")]
    public class AdsApiController : ApiController
    {
		[GET("{id:int}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject GetAd([FromUri] int id)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			try
			{
				var ads = IoC.Resolve<IAdService>();
				var ad = ads.Get(id);
				if(ad == null)
					return null;

				var adViewModel = new AdViewModel(ad);
				var json = AdViewModel.ToJObject(adViewModel);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				flashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[POST("save")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public int Save(AdViewModel adViewModel)
		{
			try
			{
				var ads = IoC.Resolve<IAdService>();

				var ad = ads.SaveAd(adViewModel);

				return ad.Id;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("cleanup-dirty-ads")]
		[AcceptVerbs("GET")]
		[HttpGet]
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Developer)]
		public void CleanupDirtyAds()
		{
			try
			{
				var ads = IoC.Resolve<IAdService>();
				ads.CleanupDirtyAds();
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not clean up Ads.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}
    }
}
