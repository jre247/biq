using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BrightLine.Common.Services
{
	public interface ICampaignService : ICrudService<Campaign>
	{
		List<CampaignsListingViewModel> GetCampaignsListing();

		CampaignSummaryViewModel GetSummary(int id);

		List<Placement> GetPlacements(int id);
        List<Feature> GetFeatures(int id);
		Dictionary<int, PageViewModel> GetPages(int id);
		List<CampaignCreativeViewModel> GetPromotionalCreatives(int id);
		List<CampaignCreativeViewModel> GetDestinationCreatives(int id);
		List<CampaignAdViewModel> GetAds(int id);
		Dictionary<int, CampaignDeliveryGroupViewModel> GetCampaignDeliveryGroups(int id);

		/// <summary>
		/// Determines whether or not the current User is from the Advertiser of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		bool IsCampaignAdvertiser(Campaign campaign);


		/// <summary>
		/// Determines if the Campaign is accessible to the current User
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		bool IsAccessible(Campaign campaign);


		/// <summary>
		/// Determines if the current User can create a Campaign
		/// </summary>
		/// <returns></returns>
		bool IsUserAllowedToCreate();

		/// <summary>
		/// Gets a json representation of SPA needed lookups.
		/// </summary>
		/// <returns></returns>
		JObject GetCampaignLookups();

		bool Exists(int campaignId);

		bool Exists(Campaign campaign, int campaignId);

		void PurgeCampaignAdResponses(int campaignId, string targetEnv);

	}
}
