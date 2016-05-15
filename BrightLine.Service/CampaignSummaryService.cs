using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class CampaignSummaryService : ICampaignSummaryService
	{
		#region Public Methods

		public CampaignSummaryViewModel GetSummary(Campaign campaign)
		{
			int? mediaPartnerId = null;

			if (Auth.UserModel.MediaPartner != null)
				mediaPartnerId = Auth.UserModel.MediaPartner.Id;

			var details = GetCampaignSummaryResult(campaign, mediaPartnerId);

			if (!string.IsNullOrWhiteSpace(details.beginDate))
			{
				var svc = new CampaignAnalyticsService();

				var begin = svc.GetBeginDate(campaign, details.beginDateRaw);

				var end = svc.GetEndDate(campaign, details.endDateRaw);

				details.hasAnalytics = svc.CampaignAnalyticsAccessible(campaign);

				// If the analytics is accessible, set the default begin/end date
				if (details.hasAnalytics)
				{
					details.analyticsBeginDate = begin.ToString();
					details.analyticsEndDate = end.ToString();
					details.timeInterval = svc.GetInterval(null, begin, end).ToString();
				}
			}

			return details;
		}

		#endregion

		#region Private Methods

		private CampaignSummaryViewModel GetCampaignSummaryResult(Campaign campaign, int? mediaPartnerId)
		{
			var productsRepo = IoC.Resolve<IRepository<Product>>();
			var brandsRepo = IoC.Resolve<IRepository<Brand>>();
			var advertisersRepo = IoC.Resolve<IRepository<Advertiser>>();
			var subSegmentsRepo = IoC.Resolve<IRepository<SubSegment>>();
			var segmentsRepo = IoC.Resolve<IRepository<Segment>>();
			var verticalsRepo = IoC.Resolve<IRepository<Vertical>>();
			var resourcesRepo = IoC.Resolve<IResourceService>();
			var campaignContentSchemasRepo = IoC.Resolve<IRepository<CampaignContentSchema>>();
			var deliveryGroupsRepo = IoC.Resolve<IRepository<DeliveryGroup>>();
			var mediaPartnersRepo = IoC.Resolve<IMediaPartnerService>();
			var placementsRepo = IoC.Resolve<IPlacementService>();
			var agenciesRepo = IoC.Resolve<IAgencyService>();
			var adsRepo = IoC.Resolve<IAdService>();
			var campaignsRepo = IoC.Resolve<ICampaignService>();

			var campaignId = campaign.Id;

			var products = productsRepo.GetAll();
			var brands = brandsRepo.GetAll();
			var advertisers = advertisersRepo.GetAll();
			var subsegments = subSegmentsRepo.GetAll();
			var segments = segmentsRepo.GetAll();
			var verticals = verticalsRepo.GetAll();
			var resources = resourcesRepo.GetAll();
			var campaignContentSchemas = campaignContentSchemasRepo.GetAll();
			var campaigns = campaignsRepo.GetAll();
			var deliveryGroups = deliveryGroupsRepo.GetAll();
			var mediaPartnersList = mediaPartnersRepo.GetAll();
			var agencies = agenciesRepo.GetAll();
			var ads = adsRepo.GetAll();
			var placements = placementsRepo.GetAll();

			var details = (from c in campaigns
						   join p in products on c.Product.Id equals p.Id
						   join b in brands on p.Brand.Id equals b.Id into bp
						   from b in bp.DefaultIfEmpty()
						   join a in advertisers on b.Advertiser.Id equals a.Id into ab
						   from a in ab.DefaultIfEmpty()
						   join r in resources on c.Thumbnail.Id equals r.Id into rc
						   from r in rc.DefaultIfEmpty()
						   where c.Id == campaignId
						   select new CampaignSummaryViewModel
						   {
							   id = c.Id,
							   name = c.Name,
							   description = c.Description,
							   salesForceId = c.SalesForceId,
							   resourceId = r.Id,
							   resourceName = r.Name,
							   resourceFilename = r.Filename,
							   resourceType = r.ResourceType.Id,
							   productName = p.Name,
							   brandName = b.Name,
							   advertiserName = a.Name,
							   isDeleted = c.IsDeleted,
							   dateUpdatedRaw = c.DateUpdated,
							   beginDateRaw = ads.Where(aa => aa.Campaign.Id == campaignId).Min(aa => aa.BeginDate),
							   endDateRaw = ads.Where(aa => aa.Campaign.Id == campaignId).Max(aa => aa.EndDate),
							   databaseDate = DateTime.UtcNow,
							   platforms = ads.Where(ai => ai.Campaign.Id == c.Id).Select(ai => ai.Platform.Id).Distinct(),
							   mediaPartners = (
								  from aa in ads
								  join pp in placements on aa.Placement.Id equals pp.Id
								  join m in mediaPartnersList on pp.MediaPartner.Id equals m.Id
								  where aa.Campaign.Id == campaignId &&
								  (mediaPartnerId == null || (pp != null && pp.MediaPartner.Id == mediaPartnerId) || (m != null && m.Parent_Id == mediaPartnerId))
								  select m.Id
							  ).Distinct()
						   }
					).FirstOrDefault();

			return details;
		}

		#endregion
	}
}
