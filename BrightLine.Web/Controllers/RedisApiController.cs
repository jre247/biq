using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Service.Redis.Interfaces;
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
	[RoutePrefix("api/redis")]
    public class RedisApiController : ApiController
    {
		private IFlashMessageExtensions FlashMessageExtensions { get; set; }

		public RedisApiController()
		{
			FlashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();
		}

		/// <summary>
		/// Purge all items in Redis
		/// </summary>
		[GET("purge/all")]
		[AcceptVerbs("GET")]
		[HttpGet]
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Developer)]
		public void PurgeAll()
		{
			try
			{
				var redisService = IoC.Resolve<IRedisService>();

				redisService.PurgeAll();
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not purge all items in Redis.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		/// <summary>
		/// Refresh brs files by downloading and storing in Redis
		/// </summary>
		[GET("brs/refresh")]
		[AcceptVerbs("GET")]
		[HttpGet]
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Developer)]
		public void RefreshBrs()
		{
			try
			{
				var service = IoC.Resolve<IRedisSubscriptionsService>();

				service.RefreshBrsFiles();
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not refresh brs files.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}
    }
}
