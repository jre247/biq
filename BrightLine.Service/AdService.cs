using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Common.Utility.Enums;
using BrightLine.Service.AdTrackingUrl;
using BrightLine.Common.ViewModels.AdTrackingUrl.IQ;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.Platform;

namespace BrightLine.Service
{
	public class AdService : CrudService<Ad>, IAdService
	{
		public AdService(IRepository<Ad> repo)
			: base(repo)
		{
			Repository = repo;
			Creating += AdService_Saving;
			Updating += AdService_Saving;
			Deleting += AdService_Deleting;
		}

		public Ad SaveAd(AdViewModel adViewModel)
		{
			
			var adId = adViewModel.id;
			var ad = Get(adId);
			if (ad == null)
			{
				ad = new Ad();

				var adTag = new AdTag();
				ad.AdTag = adTag;
			}

			SavePrimaryFieldsForAd(adViewModel, ad);

			SaveTrackingEventsForAd(adViewModel, ad);

			ad = Upsert(ad);

			return ad;
		}

		public List<CampaignAdViewModel> GetAdsForCampaign(Campaign campaign)
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var placementsRepo = IoC.Resolve<IPlacementService>();
			var adTypesRepo = IoC.Resolve<IRepository<AdType>>();
			var adTagsRepo = IoC.Resolve<IRepository<AdTag>>();
			var featuresRepo = IoC.Resolve<IRepository<Feature>>();
			var creativesRepo = IoC.Resolve<ICreativeService>();
			var resourcesRepo = IoC.Resolve<IResourceService>();
			var campaignsRepo = IoC.Resolve<ICampaignService>();

			var brandDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			var ads = GetAll();
			var placements = placementsRepo.GetAll();
			var adTypes = adTypesRepo.GetAll();
			var adTags = adTagsRepo.GetAll();
			var campaigns = campaignsRepo.GetAll();
			var features = featuresRepo.GetAll();
			var creatives = creativesRepo.GetAll();
			var resources = resourcesRepo.GetAll();

			var adsViewModel = (from a in ads
								join p in placements on a.Placement.Id equals p.Id
								join c in campaigns on a.Campaign.Id equals c.Id
								join at in adTypes on a.AdType.Id equals at.Id
								join atg in adTags on (a != null ? a.AdTag.Id : (int?)null) equals atg.Id into atga
								from atg in atga.DefaultIfEmpty()
								join cr in creatives on (a != null ? a.Creative.Id : (int?)null) equals cr.Id into cra
								from cr in cra.DefaultIfEmpty()
								where c.Id == campaign.Id && (a != null && a.AdType.Id != brandDestinationAdTypeId)
								let resource = resources.Where(r => r.Creative.Id == cr.Id).OrderByDescending(r => r.DateCreated).FirstOrDefault()
								select new CampaignAdViewModel
								{
									id = a.Id,
									name = a.Name,
									lastModifiedRaw = a.DateUpdated ?? a.DateCreated,
									isDeleted = a.IsDeleted,
									creativeId = cr.Id,
									adTypeId = at.Id,
									adTypeName = at.Name,
									campaignId = c.Id,
									adTag = (atg != null ? atg.Id : (int?)null),
									impressions = 0, // hardcoded to 0 for now. In the future we need to get impressions for ad
									platformId = a.Platform.Id,
									placementId = p.Id,
									resourceId = resource.Id,
									resourceName = resource.Name,
									resourceFilename = resource.Filename,
									resourceType = resource.ResourceType.Id,
									deliveryGroupId = a != null && a.DeliveryGroup != null ? a.DeliveryGroup.Id : (int?)null,
									isPromo = at.IsPromo,
									beginDateRaw = a.BeginDate,
									endDateRaw = a.EndDate,
									mediaPartnerId = p.MediaPartner.Id

								}
			).ToList();

			SetUrlsForAds(adsViewModel, campaign);

			return adsViewModel.Distinct().ToList();
		}

		#region Cleanup Dirty Ads

		/// <summary>
		/// Clean up Ads that have dirty data that would not pass the current Ad Validation.
		/// </summary>
		public void CleanupDirtyAds()
		{
			var adsService = IoC.Resolve<IAdService>();
			var adFunctions = IoC.Resolve<IRepository<AdFunction>>();

			var ads = adsService.Where(a => a.AdTag == null).ToList();
			foreach (var ad in ads)
			{
				// Create new AdTag for this Ad
				var adTag = new AdTag();
				ad.AdTag = adTag;

				// Set Ad Name
				if (string.IsNullOrEmpty(ad.Name))
					ad.Name = string.Format("Unnamed_{0}", Guid.NewGuid().ToString()); // Create unique Ad Name;

				// Set Creative
				SetCreativeForDirtyAd(ad);

				// Set AdFunction
				if(ad.Creative.AdFunction == null)
					ad.Creative.AdFunction = adFunctions.Get(2); // Set AdFunction Interactive

				// Set AdType if AdTypeGroup isn't correct
				SetAdTypeForDirtyAd(ad);

				// Make sure Campaign is same between Ad and Ad's Creative
				if (ad.Creative.Campaign.Id != ad.Campaign.Id)
					ad.Campaign = ad.Creative.Campaign;

				// Set Delivery Group
				SetDeliveryGroupForDirtyAd(ad);
					
				// Set Destination Creative
				SetDestinationCreativeForDirtyAd(ad);

				adsService.Update(ad);
			}
		}

