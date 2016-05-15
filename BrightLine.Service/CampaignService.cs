using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Resources;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Campaigns;
using BrightLine.Common.Utility.SqlServer;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BrightLine.Common.Core;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility.AdTagExport;

namespace BrightLine.Service
{
	public class CampaignService : CrudService<Campaign>, ICampaignService
	{
		#region Members

		private static readonly int[] DESTINATIONS_AD_TYPE_IDS = new int[] { 19, 22, 10010, 10015 };

		#endregion

		#region Init

		public CampaignService(IRepository<Campaign> repo)
			: base(repo)
		{ 
			Creating += CampaignService_Saving;
			Updating += CampaignService_Saving;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines whether or not the current User is from the Advertiser of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignAdvertiser(Campaign campaign)
		{
			var campaignPermissionsService = IoC.Resolve<ICampaignPermissionsService>();
			return campaignPermissionsService.IsCampaignAdvertiser(campaign);
		}

		/// <summary>
		/// Determines whether or not the current User is from the Media Agency of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignMediaAgency(Campaign campaign)
		{
			var campaignPermissionsService = IoC.Resolve<ICampaignPermissionsService>();
			return campaignPermissionsService.IsCampaignMediaAgency(campaign);
		}

		/// <summary>
		/// Determines whether or not the current User is from the Media Partner of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignMediaPartner(Campaign campaign)
		{
			var campaignPermissionsService = IoC.Resolve<ICampaignPermissionsService>();
			return campaignPermissionsService.IsCampaignMediaPartner(campaign);
		}

		/// <summary>
		/// Determines if the Campaign is accessible to the current User
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsAccessible(Campaign campaign)
		{
			var campaignPermissionsService = IoC.Resolve<ICampaignPermissionsService>();
			return campaignPermissionsService.IsAccessible(campaign);
		}

		/// <summary>
		/// Determines if the current User can create a Campaign
		/// </summary>
		/// <returns></returns>
		public bool IsUserAllowedToCreate()
		{
			// Check if User is Admin or Employee
			if (Auth.Service.IsAdmin() || Auth.Service.IsEmployee())
				return true;

			return false;
		}

		public List<CampaignsListingViewModel> GetCampaignsListing()
		{
			var campaignsListingService = IoC.Resolve<ICampaignsListingService>();
			var campaigns = campaignsListingService.GetCampaignsListing();
			
			return campaigns;
		}

		public CampaignSummaryViewModel GetSummary(int campaignId)
		{
			var campaign = Get(campaignId);
			if (!IsAccessible(campaign))
				return null;

			var campaignSummaryService = IoC.Resolve<ICampaignSummaryService>();
			var details = campaignSummaryService.GetSummary(campaign);

			return details;
		}

		public List<Placement> GetPlacements(int id)
		{
			var placementsRepo = IoC.Resolve<IPlacementService>();

			var campaign = Get(id);
			if (!IsAccessible(campaign))
				return null;

			var adTypeGroups = campaign.Creatives.SelectMany(c => c.AdType.AdTypeGroups).Distinct().Select(atg => atg.Id);
			var placements = placementsRepo.Where(p => (p.AdTypeGroup != null) && adTypeGroups.Contains(p.AdTypeGroup.Id)).Distinct();
			return placements.ToList();
		}

		public List<Feature> GetFeatures(int id)
		{
			var featuresRepo = IoC.Resolve<IRepository<Feature>>();

			var campaign = Get(id);
			if (!IsAccessible(campaign))
				return null;

			var features = featuresRepo.Where(f => f.Campaign.Id == id);
			return features.ToList();
		}

		public Dictionary<int, PageViewModel> GetPages(int id)
		{
			var pagesRepo = IoC.Resolve<IRepository<Page>>();

			var pages = pagesRepo.Where(p => p.Feature.Campaign.Id == id);
			if (pages == null)
				return null;

			var pagesDictionary = pages.Select(m => new PageViewModel
			{
				id = m.Id,
				name = m.Name
			}).ToList().ToDictionary(c => c.id, c => c);

			return pagesDictionary;
		}

		public List<CampaignCreativeViewModel> GetPromotionalCreatives(int id)
		{
			var campaign = Get(id);
			if (!IsAccessible(campaign))
				return null;

			var creatives = IoC.Resolve<ICreativeService>();
			var isPromo = true;
			var promotionals = creatives.GetCreativesForCampaign(id, isPromo);

			return promotionals.ToList();
		}

		public List<CampaignCreativeViewModel> GetDestinationCreatives(int id)
		{
			var campaign = Get(id);
			if (!IsAccessible(campaign))
				return null;

			var creatives = IoC.Resolve<ICreativeService>();
			var isPromo = false;
			var destinations = creatives.GetCreativesForCampaign(id, isPromo);

			return destinations.ToList();
		}

		public List<CampaignAdViewModel> GetAds(int campaignId)
		{

			var campaign = Get(campaignId);
			if (!IsAccessible(campaign))
				return null;

			var adsService = IoC.Resolve<IAdService>();

			var ads = adsService.GetAdsForCampaign(campaign);
			return ads;
		}

		public override Campaign Create(Campaign campaign)
		{
			campaign.Generation = CampaignConstants.Generation;

			var campaignContentSchemasRepo = IoC.Resolve<IRepository<CampaignContentSchema>>();

			campaign = base.Create(campaign);
			campaign.CmsKey = GenerateCmsKey(campaign);
			base.Update(campaign);

			// If the Campaign does not have a CampaignContentSchema, create it
			if (campaign.CampaignContentSchemas == null || !campaign.CampaignContentSchemas.Any())
			{
				var schema = new CampaignContentSchema
				{
					Tag = CommonResources.ProductionTag,
					IsPublished = false,
					TotalLookups = 0,
					TotalModels = 0,
					Campaign = campaign
				};

				campaignContentSchemasRepo.Insert(schema);
			}

			return campaign;
		}

		public override List<Campaign> Create(IEnumerable<Campaign> campaigns)
		{
			if (campaigns == null)
				return new List<Campaign>();

			base.Create(campaigns);
			foreach (var campaign in campaigns)
			{
				campaign.CmsKey = GenerateCmsKey(campaign);
			}

			return base.Update(campaigns);
		}

		public JObject GetCampaignLookups()
		{
			var campaignLookupsService = IoC.Resolve<ICampaignLookupsService>();
			var json = campaignLookupsService.GetCampaignLookups();

			return json;
		}

		public Dictionary<int, CampaignDeliveryGroupViewModel> GetCampaignDeliveryGroups(int id)
		{
			var deliveryGroupsRepo = IoC.Resolve<IRepository<DeliveryGroup>>();

			var deliveryGroups = deliveryGroupsRepo.Where(d => d.Campaign.Id == id);

			if(Auth.UserModel.MediaPartner != null)
			{
				var mediaPartnerId = Auth.UserModel.MediaPartner.Id;
				deliveryGroups = deliveryGroups.Where(d => d.MediaPartner != null && (d.MediaPartner.Id == mediaPartnerId) || d.MediaPartner.Parent_Id == mediaPartnerId);
			}

			var deliveryGroupsHash = deliveryGroups.Select(d => new CampaignDeliveryGroupViewModel
			{
				id = d.Id,
				impressionGoal = d.ImpressionGoal,
				mediaPartnerId = d.MediaPartner.Id,
				name = d.Name,
				beginDateRaw = d.Ads.Count() > 0 ? d.Ads.Min(a => a.BeginDate) : (DateTime?)null,
				endDateRaw = d.Ads.Count() > 0 ? d.Ads.Max(a => a.EndDate) : null,
				mediaSpend = d.MediaSpend
			}).ToList().ToDictionary(c => c.id, c => c);

			return deliveryGroupsHash;

		}

		public bool Exists(int campaignId)
		{
			var campaign = Get(campaignId);
			if (campaign == null)
			{
				IoC.Log.Warn(string.Format(CommonResources.NonExistentEntity, campaignId));
				return false;
			}

			return true;
		}

		public bool Exists(Campaign campaign, int campaignId)
		{
			if (campaign == null)
			{
				IoC.Log.Warn(string.Format(CommonResources.NonExistentEntity, campaignId));
				return false;
			}

			return true;
		}

		public void PurgeCampaignAdResponses(int campaignId, string targetEnv)
		{
			var publishService = IoC.Resolve<IPublishService>();
			publishService.PurgeCampaignAdResponses(campaignId, targetEnv);
		}

		#endregion

		#region Private Methods

		private static string GetInvalidDateExceptionMessage(Campaign campaign)
		{
			return string.Format("{0} ({1}) has not begun.", campaign.Name, campaign.Id);
		}

		private string GenerateCmsKey(Campaign campaign)
		{
			var key = campaign.Id.ToString();
			return key;
		}

		private void CampaignService_Saving(object sender, CrudBeforeEventArgs args)
		{
			var campaign = sender as Campaign;
			if (campaign == null)
				throw new ArgumentNullException(CampaignConstants.Errors.CAMPAIGN_NULL);

			var vex = new ValidationException();

			// check for name is not null and name.length <=255
			if (string.IsNullOrEmpty(campaign.Name))
				vex.Add(CampaignConstants.Errors.CAMPAIGN_NAME_MISSING);
			else if (campaign.Name.Length > CampaignConstants.NAME_MAX_LENGTH)
				vex.Add(CampaignConstants.Errors.CAMPAIGN_NAME_MAX_LENGTH);

			if (vex.HasErrors)
			{
				args.Cancel = true;
				throw vex;
			}
		}

		#endregion
	}
}
