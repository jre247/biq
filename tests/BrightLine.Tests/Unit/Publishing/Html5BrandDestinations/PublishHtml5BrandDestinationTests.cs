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
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.ResourceType;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnitAlias = NUnit.Framework;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class PublishHtml5BrandDestinationTests
	{
		#region Variables

		IocRegistration Container;
		Campaign Campaign;
		int AdBrandDesintationRokuId = 100;
		int AdBrandDesintationTvosId = 101;
		int AdCommercialSpotRokuId = 102;
		int AdOverlayDirecTVId = 103;
		int AdCommercialSpotDirecTVId = 104;
		string AdBrandDestinationRokuName = "Test 123";
		string AdBrandDesintationTvosName = "Test 1234";
		string AdCommercialSpotRokuName = "Test 1235";
		string AdOverlayDirecTVName = "Test 1236";
		string AdCommercialSpotDirecTVName = "Test 1237";

		#endregion //Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);

			SetupObjectGraph();
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-514) The Manifest to publish for Html5 Brand Destination contains correct Ads count")]
		public void Publish_Html5BrandDestination_Manifest_Ads_Count_Is_Correct()
		{
			// Arrange
			var cmsPublish = new CmsPublish{Id = 1};
			var Html5BrandDestinationService = IoC.Resolve<IHtml5BrandDestinationService>();

			// Act
			var privateObj = new PrivateObject(Html5BrandDestinationService); // Need to use PrivateObject class because invoking a private method inside service
			var args = new object[2] { Campaign, cmsPublish }; // Passing parameters to GetManifestToPublish method inside service
			var manifest = privateObj.Invoke("GetManifestToPublish", args) as ManifestViewModel;

			// Assert
			NUnitAlias.Assert.AreEqual(manifest.campaign.ads.Count(), 2, "Manifest Campaign Ads Count is not correct.");
		}

		[Test(Description = "(BL-514) The Manifest to publish for Html5 Brand Destination contains correct Ads count")]
		public void Publish_Html5BrandDestination_Manifest_Has_Correct_Ads()
		{
			// Arrange
			var cmsPublish = new CmsPublish { Id = 1 };
			var Html5BrandDestinationService = IoC.Resolve<IHtml5BrandDestinationService>();

			// Act
			var privateObj = new PrivateObject(Html5BrandDestinationService); // Need to use PrivateObject class because invoking a private method inside service
			var args = new object[2] { Campaign, cmsPublish }; // Passing parameters to GetManifestToPublish method inside service
			var manifest = privateObj.Invoke("GetManifestToPublish", args) as ManifestViewModel;

			// Assert
			NUnitAlias.Assert.AreEqual(manifest.campaign.ads.ElementAt(0).ad_id, AdOverlayDirecTVId, "Manifest Campaign Ad is not correct.");
			NUnitAlias.Assert.AreEqual(manifest.campaign.ads.ElementAt(1).ad_id, AdCommercialSpotDirecTVId, "Manifest Campaign Ad is not correct.");
		}

		#endregion 

		#region Helper Methods

		private void SetupObjectGraph()
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var settings = IoC.Resolve<ISettingsService>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();
			var ads = IoC.Resolve<IAdService>();
			var campaigns = IoC.Resolve<ICampaignService>();
			var campaignId = 1;

			// Create Ads
			var adBrandDesintationRoku = CreateAd(campaignId, AdBrandDesintationRokuId, AdBrandDestinationRokuName, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeConstants.AdTypeNames.BrandDestination, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku], PlatformConstants.PlatformNames.Roku, 1);
			var adBrandDesintationTvos = CreateAd(campaignId, AdBrandDesintationTvosId, AdBrandDesintationTvosName, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeConstants.AdTypeNames.BrandDestination, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.TVOS], PlatformConstants.PlatformNames.TVOS, 2);
			var adCommercialSpotRoku = CreateAd(campaignId, AdCommercialSpotRokuId, AdCommercialSpotRokuName, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeConstants.AdTypeNames.CommercialSpot, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku], PlatformConstants.PlatformNames.Roku, 3);
			var adOverlayDirecTV = CreateAd(campaignId, AdOverlayDirecTVId, AdOverlayDirecTVName, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay], AdTypeConstants.AdTypeNames.Overlay, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV], PlatformConstants.PlatformNames.DirecTV, 4);
			var adCommercialSpotDirecTV = CreateAd(campaignId, AdCommercialSpotDirecTVId, AdCommercialSpotDirecTVName, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeConstants.AdTypeNames.BrandDestination, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV], PlatformConstants.PlatformNames.DirecTV, 5);
	
			Campaign = campaigns.Get(campaignId);

			// Set up Campaign to have Creatives to pass Ad Validation
			Campaign.Creatives = new List<Creative>();

			// Set up campaign to have Ads to pass Ad Validation
			Campaign.Ads = new List<Ad>();
			Campaign.Ads.Add(adBrandDesintationRoku);
			Campaign.Ads.Add(adBrandDesintationTvos);
			Campaign.Ads.Add(adCommercialSpotRoku);
			Campaign.Ads.Add(adOverlayDirecTV);
			Campaign.Ads.Add(adCommercialSpotDirecTV);

			// Create Ads
			ads.Create(adBrandDesintationRoku);
			ads.Create(adBrandDesintationTvos);
			ads.Create(adCommercialSpotRoku);
			ads.Create(adOverlayDirecTV);
			ads.Create(adCommercialSpotDirecTV);
		}

		private Ad CreateAd(int campaignId, int adId, string adName, int adTypeId, string adTypeName, int adFunctionId, int platformId, string platformName, int creativeId)
		{
			var campaign = new Campaign
			{
				Creatives = new List<Creative>{new Creative{Id = creativeId, AdType = new AdType{Id = adTypeId}}},
				Ads = new List<Ad> { new Ad { Id = adId } }
			};

			var adBrandDesintationRoku = new Ad
			{
				Id = adId,
				Name = adName,
				AdType = new AdType
				{
					Id = adTypeId,
					Name = adTypeName
				},
				DeliveryGroup = new DeliveryGroup{Id = 1},
				AdTag = new AdTag
				{
					Id = 100
				},
				Creative = new Creative
				{
					Id = 1,
					Name = "Creative 1",
					Description = "Creative Description 1",
					AdFunction = new AdFunction { Id = adFunctionId },
					Features = new List<Feature>(),
					AdType = new AdType { Id = adTypeId },
					Campaign = campaign
				},
				Platform = new Platform
				{
					Id = platformId,
					Name = platformName
				},
				Campaign = campaign
			};
			return adBrandDesintationRoku;
		}

		#endregion
	}
}
