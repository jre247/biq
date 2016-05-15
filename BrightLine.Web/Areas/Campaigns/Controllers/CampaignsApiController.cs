using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Resources;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Data;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Publishing.Constants;
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

namespace BrightLine.Web.Areas.Campaigns.Controllers
{
	[RoutePrefix("api/campaigns")]
	[CamelCase]
	public class CampaignsApiController : ApiController
	{
		private ICampaignService Campaigns { get;set;}
		private IFlashMessageExtensions FlashMessageExtensions { get; set; }

		public CampaignsApiController()
		{
			Campaigns = IoC.Resolve<ICampaignService>();
			FlashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();
		}

		[GET("")]
		[GET("listing")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public IEnumerable<CampaignsListingViewModel> CampaignListing()
		{
			IEnumerable<CampaignsListingViewModel> models;
			try
			{
				models = Campaigns.GetCampaignsListing().OrderBy(c => c.Name);	
			}
			catch (Exception ex)
			{
				IoC.Log.Error("CampaignsController.Index()", ex);
				models = new List<CampaignsListingViewModel>();
			}

			return models;
		}

		[GET("{campaignId:int}/summary")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignSummary([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var summary = Campaigns.GetSummary(campaignId);
				var json = CampaignSummaryViewModel.ToJObject(summary);

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign summary.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}


		[GET("{campaignId:int}/placements")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignPlacements([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var placements = Campaigns.GetPlacements(campaignId);
				var json = CampaignPlacementViewModel.ToJObject(placements);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign placements.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/features")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignFeatures([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var features = Campaigns.GetFeatures(campaignId);
				var json = CampaignFeatureViewModel.ToJObject(features);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign pages.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{pageId:int}/pages")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignPages([FromUri] int pageId)
		{
			try
			{
				var models = Campaigns.GetPages(pageId);
				var pages = JObject.FromObject(models);
				var json = new JObject();
				json["pages"] = pages;
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign pages.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/creatives/promotional")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignPromotionalCreatives([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var promotionals = Campaigns.GetPromotionalCreatives(campaignId);
				var json = CampaignCreativeViewModel.ToJObject(promotionals, "promotional");

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign promotional creatives.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/creatives/destinations")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignDestinationCreatives([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var destinations = Campaigns.GetDestinationCreatives(campaignId);
				var json = CampaignCreativeViewModel.ToJObject(destinations, "destination");

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign destination creatives.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/ads")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignAds([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var ads = Campaigns.GetAds(campaignId);
				var json = CampaignAdViewModel.ToJObject(ads, "ads");

				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign ads.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("lookups")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignLookups()
		{
			try
			{
				var json = Campaigns.GetCampaignLookups();
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign lookups.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("togglefavorite/{campaignId:int}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public bool ToggleFavorite([FromUri] int campaignId)
		{
			if (!Campaigns.Exists(campaignId))
				return false;

			var campaign = Campaigns.Get(campaignId);
			if (!Campaigns.IsAccessible(campaign))
				return false;

			try
			{
				var users = IoC.Resolve<IUserService>();

				var user = Auth.UserModel;
				var toggled = users.ToggleFavorite(user, campaign);
				return toggled;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not toggle favorite status.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/preview/{fullManifest:bool=false}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Preview([FromUri] int campaignId, [FromUri]bool fullManifest)
		{
			var campaign = Campaigns.Get(campaignId);

			if (!Campaigns.Exists(campaign, campaignId))
				return null;

			if (!Campaigns.IsAccessible(campaign))
				return null;

			try
			{
				var user = Auth.UserModel;
				var manifestVm = new ManifestViewModel(campaign, user);
				var manifestVmAsJson = ManifestViewModel.ToJObject(manifestVm);
				return manifestVmAsJson;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not create preview for campaign: " + campaignId + ".", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/ad-responses/{adId:int}/preview")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject PreviewAdResponses([FromUri] int campaignId, [FromUri] int adId)
		{
			var ads = IoC.Resolve<IAdService>();

			var campaign = Campaigns.Get(campaignId);

			if (!Campaigns.Exists(campaign, campaignId))
				return null;

			if (!Campaigns.IsAccessible(campaign))
				return null;
		
			var ad = ads.Get(adId);
			if(ad == null)
				return null;

			try
			{
				var publishAdResponsesService = IoC.Resolve<IAdResponsesService>();
				var adResponses = publishAdResponsesService.GetAdResponse(ad, PublishConstants.TargetEnvironments.Develop, Guid.NewGuid());

				var adResponsesJson = JObject.FromObject(adResponses);
				return adResponsesJson;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not create Ad Responses preview for campaign: " + campaignId + ".", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/cache/clear")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public bool ClearCache([FromUri] int campaignId)
		{
			var campaign = Campaigns.Get(campaignId);

			if (!Campaigns.Exists(campaign, campaignId))
				return false;

			if (!Campaigns.IsAccessible(campaign))
				return false;

			try
			{
				var prefix = campaignId.ToString() + "|";
				IoC.Cache.Remove((key) => key.StartsWith(prefix));
				return true;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not create preview for campaign: " + campaignId + ".", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("isduplicatecreativename/{campaignId:int}/{creativeId:int}/{name}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public bool IsDuplicateCreativeName([FromUri] int campaignId, [FromUri] int creativeId, [FromUri] string name)
		{
			var campaign = Campaigns.Get(campaignId, "Creatives");

			if (!Campaigns.Exists(campaign, campaignId))
				return false;

			if (!Campaigns.IsAccessible(campaign))
				return false;

			if (string.IsNullOrWhiteSpace(name))
				return false;

			try
			{
				var exists = campaign.Creatives.Any(c => c.Id != creativeId && !string.IsNullOrWhiteSpace(c.Name) &&
					c.Name.ToLowerInvariant() == name.ToLowerInvariant());
				return exists;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not check duplicate creative name for campaign: " + campaignId + ".", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("isduplicateadname/{campaignId:int}/{adId:int}/{name}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public bool IsDuplicateAdName([FromUri] int campaignId, [FromUri] int adId, [FromUri] string name)
		{
			var campaign = Campaigns.Get(campaignId, "Ads");

			if (!Campaigns.Exists(campaign, campaignId))
				return false;

			if (!Campaigns.IsAccessible(campaign))
				return false;

			if (string.IsNullOrWhiteSpace(name))
				return false;

			try
			{
				var exists = campaign.Ads.Any(a => a.Id != adId &&
					!string.IsNullOrWhiteSpace(a.Name) && a.Name.ToLowerInvariant() == name.ToLowerInvariant());
				return exists;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not check duplicate ad name for campaign: " + campaignId + ".", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[PascalCase]
		[GET("{campaignId:int}/adTagExport")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public HttpResponseMessage ExportAdTags([FromUri] int campaignId)
		{
			var response = new HttpResponseMessage();
			var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			try
			{
				var adTagsExportService = IoC.Resolve<IAdTagsExportService>();
				var auditEvents = IoC.Resolve<IAuditEventService>();

				var campaign = Campaigns.Get(campaignId);

				if (!Campaigns.Exists(campaign, campaignId))
					return null;

				var ms = new MemoryStream();
				adTagsExportService.ExportAdTags(ms, campaign);

				var fileName = string.Format(
					"{0}_{1}_Tags.xlsx",
					DateTime.UtcNow.ToString("yyyy_MM_dd"),
					campaign.Name.ToLower().Replace(" ", "_")
				);

				ms.Position = 0;
				response.Content = new StreamContent(ms);
				response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
				response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
				response.StatusCode = HttpStatusCode.OK;

				auditEvents.Audit("Download", campaign.Id.ToString(), "AdApiController.Download: " + fileName);

				return response;

			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign settings.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/deliveryGroups")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignDeliveryGroups([FromUri] int campaignId)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				var deliveryGroups = Campaigns.GetCampaignDeliveryGroups(campaignId);
				var json = CampaignDeliveryGroupViewModel.ToJObject(deliveryGroups);
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign promotional creatives.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{campaignId:int}/ad-responses/purge?targetEnv={targetEnv}")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject PurgeCampaignAdResponses([FromUri] int campaignId, [FromUri] string targetEnv)
		{
			try
			{
				if (!Campaigns.Exists(campaignId))
					return null;

				Campaigns.PurgeCampaignAdResponses(campaignId, targetEnv);

				return null;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not purge Campaign Ad Responses.", ex);
				FlashMessageExtensions.Debug(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

	}

}