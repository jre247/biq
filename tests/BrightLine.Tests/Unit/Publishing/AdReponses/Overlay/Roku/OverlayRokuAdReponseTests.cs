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
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.Roku;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using Moq;
using System.Text.RegularExpressions;
using BrightLine.Publishing.Areas.AdResponses.Services.Overlay.Platforms;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class OverlayRokuAdReponseTests
	{
		#region Variables

		IocRegistration Container;
		int AdTagId = 12;
		int AdId = 1;
		Ad Ad;
		int XCoordinateHd = 111;
		int YCoordinateHd = 222;
		int XCoordinateSd = 333;
		int YCoordinateSd = 444;
		int ResourceHeight = 22;
		int ResourceWidth = 11;
		string ResourceFilename = "file1.mp4";
		string TargetEnv = PublishConstants.TargetEnvironments.Production;
		string PlatformManifestName = PlatformConstants.ManifestNames.Roku;
		string PlatformName = PlatformConstants.PlatformNames.Roku;
		int PlatformId;
		string AdTypeManifestName = AdTypeConstants.ManifestNames.Overlay;
		string AdTypeName = AdTypeConstants.AdTypeNames.Overlay;
		int AdTypeId;
		int CompanionAdId = 23;
		int CompanionAdTagId = 24;
		int CampaignId = 1;
		int CreativeId = 1;
		int DestinationAdId = 1;
		int DestinationAdTagId = 13;

		#endregion //Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			PlatformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			AdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];

			SetupObjectGraph();
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-462) Overlay Roku Ad Responses Count is correct")]
		public void Overlay_Roku_AdResponses_Count_Is_Correct()
		{
			// Act
			var RokuOverlayAdResponse = new RokuOverlayAdResponse(Ad, TargetEnv);
			var adResponses = RokuOverlayAdResponse.GetAdResponse();

			// Assert
			Assert.IsNotNull(adResponses.AdResponseBody.DI, "Ad Response DI should not be null.");
			Assert.IsNotNull(adResponses.AdResponseBody.RAF, "Ad Response RAF should not be null.");
			Assert.IsNull(adResponses.AdResponseBody.Default, "Ad Response Default should be null.");
		}

		[Test(Description = "(BL-462) Overlay Roku Ad Response key is correct")]
		public void Overlay_RAF_AdResponse_Key_Is_Correct()
		{
			// Act
			var RokuOverlayAdResponse = new RokuOverlayAdResponse(Ad, TargetEnv);
			var adResponses = RokuOverlayAdResponse.GetAdResponse();

			// Assert
			var key = adResponses.Key;
			Assert.AreEqual(key, string.Format("{0}_{1}", PublishConstants.TargetEnvironments.Production, AdTagId), "Ad Response Key is not correct.");
		}

		[Test(Description = "(BL-462) Overlay Roku Ad Response metadata is correct")]
		public void Overlay_Roku_AdResponse_Metadata_Is_Correct()
		{
			// Act
			var RokuOverlayAdResponse = new RokuOverlayAdResponse(Ad, TargetEnv);
			var adResponses = RokuOverlayAdResponse.GetAdResponse();

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
			Assert.AreEqual(adResponses.Metadata.ad.destinationAd.id, DestinationAdId, "Ad Response Metadata Ad DestinationAdId is not correct.");
		}

		#region RAF Tests

		[Test(Description = "(BL-462) Overlay Roku RAF Ad Response Poco is correct")]
		public void Overlay_Roku_RAF_AdResponseData_Poco_Is_Correct()
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Act
			var RokuOverlayAdResponse = new RokuOverlayAdResponse(Ad, TargetEnv);
			var adResponses = RokuOverlayAdResponse.GetAdResponse();

			// Assert
			var adResponsePoco = (RokuOverlayViewModel)adResponses.AdResponseBody.RAF;
			AssertLaunchMicrositeForRAF(settings, AdTagId, AdId, adResponsePoco);
			AssertInitialState(adResponsePoco);
			AssertMedia(AdId, adResponsePoco);
			AssertActionsEnter(adResponsePoco);
			AssertHdLayers(settings, XCoordinateHd, YCoordinateHd, ResourceHeight, ResourceWidth, ResourceFilename, Ad, adResponsePoco);
			AssertSdLayers(settings, XCoordinateSd, YCoordinateSd, ResourceHeight, ResourceWidth, ResourceFilename, Ad, adResponsePoco);
			AssertTimeline(adResponsePoco);
		}

		#endregion //RAF Tests

		#region DI Tests

		[Test(Description = "(BL-462) Overlay Roku DI Ad Response Poco is correct")]
		public void Overlay_Roku_DI_AdResponseData_Poco_Is_Correct()
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Act
			var RokuOverlayAdResponse = new RokuOverlayAdResponse(Ad, TargetEnv);
			var adResponses = RokuOverlayAdResponse.GetAdResponse();

			// Assert
			var adResponsePoco = (RokuOverlayViewModel)adResponses.AdResponseBody.DI;
			AssertLaunchMicrositeForDI(settings, AdTagId, AdId, adResponsePoco);
			AssertInitialState(adResponsePoco);
			AssertMedia(AdId, adResponsePoco);
			AssertActionsEnter(adResponsePoco);
			AssertHdLayers(settings, XCoordinateHd, YCoordinateHd, ResourceHeight, ResourceWidth, ResourceFilename, Ad, adResponsePoco);
			AssertSdLayers(settings, XCoordinateSd, YCoordinateSd, ResourceHeight, ResourceWidth, ResourceFilename, Ad, adResponsePoco);
			AssertTimeline(adResponsePoco);

		}

		#endregion //DI Tests

		#endregion // Tests

		#region Assertions

		private void AssertInitialState(RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.initialState, PublishRokuOverlayConstants.InitialState, "InitialState is not correct.");
		}

		private void AssertMedia(int AdId, RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.meta.ad_id, AdId, "Meta ad_id is not correct.");
		}

		private void AssertActionsEnter(RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.states.state0.actions.enter, PublishRokuOverlayConstants.StateActionsEnter, "States actions enter is not correct.");
		}

		private void AssertTimeline(RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.timeline[0].state, PublishRokuOverlayConstants.TimelineState, "Timeline state is not correct.");
			Assert.AreEqual(adResponse.timeline[0].ts, 1, "Timeline ts is not correct.");
		}

		//TODO: uncomment when microsite is working
		private void AssertLaunchMicrositeForDI(ISettingsService settings, int AdTagId, int AdId, RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.actions.launchMicrosite.type, PublishRokuOverlayConstants.LaunchMicrositeType, "Actions LaunchMicrosite Type is not correct."); // This should be null for Roku DI
			Assert.AreEqual(adResponse.actions.launchMicrosite.target.id, AdId, "Actions LaunchMicrosite Target Id is not correct.");
			Assert.AreEqual(adResponse.actions.launchMicrosite.target.includeCore, false, "Actions LaunchMicrosite Target includeCore is not correct.");

			//TODO: don't split on &ts but use a utilitiy class and then use Moq to return an expected DateTime.Now as milliseconds value
			var url = Regex.Split(adResponse.actions.launchMicrosite.target.url, "&ts")[0];
			//Assert.AreEqual(url, string.Format("{0}?id={1}&{2}", settings.AdServerUrl, DestinationAdTagId, AdTagUrlConstants.RokuAdIdMacro), "Actions LaunchMicrosite Target url is not correct.");
		}

		//TODO: uncomment when microsite is working
		private void AssertLaunchMicrositeForRAF(ISettingsService settings, int AdTagId, int AdId, RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.actions.launchMicrosite.type, PublishRokuOverlayConstants.LaunchMicrositeType, "Actions LaunchMicrosite Type is not correct."); // This should be null for Roku DI
			Assert.IsNull(adResponse.actions.launchMicrosite.target.id, "Actions LaunchMicrosite Target Id should be null.");
			Assert.IsNull(adResponse.actions.launchMicrosite.target.includeCore, "Actions LaunchMicrosite Target includeCore should be null.");
			
			
			//TODO: don't split on &ts but use a utilitiy class and then use Moq to return an expected DateTime.Now as milliseconds value
			var url = Regex.Split(adResponse.actions.launchMicrosite.target.url, "&ts")[0];
			//Assert.AreEqual(url, string.Format("{0}?id={1}&{2}", settings.AdServerUrl, DestinationAdTagId, AdTagUrlConstants.RokuAdIdMacro), "Actions LaunchMicrosite Target url is not correct.");
		}

		private void AssertHdLayers(ISettingsService settings, int XCoordinateHd, int YCoordinateHd, int ResourceHeight, int ResourceWidth, string ResourceFilename, Ad Ad, RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].CompositionMode, PublishRokuOverlayConstants.CompositionMode, "States hdLayers content CompositionMode is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].TargetRect.h, ResourceHeight, "States hdLayers content TargetRect H is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].TargetRect.w, ResourceWidth, "States hdLayers content TargetRect W is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].TargetRect.x, XCoordinateHd, "States hdLayers content TargetRect X is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].TargetRect.y, YCoordinateHd, "States hdLayers content TargetRect Y is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].content[0].url, string.Format("http://cdn-local-m.brightline.tv/campaigns/{0}/images/{1}", Ad.Campaign.Id, ResourceFilename), "States hdLayers content url is not correct.");
			Assert.AreEqual(adResponse.states.state0.hdLayers[0].index, 10, "States hdLayers index is not correct.");
		}

		private void AssertSdLayers(ISettingsService settings, int XCoordinateSd, int YCoordinateSd, int ResourceHeight, int ResourceWidth, string ResourceFilename, Ad Ad, RokuOverlayViewModel adResponse)
		{
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].CompositionMode, PublishRokuOverlayConstants.CompositionMode, "States sdLayers content CompositionMode is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].TargetRect.h, ResourceHeight, "States sdLayers content TargetRect H is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].TargetRect.w, ResourceWidth, "States sdLayers content TargetRect W is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].TargetRect.x, XCoordinateSd, "States sdLayers content TargetRect X is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].TargetRect.y, YCoordinateSd, "States sdLayers content TargetRect Y is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].content[0].url, string.Format("http://cdn-local-m.brightline.tv/campaigns/{0}/images/{1}", Ad.Campaign.Id, ResourceFilename), "States sdLayers content url is not correct.");
			Assert.AreEqual(adResponse.states.state0.sdLayers[0].index, 10, "States sdLayers index is not correct.");
		}

		#endregion	

		#region Helper Methods

		private void SetupObjectGraph()
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var settings = IoC.Resolve<ISettingsService>();

			var campaigns = IoC.Resolve<ICampaignService>();
			var adName = "Test Ad";
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var creativeName = "Creative 1";
			var creativeDescription = "Creative Description 1";

			Ad = new Ad
			{
				Id = AdId,
				Name = adName,
				AdType = new AdType { Id = adTypeId, Name = AdTypeName, ManifestName = AdTypeManifestName },
				AdTag = new AdTag { Id = AdTagId },
				Creative = new Creative { Id = CreativeId, Name = creativeName, Description = creativeDescription },
				Platform = new Platform { Id = PlatformId, Name = PlatformName, ManifestName = PlatformManifestName },
				Campaign = new Campaign { Id = CampaignId },
				CompanionAd = new Ad { Id = CompanionAdId, AdTag = new AdTag { Id = CompanionAdTagId } },
				DestinationAd = new Ad { Id = DestinationAdId, AdTag = new AdTag { Id = DestinationAdTagId } },
				XCoordinateHd = XCoordinateHd,
				YCoordinateHd = YCoordinateHd,
				XCoordinateSd = XCoordinateSd,
				YCoordinateSd = YCoordinateSd
			};

			SetupObjectGraphForAd(CampaignId);
		}

		private void SetupObjectGraphForAd(int campaignId)
		{
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();
			var campaigns = IoC.Resolve<ICampaignService>();

			var resourceId = 1;
			var resourceDuration = 160;
			var resourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];
			var resourceTypeId1 = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage];
			var resourceTypeId2 = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage];

			// set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(campaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(Ad);
	
			var resource1 = new Resource{
				Id = resourceId, 
				ResourceType = new ResourceType{Id = resourceTypeId1}, 
				Extension = new FileType{Id = resourceExtensionId }, 
				Width = ResourceWidth, 
				Height = ResourceHeight, 
				Filename = ResourceFilename,
				Duration = resourceDuration,
				Creative = new Creative { Id = 1, Campaign = new Campaign { Id = campaignId } }
			};
			var resource2 = new Resource
			{
				Id = resourceId,
				ResourceType = new ResourceType { Id = resourceTypeId2 },
				Extension = new FileType { Id = resourceExtensionId },
				Width = ResourceWidth,
				Height = ResourceHeight,
				Filename = ResourceFilename,
				Duration = resourceDuration,
				Creative = new Creative { Id = 1, Campaign = new Campaign { Id = campaignId } }           
			};
			Ad.Creative.Resources = new List<Resource> { resource1, resource2 };
		}


		#endregion
	}
}
