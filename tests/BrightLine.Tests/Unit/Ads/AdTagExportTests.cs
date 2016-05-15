using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BrightLine.Tests.Common;
using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.CMS.Services;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using BrightLine.Service;
using BrightLine.Common.Services;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Utility;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Web.Areas.Campaigns.Controllers;
using BrightLine.Common.Utility.AdType;
using System.Web;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.ViewModels.AdTrackingUrl.IQ;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class AdTagExportTests
	{
		#region Members

		IocRegistration Container;
		private IAdTagsExportService AdTagsExportService { get;set;}
		private ICampaignService Campaigns { get; set; }
		private ISettingsService Settings { get;set;}
		int CampaignId = 1;

		#endregion

		#region Init

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);

			Container.Register<IFlashMessageExtensions, FlashMessageExtensions>();

			AdTagsExportService = IoC.Resolve<IAdTagsExportService>();
			Campaigns = IoC.Resolve<ICampaignService>();
			Settings = IoC.Resolve<ISettingsService>();

			SetupObjectGraph();
		}

		#endregion

		#region Tests

		[Test(Description = "Ad Tag Export gets correct ads for campaign.")]
		public void AdTagExport_Gets_Correct_Ads_For_Campaign()
		{
			var campaign = Campaigns.Get(1);

			var campaignAds = AdTagsExportService.GetCampaignAds(campaign);

			Assert.IsNotNull(campaignAds, "Campaign Ads is null.");
			Assert.IsTrue(campaignAds.Count() == 4, "Campaign Ads count is not correct.");
		}

		[Test(Description = "Ad Tag Export creates correct Ad Tag Url.")]
		public void AdTagExport_Creates_Correct_AdTag_Url_For_Roku()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			// Get Roku Ad specifically
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var rokuAd = adsWithUrls.Where(a => a.PlatformId == roku).First();

			var adTagId = rokuAd.AdTagId;
			var adTag = rokuAd.AdTag;
			var adTagCompare = string.Format("{0}/?id={1}&{2}&{3}", Settings.AdServerUrl, adTagId, AdTagUrlConstants.RokuAdIdMacro, AdTagExportConstants.AdTagUrlPostfix);
			Assert.IsTrue(adTag == adTagCompare);
		}

		[Test(Description = "Ad Tag Export creates correct Ad Tag Url.")]
		public void AdTagExport_Creates_Correct_AdTag_Url_For_Html5()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			// Get Html5 Ad specifically
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var html5Ad = adsWithUrls.Where(a => a.PlatformId != roku).First();

			var adTagId = html5Ad.AdTagId;
			var adTag = html5Ad.AdTag;
			var adTagCompare = string.Format("{0}/?id={1}&{2}", Settings.AdServerUrl, adTagId, AdTagExportConstants.AdTagUrlPostfix);
			Assert.IsTrue(adTag == adTagCompare);
		}

		[Test(Description = "Ad Tag Export creates correct Ad Tag Url for Commercial Spot Ad Type.")]
		public void AdTagExport_Creates_Correct_AdTag_Url_For_CommercialSpot_AdType()
		{
			var campaign = Campaigns.Get(CampaignId);
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var ads = AdTagsExportService.GetCampaignAds(campaign);
			var tvos = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.TVOS];
			ads.Add(new AdTagExportViewModel { AdTypeId = commercialSpotAdTypeId, PlatformId = tvos, AdId = 100, AdTagId = 1000 }); //add ad to list that had Commercial Spot Ad Type

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adTagId = lastAd.AdTagId;
			var adTag = lastAd.AdTag;
			var adTagCompare = string.Format("{0}/?id={1}&{2}", Settings.AdServerUrl, adTagId, AdTagExportConstants.AdTagUrlPostfix);
			Assert.IsTrue(adTag == adTagCompare);
		}

		[Test(Description = "Ad Tag Export creates correct Impression Url.")]
		public void AdTagExport_Creates_Correct_Impression_Url()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var adId = adsWithUrls[0].AdId;
			var impressionUrl = adsWithUrls[0].ImpressionUrl;

			// Deserialize Impression Url to POCO
			string trackingUrlBase;
			IQAdTrackingUrlViewModel trackingUrlDataDeserialized;
			GetDeserializedTrackingUrl(impressionUrl, out trackingUrlBase, out trackingUrlDataDeserialized);

			// Assert Tracking Url Data properties (after being deserialized)
			Assert.AreEqual(trackingUrlDataDeserialized.ad_id, adId, "Impression Url ad_id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.type, AdTagUrlConstants.IQ.Types.Impression, "Impression Url type is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.valid, true, "Impression Url valid is not correct.");

			// Assert Base Tracking Url
			Assert.AreEqual(trackingUrlBase, Settings.TrackingUrl, "Impression Base TrackingUrl is not correct.");
		}

		[Test(Description = "Ad Tag Export has a redirect parameter in it.")]
		public void AdTagExport_Impression_Url_Has_Redirect_Param()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			// Add an ad that will have a Samsung AdHub First Tile Placement
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var samsungAdHubFirstTile = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile];
			ads.Add(new AdTagExportViewModel { AdTypeId = overlay, PlatformId = samsung, PlacementId = samsungAdHubFirstTile, AdId = 100, AdTagId = 1000 }); //add ad to list that had Commercial Spot Ad Type

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adId = lastAd.AdId;
			var impressionUrl = lastAd.ImpressionUrl;
		
			// Assert Redirect param exists in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsTrue(impressionUrl.Contains(redirectParam), "Impression Url does not have the Redirect param.");
		}

		[Test(Description = "Ad Tag Export does not have a redirect parameter in it.")]
		public void AdTagExport_Impression_Url_Does_Not_Have_Redirect_Param()
		{
			var campaign = Campaigns.Get(CampaignId);
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			// Add an ad that will not have a Samsung AdHub First Tile Placement
			// So a Redirect param should not be generated for this ad
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var cnbcHorizontalBanner = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner];
			ads.Add(new AdTagExportViewModel { AdTypeId = overlay, PlatformId = samsung, PlacementId = cnbcHorizontalBanner, AdId = 100, AdTagId = 1000 });

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adId = lastAd.AdId;
			var impressionUrl = lastAd.ImpressionUrl;

			// Assert Redirect param does not exist in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsFalse(impressionUrl.Contains(redirectParam), "Impression Url should not have the Redirect param.");
		}

		[Test(Description = "Ad Tag Export creates correct Click Url for non-Commercial Spot ads.")]
		public void AdTagExport_Creates_Correct_Click_Url()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var adId = adsWithUrls[0].AdId;
			var clickUrl = adsWithUrls[0].ClickUrl;

			// Deserialize Click Url to POCO
			string trackingUrlBase;
			IQAdTrackingUrlViewModel trackingUrlDataDeserialized;
			GetDeserializedTrackingUrl(clickUrl, out trackingUrlBase, out trackingUrlDataDeserialized);

			// Assert Tracking Url Data properties (after being deserialized)
			Assert.AreEqual(trackingUrlDataDeserialized.ad_id, adId, "Click Url ad_id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.type, AdTagUrlConstants.IQ.Types.AdClick, "Click Url type is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.valid, true, "Click Url valid is not correct.");

			// Assert Base Tracking Url
			Assert.AreEqual(trackingUrlBase, Settings.TrackingUrl, "Impression Base TrackingUrl is not correct.");
		}

		[Test(Description = "Ad Tag Export has Redirect param in its ClickUrl.")]
		public void AdTagExport_ClickUrl_Has_Redirect_Param()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			// Add an ad that will have a Samsung AdHub First Tile Placement
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var samsungAdHubFirstTile = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile];
			ads.Add(new AdTagExportViewModel { AdTypeId = overlay, PlatformId = samsung, PlacementId = samsungAdHubFirstTile, AdId = 100, AdTagId = 1000 }); //add ad to list that had Commercial Spot Ad Type

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adId = lastAd.AdId;
			var clickUrl = lastAd.ClickUrl;

			// Assert Redirect param exists in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsTrue(clickUrl.Contains(redirectParam), "Click Url does not have the Redirect param.");
		}

		[Test(Description = "Ad Tag Export does not have Redirect param in its ClickUrl.")]
		public void AdTagExport_ClickUrl_Does_Not_Have_Redirect_Param()
		{
			var campaign = Campaigns.Get(CampaignId);
			var ads = AdTagsExportService.GetCampaignAds(campaign);

			// Add an ad that will have a Samsung AdHub First Tile Placement
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var cnbcHorizontalBanner = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner];
			ads.Add(new AdTagExportViewModel { AdTypeId = overlay, PlatformId = samsung, PlacementId = cnbcHorizontalBanner, AdId = 100, AdTagId = 1000 }); //add ad to list that had Commercial Spot Ad Type

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adId = lastAd.AdId;
			var clickUrl = lastAd.ClickUrl;

			// Assert Redirect param does not exist in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsFalse(clickUrl.Contains(redirectParam), "Click Url should not have the Redirect param.");
		}

		[Test(Description = "Ad Tag Export does not create Click Url for Commercial Spot ads.")]
		public void AdTagExport_Does_Not_Create_Click_Url_For_Commercial_Spot_Ads()
		{
			var campaign = Campaigns.Get(CampaignId);
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var ads = AdTagsExportService.GetCampaignAds(campaign);
			ads.Add(new AdTagExportViewModel { AdTypeId = commercialSpotAdTypeId, PlatformId = commercialSpotAdTypeId, AdId = 100, AdTagId = 1000 }); //add ad to list that had Commercial Spot Ad Type

			var adsWithUrls = AdTagsExportService.SetAdTagAndUrls(ads, campaign);

			var lastAd = adsWithUrls.Last();
			var adId = lastAd.AdId;
			var clickUrl = lastAd.ClickUrl;

			Assert.IsNull(clickUrl);
		}

		[Test(Description = "Ad Tag Export does not throw any exceptions with an end to end test.")]
		public void AdTagExport_End_To_End_Test()
		{
			var campaignsApiController = new CampaignsApiController();
			campaignsApiController.ExportAdTags(1);
		}

		#endregion

		#region Helper Methods

		private void SetupObjectGraph()
		{
			int adId1 = 3;
			var platformId1 = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var platformName1 = PlatformConstants.PlatformNames.Samsung;
			var adTypeId1 = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner];
			var adTypeName1 = AdTypeConstants.AdTypeNames.ImageBanner;
			CreateAd(adId1, platformId1, platformName1, adTypeId1, adTypeName1);

			var adId2 = 4;
			var platformId2 = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var platformName2 = PlatformConstants.PlatformNames.Roku;
			var adTypeId2 = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner];
			var adTypeName2 = AdTypeConstants.AdTypeNames.ImageBanner;
			CreateAd(adId2, platformId2, platformName2, adTypeId2, adTypeName2);
		}

		// TODO: Put this CreateAd in a AdHelper class in Test project
		private void CreateAd(int adId, int platformId, string platformName, int adTypeId, string adTypeName)
		{
			var adType = new AdType { Id = adTypeId, Name = adTypeName, AdTypeGroups = new List<AdTypeGroup> { new AdTypeGroup { Id = 1 } } };
			var creative = new Creative
			{
				Id = 1,
				AdType = adType,
				AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] },
				Campaign = new Campaign { Id = CampaignId },
				Features = new List<Feature>()
			};

			// Create an Ad
			var ad = new Ad
			{
				Id = adId,
				Name = "Test",
				AdType = adType,
				AdTag = new AdTag { Id = 1 },
				Creative = creative,
				Platform = new Platform { Id = platformId, Name = platformName },
				Campaign = new Campaign { Id = CampaignId, Ads = new List<Ad>() },
				DeliveryGroup = new DeliveryGroup { Id = 1},
				DestinationAd = new Ad { Id = 2 },
				Placement = new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile], Name = PlacementConstants.Names.SamsungAdHubFirstTile, AdTypeGroup = new AdTypeGroup { Id = 1 } }
			};
			var ads = IoC.Resolve<IAdService>();

			// Add Ads to Campaign to pass Ad Validation
			var campaign = Campaigns.Get(CampaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(ad);

			// Setup Ad's Campaign
			ad.Campaign.Ads.Add(ad);
			ad.Campaign.Creatives = new List<Creative>();
			ad.Campaign.Creatives.Add(creative);

			// Setup Ad's Creative
			ad.Creative = creative;

			// Make sure Ad's Campaign has Creative with same Ad's AdType
			campaign.Creatives = new List<Creative>();

			// Save new Ad
			ads.Create(ad);
			ads.Save();
		}

		private void GetDeserializedTrackingUrl(string trackingUrl, out string trackingUrlBase, out IQAdTrackingUrlViewModel trackingUrlDataDeserialized)
		{
			var trackingUrlDecoded = HttpUtility.UrlDecode(trackingUrl);
			var trackingUrlSplit = trackingUrlDecoded.Split(new string[] { "?data=" }, StringSplitOptions.None);
			trackingUrlBase = trackingUrlSplit[0];
			var trackingUrlData = trackingUrlSplit[1];
			trackingUrlDataDeserialized = JsonConvert.DeserializeObject<IQAdTrackingUrlViewModel>(trackingUrlData);
		}

		#endregion
	}
}
