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
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.Services.Destination.Platforms;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class DestinationSamsungAdReponseTests
	{
		#region Variables

		IocRegistration Container;
		int AdTagId = 12;
		int AdId = 1;
		Ad Ad;
		string TargetEnv = PublishConstants.TargetEnvironments.Production;
		string PlatformManifestName = PlatformConstants.ManifestNames.Samsung;
		string PlatformName = PlatformConstants.PlatformNames.Samsung;
		int PlatformId;
		string AdTypeManifestName = AdTypeConstants.ManifestNames.BrandDestination;
		string AdTypeName = AdTypeConstants.AdTypeNames.BrandDestination;
		int AdTypeId;
		int CompanionAdId = 23;
		int CompanionAdTagId = 24;
		int ResourceId = 30;
		int ResourceTypeId;
		string ResourceManifestName = ResourceTypeConstants.ManifestNames.HdImage;
		int ResourceExtensionId;
		int ResourceWidth = 40;
		int ResourceHeight = 50;
		string ResourceMd5Hash = "abc123-32309-aa";
		string ResourceFilename = "abc.png";
		string ResourceName = "abc";
		int CampaignId = 1;
		int CreativeId = 1;

		#endregion //Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();
			Container.Register<IEnvironmentHelper, EnvironmentHelper>();

			PlatformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			AdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			ResourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage];
			ResourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Png];

			SetupObjectGraph();
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-629) Destination Samsung Ad Responses Count is correct")]
		public void BrandDestination_Html5_AdResponses_Count_Is_Correct()
		{
			// Act
			var service = new Html5DestinationAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			Assert.IsNull(adResponses.AdResponseBody.Default, "Ad Responses Default not be null.");
			Assert.IsNull(adResponses.AdResponseBody.RAF, "Ad Responses RAF should be null.");
			Assert.IsNull(adResponses.AdResponseBody.DI, "Ad Responses DI should be null.");
		}

		[Test(Description = "(BL-462) Destination Samsung Ad Response key is correct")]
		public void BrandDestination_Samsung_AdResponse_Key_Is_Correct()
		{
			// Act
			var service = new Html5DestinationAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			var key = adResponses.Key;
			Assert.AreEqual(key, string.Format("{0}_{1}", PublishConstants.TargetEnvironments.Production, AdTagId), "Ad Response Key is not correct.");
		}

		[Test(Description = "(BL-462) Destination Samsung Ad Response metadata is correct")]
		public void BrandDestination_Samsung_AdResponse_Metadata_Is_Correct()
		{
			// Act
			var service = new Html5DestinationAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			Assert.AreEqual(adResponses.Metadata.ad.id, AdId, "Ad Response Metadata Ad Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adType.manifestName, AdTypeManifestName, "Ad Response Metadata AdType ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.platform.manifestName, PlatformManifestName, "Ad Response Metadata Platform ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adTagId, AdTagId, "Ad Response Metadata Ad Tag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.responseType, PublishConstants.ResponseTypes.Json, "Ad Response Metadata responseType is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.companionAd.id, CompanionAdId, "Ad Response Metadata CompanionAd Id is not correct.");
			Assert.IsNotNull(adResponses.Metadata.ad.companionAd.adTagId, "Ad Response Metadata CompanionAd AdTag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.campaign.id, CampaignId, "Ad Response Metadata Ad CampaignId is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.creative.id, CreativeId, "Ad Response Metadata Ad CreativeId is not correct.");

			AssertMetadataResources(adResponses);
		}

		#endregion 

		#region Asserts

		private void AssertMetadataResources(AdResponseViewModel adResponse)
		{
			var resources = adResponse.Metadata.ad.creative.resources;
			var resource = resources.ElementAt(0);
			Assert.AreEqual(resources.Count(), 1, "Ad Response Metadata Resources count is not correct.");
			Assert.AreEqual(resource.filename, ResourceFilename, "Ad Response Metadata Resource Filename is not correct.");
			Assert.AreEqual(resource.height, ResourceHeight, "Ad Response Metadata Resource Height is not correct.");
			Assert.AreEqual(resource.id, ResourceId, "Ad Response Metadata Resource Id is not correct.");
			Assert.AreEqual(resource.md5Hash, ResourceMd5Hash, "Ad Response Metadata Resource Md5Hash is not correct.");
			Assert.AreEqual(resource.name, ResourceName, "Ad Response Metadata Resource Name is not correct.");
			Assert.AreEqual(resource.resourceType.manifestName, ResourceManifestName, "Ad Response Metadata Resource ManifestName is not correct.");
			Assert.AreEqual(resource.width, ResourceWidth, "Ad Response Metadata Resource Width is not correct.");
		}

		#endregion

		#region Helper Methods

		private void SetupObjectGraph()
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var settings = IoC.Resolve<ISettingsService>();

			var adName = "Test Ad";
			var creativeName = "Creative 1";
			var creativeDescription = "Creative Description 1";

			var xCoordinateHd = 111;
			var yCoordinateHd = 222;
			var xCoordinateSd = 333;
			var yCoordinateSd = 444;

			Ad = new Ad
			{
				Id = AdId,
				Name = adName,
				AdType = new AdType { Id = AdTypeId, Name = AdTypeName, ManifestName = AdTypeManifestName},
				AdTag = new AdTag { Id = AdTagId },
				CompanionAd = new Ad{Id = CompanionAdId, AdTag = new AdTag{Id = CompanionAdTagId}},
				Creative = new Creative { Id = CreativeId, Name = creativeName, Description = creativeDescription },
				Platform = new Platform { Id = PlatformId, Name = PlatformName, ManifestName = PlatformManifestName },
				Campaign = new Campaign { Id = CampaignId },
				XCoordinateHd = xCoordinateHd,
				YCoordinateHd = yCoordinateHd,
				XCoordinateSd = xCoordinateSd,
				YCoordinateSd = yCoordinateSd
			};

			SetupObjectGraphForAd(CampaignId);
		}

		private void SetupObjectGraphForAd(int campaignId)
		{
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();
			var campaigns = IoC.Resolve<ICampaignService>();

			var resourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];
			var resourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage];

			// set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(campaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(Ad);
	
			var resource = new Resource{
				Id = ResourceId, 
				Name = ResourceName,
				ResourceType = new ResourceType{Id = ResourceTypeId, ManifestName = ResourceManifestName}, 
				Extension = new FileType{Id = ResourceExtensionId }, 
				Width = ResourceWidth, 
				Height = ResourceHeight, 
				MD5Hash = ResourceMd5Hash,
				Filename = ResourceFilename,
			};
			Ad.Creative.Resources = new List<Resource>{resource};
		}

		#endregion

	}
}
