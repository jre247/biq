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
	public class CampaignsListingService : ICampaignsListingService
	{
		#region Public Methods

		public List<CampaignsListingViewModel> GetCampaignsListing()
		{
			var userId = Auth.UserModel.Id;
			int? mediaAgencyId = null;
			int? mediaPartnerId = null;
			int? advertiserId = null;

			if (Auth.Service.IsClient())
			{
				if (Auth.UserModel.Advertiser == null)
					throw new ApplicationException(string.Format("Unable to retrieve Advertiser of user {0}", Auth.UserName));

				advertiserId = Auth.UserModel.Advertiser.Id;
			}

			if (Auth.Service.IsAgencyPartner())
			{
				if (Auth.UserModel.MediaAgency == null)
					throw new ApplicationException(string.Format("Unable to retrieve Media Agency of user {0}", Auth.UserName));

				mediaAgencyId = Auth.UserModel.MediaAgency.Id;
			}

			if (Auth.Service.IsMediaPartner())
			{
				if (Auth.UserModel.MediaPartner == null)
					throw new ApplicationException(string.Format("Unable to retrieve Media Partner of user {0}", Auth.UserName));

				mediaPartnerId = Auth.UserModel.MediaPartner.Id;
			}

			var csvms = GetCampaignListingQueryResults(userId, mediaAgencyId, mediaPartnerId, advertiserId);


			var svc = new CampaignAnalyticsService();
			foreach (var campaign in csvms)
			{
				campaign.HasAnalytics = svc.CampaignAnalyticsAccessible(campaign.BeginDate);
				campaign.HasCms = (Auth.IsUserInRole(AuthConstants.Roles.Developer) || Auth.IsUserInRole(AuthConstants.Roles.CMSAdmin));
			}

			return csvms;
		}

		#endregion

		#region Private Methods

		private List<CampaignsListingViewModel> GetCampaignListingQueryResults(int userId, int? mediaAgencyId, int? mediaPartnerId, int? advertiserId)
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
			var agenciesRepo = IoC.Resolve<IAgencyService>();
			var adsRepo = IoC.Resolve<IAdService>();
			var campaignsRepo = IoC.Resolve<ICampaignService>();

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
			var mediaPartners = mediaPartnersRepo.GetAll();
			var agencies = agenciesRepo.GetAll();
			var ads = adsRepo.GetAll();

			var csvms = (from c in campaigns
						 join p in products on c.Product.Id equals p.Id
						 join b in brands on p.Brand.Id equals b.Id into bp
						 from b in bp.DefaultIfEmpty()
						 join a in advertisers on b.Advertiser.Id equals a.Id into ab
						 from a in ab.DefaultIfEmpty()
						 join ss in subsegments on p.SubSegment.Id equals ss.Id into ssp
						 from ss in ssp.DefaultIfEmpty()
						 join s in segments on ss.Segment.Id equals s.Id into sss
						 from s in sss.DefaultIfEmpty()
						 join v in verticals on s.Vertical.Id equals v.Id into vs
						 from v in vs.DefaultIfEmpty()
						 join r in resources on c.Thumbnail.Id equals r.Id into rc
						 from r in rc.DefaultIfEmpty()
						 join ccs in campaignContentSchemas on c.Id equals ccs.Campaign.Id into ccsc
						 from ccs in ccsc.DefaultIfEmpty()
						 join d in deliveryGroups on c.Id equals d.Campaign.Id into dc
						 from d in dc.DefaultIfEmpty()
						 join m in mediaPartners on (d != null ? (int?)d.MediaPartner.Id : null) equals m.Id into md
						 from m in md.DefaultIfEmpty()
						 join ag in agencies on c.MediaAgency.Id equals ag.Id into agc
						 from ag in agc.DefaultIfEmpty()
						 where
							(mediaAgencyId == null || c.MediaAgency.Id == mediaAgencyId || (ag != null && ag.Parent_Id == mediaAgencyId)) &&
							(advertiserId == null || b.Advertiser.Id == advertiserId) &&
							(mediaPartnerId == null || (d != null && d.MediaPartner.Id == mediaPartnerId) || (m != null && m.Parent_Id == mediaPartnerId))
						 select new CampaignsListingViewModel
						 {
							 Id = c.Id,
							 Name = c.Name,
							 BrandName = b.Name,
							 AdvertiserName = a.Name,
							 VerticalName = v.Name,
							 ProductName = p.Name,
							 Internal = c.Internal,
							 beginDateRaw = ads.Where(aa => aa.Campaign.Id == c.Id).Min(aa => aa.BeginDate),
							 endDateRaw = ads.Where(aa => aa.Campaign.Id == c.Id).Max(aa => aa.EndDate),
							 LastModifiedRaw = c.DateUpdated ?? c.DateCreated,
							 IsFavorite = c.UsersFavorite.Any(y => y.Id == userId),
							 ThumbnailId = r.Id,
							 ThumbnailName = r.Name,
							 ThumbnailFileName = r.Filename,
							 ThumbnailResourceType = r.ResourceType.Id,
							 AgencyId = (ag != null ? ag.Id : (int?)null),
							 MediaPartnerId = (m != null ? m.Id : (int?)null),
							 AdvertiserId = (a != null ? a.Id : (int?)null)
						 }
					).Distinct().OrderBy(c => c.Id).ToList();

			// Distinct won't work for LINQ to Objects (but will work for LINQ to SQL), so need to flatten by grouping and selecting the first item in each group
			var flattenedCampaignList = csvms.GroupBy(c => c.Id).Select(c => c.First()).ToList();

			return flattenedCampaignList;
		}

		#endregion	
	}
}
