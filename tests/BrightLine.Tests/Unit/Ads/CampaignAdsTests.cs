using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Service.External.AmazonStorage;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Web.Areas.Campaigns.Controllers;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class CampaignAdsTests
	{
		IocRegistration Container;
		private int CampaignId = 2;
		private int UserId = 1;
		private Mock<IResourceHelper> ResourceHelper { get; set; }
		private Stream FileStream;
		private CampaignService CampaignService { get; set; }
		private Resource Resource1 { get; set; }
		private Resource Resource2 { get; set; }
		private Resource Resource3 { get; set; }
		private const string Md5Hash = "abc-123";

		[SetUp]
		public void Setup()
		{
			MockHelper.BuildMockLookups();

			var dateTime1 = new DateTime(2016, 5, 4);
			var dateTime2 = new DateTime(2016, 5, 14);
			var creativeId1 = 1;
			var creativeId2 = 2;
			var creativeId3 = 3;

			Resource1 = MockEntities.CreateResource(5, "blueberries1", "blueberries1.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ResourceTypeConstants.ResourceTypeNames.SdVideo, 2, 3, 4, 5, creativeId1, CampaignId, dateTime1);
			Resource2 = MockEntities.CreateResource(6, "blueberries2", "blueberries2.png", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Png], FileTypeConstants.FileTypeNames.Png, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, creativeId2, CampaignId, dateTime2);
			Resource3 = MockEntities.CreateResource(7, "blueberries3", "blueberries3.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Jpg], FileTypeConstants.FileTypeNames.Jpg, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, creativeId3, 543534, dateTime2);

			Container = new IocRegistration();

			Container.RegisterSingleton(() => MockBuilder<Resource>.GetPopulatedRepository(GetResources));
			Container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(GetCampaigns));
			Container.RegisterSingleton(() => MockBuilder<AdFunction>.GetPopulatedRepository(GetAdFunctions));
			Container.RegisterSingleton(() => MockBuilder<AdType>.GetPopulatedRepository(GetAdTypes));
			Container.RegisterSingleton(() => MockBuilder<Creative>.GetPopulatedRepository(GetCreatives));
			Container.RegisterSingleton(() => MockBuilder<Feature>.GetPopulatedRepository(GetFeatures));
			Container.RegisterSingleton(() => MockBuilder<CampaignContentSchema>.GetPopulatedRepository(GetCampaignContentSchemas));
			Container.RegisterSingleton(() => MockBuilder<Placement>.GetPopulatedRepository(GetPlacements));
			Container.RegisterSingleton(() => MockBuilder<Ad>.GetPopulatedRepository(GetAds));
			Container.RegisterSingleton(() => MockBuilder<AdTag>.GetPopulatedRepository(GetAdTags));

			Container.Register<IUserService, UserService>();
			Container.Register<ICampaignService, CampaignService>();
			Container.Register<IRoleService, RoleService>();
			Container.Register<IResourceService, ResourceService>();
			Container.Register<ICreativeService, CreativeService>();
			Container.Register<IPlacementService, PlacementService>();
			Container.Register<IAdService, AdService>();
			Container.Register<ISettingsService, MockSettingService>();
			Container.Register<ICampaignLookupsService, CampaignLookupsService>();
			Container.Register<ICampaignPermissionsService, CampaignPermissionsService>();
			Container.Register<ICampaignsListingService, CampaignsListingService>();
			Container.Register<ICampaignSummaryService, CampaignSummaryService>();

			var ResourceHelper = new Mock<IResourceHelper>();
			var fileStream = new Mock<FileStream>();
			ResourceHelper.Setup(c => c.GenerateMd5HashForFile(FileStream)).Returns(Md5Hash);
			Container.Register<IResourceHelper>(() => ResourceHelper.Object);

			IocSetup.Setup(Container);

			var repo = IoC.Resolve<IRepository<Campaign>>();
			CampaignService = new CampaignService(repo);

		}

		[Test]
		public void Ads_Listing_No_Destination_Ads_Returned()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId = 1;

			var ads = CampaignService.GetAds(campaignId);
			var destinationAds = ads.FindAll(a => a.adTypeId == Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]);

			Assert.IsEmpty(destinationAds);
		}

		[Test]
		public void Ads_Listing_Promo_Ads_Returned()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId = 2;

			var ads = CampaignService.GetAds(campaignId);
			var promoAds = ads.FindAll(a => a.adTypeId != Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]);

			Assert.IsTrue(promoAds.Count > 0);
		}

		[Test]
		public void Ads_Listing_Properties_Are_Correct_For_Campaign_Ad_Html5()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var creativeId = 1;
			var adTag = 2;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var placement = 2;
			var deliveryGroup = 2;
			var resourceId = 5;
			var adId = 2;
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);
			var adTagUrl = string.Format("{0}/?id={1}&{2}", settings.AdServerUrl, adTag, AdTagExportConstants.AdTagUrlPostfix);

			var ads = CampaignService.GetAds(campaignId);
			var ad = ads.Find(a => a.id == adId);

			AssertAdPropertyValues(ad, adId, "Ad 2", false, creativeId, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeConstants.AdTypeNames.CommercialSpot, campaignId, adTag, platformId, placement,
				 resourceId, "blueberries1", "blueberries1.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], deliveryGroup, true, adTagUrl);
		}

		[Test]
		public void Ads_Listing_Properties_Are_Correct_For_Campaign_Ad_Roku()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var creativeId = 1;
			var adTag = 1;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var placement = 1;
			var deliveryGroup = 1;
			var resourceId = 5;
			var adId = 3;
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);
			var adTagUrl = string.Format("{0}/?id={1}&{2}&{3}", settings.AdServerUrl, adTag, AdTagUrlConstants.RokuAdIdMacro, AdTagExportConstants.AdTagUrlPostfix);

			var ads = CampaignService.GetAds(campaignId);
			var ad = ads.Find(a => a.id == adId);

			AssertAdPropertyValues(ad, adId, "Ad 3", false, creativeId, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeConstants.AdTypeNames.CommercialSpot, campaignId, adTag, platformId, placement,
				 resourceId, "blueberries1", "blueberries1.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], deliveryGroup, true, adTagUrl);
		}

		[Test]
		public void Ads_Listing_ImpressionUrl_Has_Redirect_Param()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var ads = CampaignService.GetAds(campaignId);
			var samsungAdHubFirstTile = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile];
			var ad = ads.Find(a => a.placementId == samsungAdHubFirstTile);

			// Assert that Redirect param exists in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsTrue(ad.impressionUrl.Contains(redirectParam), "Impression Url does not have the Redirect param.");
		}

		[Test]
		public void Ads_Listing_ImpressionUrl_Does_Not_Have_Redirect_Param()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var ads = CampaignService.GetAds(campaignId);
			var cnbcHorizontalBanner = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner];
			var ad = ads.Find(a => a.placementId == cnbcHorizontalBanner);

			// Assert Redirect param does not exist in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsFalse(ad.impressionUrl.Contains(redirectParam), "Impression Url should not have the Redirect param.");
		}

		[Test]
		public void Ads_Listing_ClickUrl_Has_Redirect_Param()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var ads = CampaignService.GetAds(campaignId);
			var samsungAdHubFirstTile = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile];
			var ad = ads.Find(a => a.placementId == samsungAdHubFirstTile);

			// Assert Redirect param exists in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsTrue(ad.clickUrl.Contains(redirectParam), "Click Url does not have the Redirect param.");
		}

		[Test]
		public void Ads_Listing_ClickUrl_Should_Not_Have_Redirect_Param()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var campaignId = 1;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var ads = CampaignService.GetAds(campaignId);
			var cnbcHorizontalBanner = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner];
			var ad = ads.Find(a => a.placementId == cnbcHorizontalBanner);

			// Assert Redirect param does not exist in the impression url
			var redirectParam = string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);
			Assert.IsFalse(ad.clickUrl.Contains(redirectParam), "Click Url does not have the Redirect param.");
		}

		#region Private Methods

		private void AssertAdPropertyValues(CampaignAdViewModel ad, int id, string name, bool isDeleted, int creativeId, int adTypeId, string adTypeName, int campaignId, int adTag, int platformId, int placementId, int resourceId,
			string resourceName, string resourceFilename, int resourceType, int deliveryGroupId, bool isPromo, string adTagUrl)
		{
			Assert.That(ad.id == id, "Creative Id is not correct.");
			Assert.That(ad.name == name, "Creative Name is not correct.");
			Assert.That(ad.isDeleted == isDeleted, "Creative IsDeleted is not correct.");
			Assert.That(ad.creativeId == creativeId, "Creative CreativeId is not correct.");
			Assert.That(ad.adTypeId == adTypeId, "Creative AdTypeId is not correct.");
			Assert.That(ad.adTypeName == adTypeName, "Creative AdTypeName is not correct.");
			Assert.That(ad.campaignId == campaignId, "Creative CampaignId is not correct.");
			Assert.That(ad.adTag == adTag, "Creative AdTag is not correct.");
			Assert.That(ad.platformId == platformId, "Creative PlatformId is not correct.");
			Assert.That(ad.placementId == placementId, "Creative PlacementId is not correct.");
			Assert.That(ad.resourceId == resourceId, "Creative ResourceId is not correct.");
			Assert.That(ad.resourceName == resourceName, "Creative ResourceName is not correct.");
			Assert.That(ad.resourceFilename == resourceFilename, "Creative ResourceFilename is not correct.");
			Assert.That(ad.resourceType == resourceType, "Creative ResourceType is not correct.");
			Assert.That(ad.deliveryGroupId == deliveryGroupId, "Creative DeliverygroupId is not correct.");
			Assert.That(ad.isPromo == isPromo, "Creative IsPromo is not correct.");
			Assert.That(ad.adTagUrl == adTagUrl, "Creative AdTagUrl is not correct.");

			// *Note: not testing Begin/End Dates because those are computed fields by the database. I'm also not testing LastModifiedDate because that relies on Begin/End Dates.
		}

		private List<Resource> GetResources()
		{
			var resources = new List<Resource> { Resource1, Resource2, Resource3 };

			return resources;
		}

		private List<CampaignContentSchema> GetCampaignContentSchemas()
		{
			var schemas = new List<CampaignContentSchema>();
			schemas.Add(new CampaignContentSchema { Id = 1, Campaign = new Campaign { Id = 1 } });

			return schemas;
		}

		private List<Creative> GetCreatives()
		{
			var creatives = new List<Creative>();
			creatives.Add(new Creative
			{
				Id = 1,
				Name = "Creative 1",
				Campaign = new Campaign
				{
					Id = 1
				},
				AdType = new AdType
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination],
					Name = AdTypeConstants.AdTypeNames.BrandDestination,
					IsPromo = false
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
					Name = AdFunctionConstants.AdFunctionNames.ClickToJump
				}
			});
			creatives.Add(new Creative
			{
				Id = 2,
				Name = "Creative 2",
				Campaign = new Campaign
				{
					Id = 1
				},
				AdType = new AdType
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination],
					Name = AdTypeConstants.AdTypeNames.BrandDestination,
					IsPromo = false
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
					Name = AdFunctionConstants.AdFunctionNames.OnDemand
				}
			});
			creatives.Add(new Creative
			{
				Id = 3,
				Name = "Creative 3",
				Campaign = new Campaign
				{
					Id = 2
				},
				AdType = new AdType
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot],
					Name = AdTypeConstants.AdTypeNames.CommercialSpot,
					IsPromo = true
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
					Name = AdFunctionConstants.AdFunctionNames.ClickToJump
				}
			});

			return creatives;
		}

		private List<Campaign> GetCampaigns()
		{

			var campaigns = new List<Campaign>();
			campaigns.Add(new Campaign { Id = 1, Name = "Campaign 1", Internal = false, MediaAgency = new Agency { Id = 1 }, Thumbnail = Resource1, UsersFavorite = new List<User>(), Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo] } });
			campaigns.Add(new Campaign { Id = 2, Name = "Campaign 2", Internal = true, MediaAgency = new Agency { Id = 2 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AxeMasterbrand] } });
			campaigns.Add(new Campaign { Id = 3, Name = "Campaign 3", Internal = true, MediaAgency = new Agency { Id = 3 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.Beano] } });

			return campaigns;
		}

		private List<AdType> GetAdTypes()
		{
			var adTypes = new List<AdType>();
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], Name = AdTypeConstants.AdTypeNames.CommercialSpot, IsPromo = true });
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.DedicatedBrandApp], Name = AdTypeConstants.AdTypeNames.DedicatedBrandApp });
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], Name = AdTypeConstants.AdTypeNames.BrandDestination });

			return adTypes;
		}

		private List<AdFunction> GetAdFunctions()
		{
			var adFunctions = new List<AdFunction>();
			adFunctions.Add(new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand], Name = AdFunctionConstants.AdFunctionNames.OnDemand });
			adFunctions.Add(new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump], Name = AdFunctionConstants.AdFunctionNames.ClickToJump });

			return adFunctions;
		}

		private List<Feature> GetFeatures()
		{
			var features = new List<Feature>();
			features.Add(new Feature
			{
				Id = 1,
				Creative = new Creative { Id = 1 },
				Campaign = new Campaign { Id = 1 }
			});
			features.Add(new Feature
			{
				Id = 2,
				Creative = new Creative { Id = 2 },
				Campaign = new Campaign { Id = 1 }
			});
			features.Add(new Feature
			{
				Id = 3,
				Creative = new Creative { Id = 3 },
				Campaign = new Campaign { Id = 2 }
			});

			return features;
		}

		private List<Placement> GetPlacements()
		{
			var placements = new List<Placement>();
			placements.Add(new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner], MediaPartner = new MediaPartner { Id = 1 } });
			placements.Add(new Placement { Id = 2, MediaPartner = new MediaPartner { Id = 2 } });
			placements.Add(new Placement { Id = 3, MediaPartner = new MediaPartner { Id = 3 } });
			placements.Add(new Placement { Id = 4, MediaPartner = new MediaPartner { Id = 4 } });
			placements.Add(new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile], MediaPartner = new MediaPartner { Id = 4 } });
			return placements;
		}

		private List<Ad> GetAds()
		{
			var ads = new List<Ad>();
			ads.Add(new Ad 
			{ 
				Id = 1, 
				Name = "Ad 1",
				Platform = new Platform { Id = 2 },
				Placement = new Placement { Id = 1 },
				AdType = new AdType{Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]}, 
				Campaign = new Campaign { Id = 1, Generation = 2 }, 
				Creative = new Creative{Id = 1}, 
				AdTag = new AdTag{Id = 1},
				DeliveryGroup = new DeliveryGroup { Id = 1 }
			});
			ads.Add(new Ad 
			{ 
				Id = 2,
				Name = "Ad 2",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung] },
				Placement = new Placement{Id = 2},
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] },
				Campaign = new Campaign { Id = 1, Generation = 2 }, 
				Creative = new Creative { Id = 1 },
				AdTag = new AdTag { Id = 2 },
				DeliveryGroup = new DeliveryGroup { Id = 2 }
			});
			ads.Add(new Ad
			{
				Id = 3,
				Name = "Ad 3",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] },
				Placement = new Placement { Id = 1 },
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] },
				Campaign = new Campaign { Id = 1, Generation = 2 },
				Creative = new Creative { Id = 1 },
				AdTag = new AdTag { Id = 1 },
				DeliveryGroup = new DeliveryGroup { Id = 1 }
			});
			ads.Add(new Ad 
			{ 
				Id = 4,
				Name = "Ad 4",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung] },
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] },
				Campaign = new Campaign { Id = 2, Generation = 2 }, 
				Creative = new Creative { Id = 2 }, 
				Placement = new Placement { Id = 3 }, 
				AdTag = new AdTag { Id = 3 } 
			});
			ads.Add(new Ad 
			{ 
				Id = 5,
				Name = "Ad 5",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung] },
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay] },
				Campaign = new Campaign { Id = 2, Generation = 2 }, 
				Creative = new Creative { Id = 2 }, 
				Placement = new Placement { Id = 4 }, 
				AdTag = new AdTag { Id = 4 } 
			});
			ads.Add(new Ad
			{
				Id = 6,
				Name = "Ad 6",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung] },
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] },
				Campaign = new Campaign { Id = 1, Generation = 2 },
				Creative = new Creative { Id = 2 },
				Placement = new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile] },
				AdTag = new AdTag { Id = 4 }
			});
			ads.Add(new Ad
			{
				Id = 7,
				Name = "Ad 7",
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung] },
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] },
				Campaign = new Campaign { Id = 1, Generation = 2 },
				Creative = new Creative { Id = 2 },
				Placement = new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.CnbcHorizontalBanner] },
				AdTag = new AdTag { Id = 4 }
			});
			return ads;
		}

		public List<AdTag> GetAdTags()
		{
			var adTags = new List<AdTag>();
			adTags.Add(new AdTag{Id = 1});
			adTags.Add(new AdTag { Id = 2 });
			adTags.Add(new AdTag { Id = 3 });
			adTags.Add(new AdTag { Id = 4 });
			return adTags;
		}

		#endregion

	}
}