		#endregion

		#region Private Methods

		#region Cleanup Dirty Ads

		private void SetDestinationCreativeForDirtyAd(Ad ad)
		{
			var adFunctions = IoC.Resolve<IRepository<AdFunction>>();
			var adFormats = IoC.Resolve<IRepository<AdFormat>>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();

			var clickToJump = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump];

			if (ad.Creative.AdFunction.Id == clickToJump)
			{
				if (ad.DestinationCreative == null)
				{
					var brandDestination = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

					var creative = new Creative
					{
						Campaign = ad.Campaign,
						AdType = adTypes.Get(brandDestination),
						AdFormat = adFormats.Get(1), // Set AdFormat Horizontal
						AdFunction = adFunctions.Get(2), // Set AdFunction Interactive
						Name = ad.Name,
						Features = new List<Feature>()
					};
					creative = creatives.Create(creative);

					// Assign Destination Creative for Ad
					ad.DestinationCreative = creative.Id;

					// Add Creative to Ad's Campaign
					ad.Campaign.Creatives.Add(creative);
				}
			}
		}

		private void SetDeliveryGroupForDirtyAd(Ad ad)
		{
			var deliveryGroups = IoC.Resolve<ICrudService<DeliveryGroup>>();

			var imageBanner = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner];
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];

			if (ad.DeliveryGroup == null)
			{
				if (ad.AdType.Id == overlay || ad.AdType.Id == imageBanner)
				{
					var deliveryGroup = new DeliveryGroup
					{
						Name = "Generic " + ad.Placement.MediaPartner.Name,
						MediaPartner = ad.Placement.MediaPartner
					};

					// Assign new Delivery Group to Ad
					ad.DeliveryGroup = deliveryGroups.Create(deliveryGroup);
				}
			}
		}

		private void SetAdTypeForDirtyAd(Ad ad)
		{
			var adsService = IoC.Resolve<IAdService>();
			var adFunctions = IoC.Resolve<IRepository<AdFunction>>();
			var adFormats = IoC.Resolve<IRepository<AdFormat>>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var adTypeGroups = IoC.Resolve<IRepository<AdTypeGroup>>();
			var creatives = IoC.Resolve<ICreativeService>();

			// Set Ad's AdType to be the same as the AdType for the Ad's Creative
			if (ad.AdType.Id != ad.Creative.AdType.Id)
				ad.AdType = ad.Creative.AdType;

			if (ad.Creative.AdType.Id != ad.Creative.AdType_Id)
				ad.Creative.AdType = adTypes.Get(ad.Creative.AdType_Id.Value);

			if (ad.Placement != null)
			{
				if (!ad.AdType.AdTypeGroups.Contains(ad.Placement.AdTypeGroup))
				{
					// Assign AdType to be associated with the AdTypeGroup of the Ad's Placement
					var adTypeGroup = adTypeGroups.Where(g => g.Id == ad.Placement.AdTypeGroup.Id).First();
					var adType = adTypeGroup.AdTypes.First();
					ad.AdType = adType;
					ad.Creative.AdType = adType;

					// Now create a creative with this AdType if it doesn't exist
					var creative = creatives.Where(c => c.AdType.Id == ad.AdType.Id).FirstOrDefault();
					if (creative == null)
					{
						creative = new Creative
						{
							Campaign = ad.Campaign,
							AdType = ad.AdType,
							AdFormat = adFormats.Get(1), // Set AdFormat Horizontal
							AdFunction = adFunctions.Get(2), // Set AdFunction Interactive
							Name = string.Format("Unnamed_{0}", Guid.NewGuid().ToString()), // Create unique Ad Name;
							Features = new List<Feature>()
						};
						ad.Creative = creatives.Create(creative);

						// Add Creative to Ad's Campaign
						ad.Campaign.Creatives.Add(creative);
					}
				}
			}
		}

		private void SetCreativeForDirtyAd(Ad ad)
		{
			var adFunctions = IoC.Resolve<IRepository<AdFunction>>();
			var adFormats = IoC.Resolve<IRepository<AdFormat>>();
			var creatives = IoC.Resolve<ICreativeService>();

			if (ad.Creative == null)
			{
				var creative = new Creative
				{
					Campaign = ad.Campaign,
					AdType = ad.AdType,
					AdFormat = adFormats.Get(1), // Set AdFormat Horizontal
					AdFunction = adFunctions.Get(2), // Set AdFunction Interactive
					Name = ad.Name,
					Features = new List<Feature>()
				};
				ad.Creative = creatives.Create(creative);

				// Add Creative to Ad's Campaign
				ad.Campaign.Creatives.Add(creative);
			}
		}

		#endregion

		private void AdService_Saving(object sender, CrudBeforeEventArgs args)
		{
			var ad = sender as Ad;
			if (ad == null)
				throw new ArgumentNullException("Ad cannot be null.");

			var vex = new ValidationException();

			var adValidationService = IoC.Resolve<IAdValidationService>();

			adValidationService.ValidateAd(vex, ad);

			if (vex.HasErrors)
			{
				args.Cancel = true;
				throw vex;
			}

			if (ad.AdFunction == null)
				ad.AdFunction = ad.Creative.AdFunction;

			ad.AdFormat = ad.Creative.AdFormat;

			if (ad.DestinationCreative != null)
				AssociateDestinationAdWithCurrentAd(ad);
		}

		/// <summary>
		// If there is no Brand Destination ad for the chosen platform, create the destination ad and associated the current ad with the newly created destination ad. 
		// If there is a destination ad for the given create/platform combination, associate the current ad with that destination ad.
		/// </summary>
		/// <param name="ad"></param>
		private void AssociateDestinationAdWithCurrentAd(Ad ad)
		{
			var creatives = IoC.Resolve<ICreativeService>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var adTags = IoC.Resolve<IAdTagService>();
			var brandDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];

			// Do not create a Destination Ad for a Commercial Spot Ad
			if(ad.AdType.Id == commercialSpotAdTypeId)
				return;

			var brandDestinationAdType = adTypes.Get(brandDestinationAdTypeId);		
			var destinationCreative = creatives.Get(ad.DestinationCreative.Value);

			//find an ad where its platform and creative are the same as the original ad's platform and destination creative value
			//*note: there should only be one brand destination ad per creative, per platform
			var brandDestinationAd = Where(a => a.Id != ad.Id && a.Platform.Id == ad.Platform.Id && a.Creative.Id == destinationCreative.Id).SingleOrDefault();

			//if original ad is not associated with a destination ad then create a destination ad and associate it with the original ad
			if (brandDestinationAd == null)
				brandDestinationAd = CreateBrandDestinationAd(ad, adTags, brandDestinationAdType, destinationCreative, brandDestinationAd);

			//associate the current ad with that destination ad
			ad.DestinationAd = brandDestinationAd;
		}

		private Ad CreateBrandDestinationAd(Ad ad, IAdTagService adTags, AdType brandDestinationAdType, Creative destinationCreative, Ad brandDestinationAd)
		{
			//concatenate ad's destination creative name and platform name to form an ad name that we know will be unique
			var brandDestinationAdName = string.Format("{0}:{1}:{2}", destinationCreative.Name, ad.Platform.Name, Guid.NewGuid());

			var destinationAdTypeGroup = Lookups.AdTypeGroups.HashByName[AdTypeGroupConstants.AdTypeGroupNames.Destination];

			var adTag = new AdTag();
			adTag = adTags.Create(adTag);

			brandDestinationAd = new Ad
			{
				Platform = ad.Platform,
				AdType = brandDestinationAdType,
				AdTypeGroup_Id = destinationAdTypeGroup,
				BeginDate = ad.BeginDate,
				EndDate = ad.EndDate,
				Campaign = ad.Campaign,
				Creative = destinationCreative,
				DeliveryGroup = ad.DeliveryGroup,
				Name = brandDestinationAdName,
				AdTag = adTag
			};
			brandDestinationAd = Create(brandDestinationAd);
			return brandDestinationAd;
		}

		private void SavePrimaryFieldsForAd(AdViewModel adViewModel, Ad ad)
		{
			var campaigns = IoC.Resolve<ICampaignService>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var platforms = IoC.Resolve<IRepository<Platform>>();
			var placements = IoC.Resolve<IPlacementService>();
			var adTypeGroups = IoC.Resolve<IRepository<AdTypeGroup>>();
			var deliveryGroups = IoC.Resolve<IRepository<DeliveryGroup>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var adFormats = IoC.Resolve<IRepository<AdFormat>>();

			ad.Creative = creatives.Get(adViewModel.creativeId);
			ad.AdFormat = adFormats.Get(adViewModel.adFormatId);
			ad.AdTypeGroup = adTypeGroups.Get(adViewModel.adTypeGroupId);

			if (adViewModel.companionAdId.HasValue)
				ad.CompanionAd = Get(adViewModel.companionAdId.Value);

			ad.DestinationCreative = adViewModel.destinationCreativeId;
			ad.AdType = adTypes.Get(adViewModel.adTypeId);
			ad.Campaign = campaigns.Get(adViewModel.campaignId);
			ad.BeginDate = DateTime.Parse(adViewModel.beginDate);
			ad.EndDate = !string.IsNullOrEmpty(adViewModel.endDate) ? DateTime.Parse(adViewModel.endDate) : (DateTime?)null;
			ad.Platform = platforms.Get(adViewModel.platformId);
			ad.Placement = placements.Get(adViewModel.placementId);
			ad.DeliveryGroup = deliveryGroups.Get(adViewModel.deliveryGroupId);
			ad.Name = adViewModel.name;
			ad.IsReported = adViewModel.isReported;
			ad.XCoordinateHd = adViewModel.xCoordinateHd;
			ad.YCoordinateHd = adViewModel.yCoordinateHd;
			ad.XCoordinateSd = adViewModel.xCoordinateSd;
			ad.YCoordinateSd = adViewModel.yCoordinateSd;
		}

		private void SaveTrackingEventsForAd(AdViewModel adViewModel, Ad ad)
		{
			var adTrackingEvents = IoC.Resolve<IAdTrackingEventService>();
			var trackingEvents = IoC.Resolve<IRepository<TrackingEvent>>();
			var newAdTrackingEvents = new List<AdTrackingEvent>();

			var trackingEventsHash = trackingEvents.GetAll().ToList().ToDictionary(t => t.Id, t => t);

			// delete existing AdTrackingEvent records that match on Ad Id 
			if (ad.Id > 0)
			{
				var adTrackingEventsToDelete = adTrackingEvents.Where(a => a.Ad.Id == ad.Id).Select(a => a.Id).ToList();
				adTrackingEventsToDelete.ForEach(a => adTrackingEvents.Delete(a));
			}

			// insert new AdTrackingEvent records
			foreach (var adTrackingEvent in adViewModel.adTrackingEvents)
			{
				var adTrackingEventNew = new AdTrackingEvent();

				adTrackingEventNew.Ad = ad;
				var trackingEvent = trackingEventsHash[adTrackingEvent.trackingEventId];
				adTrackingEventNew.TrackingEvent_Id = trackingEvent.Id;
				adTrackingEventNew.TrackingUrl = adTrackingEvent.trackingUrl;

				newAdTrackingEvents.Add(adTrackingEventNew);
			}

			ad.AdTrackingEvents = newAdTrackingEvents;
		}

		private void AdService_Deleting(object sender, CrudBeforeEventArgs e)
		{
			//TODO: business rules for deleting Ads.
		}

		/// <summary>
		/// Set Tracking Urls and Ad Tag Url for a list of Ads
		/// </summary>
		/// <param name="adsViewModel"></param>
		private void SetUrlsForAds(List<CampaignAdViewModel> adsViewModel, Campaign campaign)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var adTagUrlGenerator = new AdTagUrlGenerator();
			var adTrackingUrlGenerator = new IQAdTrackingUrlGenerator();
			var commercialSpotAdType = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];

			// Set Tracking Urls and Ad Tag Url for Ads
			foreach (var adViewModel in adsViewModel)
			{
				var adId = adViewModel.id;
				var adTypeId = adViewModel.adTypeId;
				var adTagId = adViewModel.adTag.ToString();
				var platformId = adViewModel.platformId.Value;
				var placementId = adViewModel.placementId.Value;

				// Generate Ad Tag Url
				if (adViewModel.adTag.HasValue)
				{
					var adTagUrl = adTagUrlGenerator.Generate(adTagId, platformId);

					// Tack on mblist query param to Ad Tag for Roku Overlay
					if (adTypeId == overlay && platformId == roku)
						adTagUrl = string.Format("{0}&{1}={2}", adTagUrl, AdTagUrlConstants.QueryParams.MBList, settings.MBList);

					adViewModel.adTagUrl = adTagUrl;
				}
					
				// Generate Impression Url
				var impressionUrlViewModel = new IQAdTrackingUrlViewModel(AdTagUrlConstants.IQ.Types.Impression, true, adId, placementId);
				adViewModel.impressionUrl = adTrackingUrlGenerator.Generate(impressionUrlViewModel);

				// Generate Click Url			
				var adClickUrlViewModel = new IQAdTrackingUrlViewModel(AdTagUrlConstants.IQ.Types.AdClick, true, adId, placementId);
				adViewModel.clickUrl = adTrackingUrlGenerator.Generate(adClickUrlViewModel);
			}
		}


		#endregion


	}
}
