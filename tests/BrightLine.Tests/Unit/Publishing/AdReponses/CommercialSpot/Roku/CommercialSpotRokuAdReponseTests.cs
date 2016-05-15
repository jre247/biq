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
using BrightLine.Common.ViewModels.AdTrackingUrl;
using BrightLine.Common.ViewModels.AdTrackingUrl.AdServer;
using Moq;
using BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot.Platforms;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CommercialSpotRokuAdReponseTests
	{
		#region Variables

		IocRegistration Container;
		int AdTagId = 12;
		int AdId = 1;
		Ad Ad;
		string TargetEnv = PublishConstants.TargetEnvironments.Production;
		string PlatformManifestName = PlatformConstants.ManifestNames.Roku;
		string PlatformName = PlatformConstants.PlatformNames.Roku;
		int PlatformId;
		string AdTypeManifestName = AdTypeConstants.ManifestNames.CommercialSpot;
		string AdTypeName = AdTypeConstants.AdTypeNames.CommercialSpot;
		int AdTypeId;
		int CompanionAdId = 23;
		int CompanionAdTagId = 24;
		int CampaignId = 1;
		int CreativeId = 1;

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
			AdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];

			SetupObjectGraph();
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-462) Commercial Spot Roku Ad Responses Count is correct")]
		public void CommercialSpot_Roku_AdResponses_Count_Is_Correct()
		{
			// Act
			var RokuCommercialSpotAdResponse = new RokuCommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = RokuCommercialSpotAdResponse.GetAdResponse();

			// Assert
			Assert.IsNotNull(adResponses.AdResponseBody.DI, "Ad Response DI should not be null.");
			Assert.IsNotNull(adResponses.AdResponseBody.RAF, "Ad Response RAF should not be null.");
			Assert.IsNull(adResponses.AdResponseBody.Default, "Ad Response Default should be null.");
		}

		[Test(Description = "(BL-462) Commercial Spot Roku Ad Response key is correct")]
		public void CommercialSpot_Roku_AdResponse_Key_Is_Correct()
		{
			// Act
			var RokuCommercialSpotAdResponse = new RokuCommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = RokuCommercialSpotAdResponse.GetAdResponse();

			// Assert
			var key = adResponses.Key;
			Assert.AreEqual(key, string.Format("{0}_{1}", PublishConstants.TargetEnvironments.Production, AdTagId), "Ad Response Key is not correct.");
		}

		[Test(Description = "(BL-462) Commercial Spot Roku Ad Response metadata is correct")]
		public void CommercialSpot_Roku__AdResponse_Metadata_Is_Correct()
		{
			// Act
			var RokuCommercialSpotAdResponse = new RokuCommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = RokuCommercialSpotAdResponse.GetAdResponse();

			// Assert
			Assert.AreEqual(adResponses.Metadata.ad.id, AdId, "Ad Response Metadata Ad Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adType.manifestName, AdTypeManifestName, "Ad Response Metadata AdType ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.platform.manifestName, PlatformManifestName, "Ad Response Metadata Platform ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adTagId, AdTagId, "Ad Response Metadata Ad Tag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.responseType, PublishConstants.ResponseTypes.Xml, "Ad Response Metadata responseType is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.companionAd.id, CompanionAdId, "Ad Response Metadata CompanionAd Id is not correct.");
			Assert.IsNotNull(adResponses.Metadata.ad.companionAd.adTagId, "Ad Response Metadata CompanionAd AdTag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.campaign.id, CampaignId, "Ad Response Metadata Ad CampaignId is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.creative.id, CreativeId, "Ad Response Metadata Ad CreativeId is not correct.");

			AssertTrackingUrlsMetaData(adResponses);
		}

		#region RAF Tests

		[Test(Description = "(BL-462) Commercial Spot Roku RAF Ad Response Poco is correct")]
		public void CommercialSpot_Roku_RAF_AdResponseData_VAST_Xml_Is_Correct()
		{
			// Act
			var RokuCommercialSpotAdResponse = new RokuCommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = RokuCommercialSpotAdResponse.GetAdResponse();

			// Assert
			var adResponseXml = adResponses.AdResponseBody.RAF.ToString();
			var adResponseXmlDecoded = HttpUtility.HtmlDecode(adResponseXml);
			var adResponsXmlCompare = GetAdResponseRAFXmlCompare();
			Assert.That(string.Compare(adResponseXmlDecoded, adResponsXmlCompare, StringComparison.OrdinalIgnoreCase) == 0, "Ad Reponse Xml does not match reference.");
		}
		
		#endregion //RAF Tests

		#region DI Tests

		[Test(Description = "(BL-462) Commercial Spot Roku DI Ad Response Poco is correct")]
		public void CommercialSpot_Roku_DI_AdResponseData_VAST_Xml_Is_Correct()
		{
			// Act
			var RokuCommercialSpotAdResponse = new RokuCommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = RokuCommercialSpotAdResponse.GetAdResponse();

			// Assert
			var adResponseXml = adResponses.AdResponseBody.DI.ToString();
			var adResponseXmlDecoded = HttpUtility.HtmlDecode(adResponseXml);
			var adResponsXmlCompare = GetAdResponseDIXmlCompare();
			Assert.That(string.Compare(adResponseXmlDecoded, adResponsXmlCompare, StringComparison.OrdinalIgnoreCase) == 0, "Ad Reponse Xml does not match reference.");
		}

		#endregion // DI Tests

		#endregion // Tests

		#region Asserts

		private void AssertTrackingUrlsMetaData(AdResponseViewModel adResponse)
		{
			AssertStartTrackingUrlMetadata(adResponse, 0);

			AssertQuartileTrackingUrlMetaData(adResponse, 1, AdTagUrlConstants.AdServer.PercentComplete.FirstQuartile);
			AssertQuartileTrackingUrlMetaData(adResponse, 2, AdTagUrlConstants.AdServer.PercentComplete.Midpoint);
			AssertQuartileTrackingUrlMetaData(adResponse, 3, AdTagUrlConstants.AdServer.PercentComplete.ThirdQuartile);
			AssertQuartileTrackingUrlMetaData(adResponse, 4, AdTagUrlConstants.AdServer.PercentComplete.Complete);
		}

		private void AssertStartTrackingUrlMetadata(AdResponseViewModel adResponse, int trackingUrlIndex)
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Deserialize Tracking Url to POCO
			var trackingUrl = adResponse.Metadata.trackingUrls.ElementAt(trackingUrlIndex);
			var trackingUrlDecoded = HttpUtility.UrlDecode(trackingUrl.Value);
			var trackingUrlSplit = trackingUrlDecoded.Split(new string[] { "?data=" }, StringSplitOptions.None);
			var trackingUrlBase = trackingUrlSplit[0];
			var trackingUrlData = trackingUrlSplit[1];
			var trackingUrlDataDeserialized = JsonConvert.DeserializeObject<StartEventAdTrackingUrlViewModel>(trackingUrlData);

			// Assert Tracking Url Data properties (after being deserialized)
			Assert.AreEqual(trackingUrlDataDeserialized.ad_id, AdId, "Ad Response Metadata Start Event TrackingUrl ad_id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.id, AdTagUrlConstants.AdServer.AdServerIdMacro, "Ad Response Metadata Start Event TrackingUrl ad_id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.type, AdTagUrlConstants.AdServer.Types.Impression, "Ad Response Metadata Start Event TrackingUrl type is not correct.");

			// Assert Base Tracking Url
			Assert.AreEqual(trackingUrlBase, settings.TrackingUrl, "Ad Response Metadata Start Event Base TrackingUrl is not correct.");
		}

		private void AssertQuartileTrackingUrlMetaData(AdResponseViewModel adResponse, int trackingUrlIndex, int percentComplete)
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Deserialize Tracking Url to POCO
			var trackingUrl = adResponse.Metadata.trackingUrls.ElementAt(trackingUrlIndex);
			var trackingUrlDecoded = HttpUtility.UrlDecode(trackingUrl.Value);
			var trackingUrlSplit = trackingUrlDecoded.Split(new string[] { "?data=" }, StringSplitOptions.None);
			var trackingUrlBase = trackingUrlSplit[0];
			var trackingUrlData = trackingUrlSplit[1];
			var trackingUrlDataDeserialized = JsonConvert.DeserializeObject<QuartileEventAdTrackingUrlViewModel>(trackingUrlData);

			// Assert Tracking Url Data properties (after being deserialized)
			Assert.AreEqual(trackingUrlDataDeserialized.duration_type, AdTagUrlConstants.AdServer.DurationType, "Ad Response Metadata Quartile Event DurationType ad_id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.id, AdTagUrlConstants.AdServer.AdServerIdMacro, "Ad Response Metadata Quartile Event TrackingUrl id is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.type, AdTagUrlConstants.AdServer.Types.Duration, "Ad Response Metadata Quartile Event TrackingUrl type is not correct.");
			Assert.AreEqual(trackingUrlDataDeserialized.percent_complete, percentComplete, "Ad Response Metadata Quartile Event PercentComplete ad_id is not correct.");

			// Assert Base Tracking Url
			Assert.AreEqual(trackingUrlBase, settings.TrackingUrl, "Ad Response Metadata Start Event Base TrackingUrl is not correct.");
		}


		#endregion // Asserts

		#region Helper Methods

		private void SetupObjectGraph()
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var settings = IoC.Resolve<ISettingsService>();

			var adName = "Test Ad";
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
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
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], Name = AdTypeName, ManifestName = AdTypeConstants .ManifestNames.CommercialSpot},
				AdTag = new AdTag { Id = AdTagId },
				CompanionAd = new Ad{Id = CompanionAdId, AdTag = new AdTag{Id = CompanionAdTagId}},
				Creative = new Creative { Id = CreativeId, Name = creativeName, Description = creativeDescription },
				Platform = new Platform { Id = PlatformId, Name = PlatformName, ManifestName = PlatformConstants.ManifestNames.Roku },
				Placement = new Placement { Id = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile] },
				Campaign = new Campaign { Id = CampaignId },
				XCoordinateHd = xCoordinateHd,
				YCoordinateHd = yCoordinateHd,
				XCoordinateSd = xCoordinateSd,
				YCoordinateSd = yCoordinateSd
			};

			SetupObjectGraphForAd(CampaignId);
			CreateAdTrackingEventsForAd();
		}


		private string GetAdResponseDIXmlCompare()
		{
			return LoadAdResponseResource("Publishing.AdResponses.CommercialSpot.Roku.RokuDIAdResponse.xml");
		}

		private string GetAdResponseRAFXmlCompare()
		{
			return LoadAdResponseResource("Publishing.AdResponses.CommercialSpot.Roku.RokuRAFAdResponse.xml");
		}

		private static string LoadAdResponseResource(string resourcePath)
		{
			var xml = ResourceLoader.ReadText(resourcePath);
			var xmlTrimmed = Regex.Replace(xml, @"\t|\n|\r", ""); // remove new lines, tabs, and returns from text
			return xmlTrimmed;
		}

		private void SetupObjectGraphForAd(int campaignId)
		{
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();
			var campaigns = IoC.Resolve<ICampaignService>();

			var resourceId = 1;
			var resourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];
			var resourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo];
			var resourceWidth = 11;
			var resourceHeight = 22;
			var resourceBitrate = 1000;
			var resourceFilename = "file1.mp4";
			var resourceDuration = 160;

			// set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(campaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(Ad);
	
			var resource = new Resource{
				Id = resourceId, 
				ResourceType = new ResourceType{Id = resourceTypeId}, 
				Extension = new FileType{Id = resourceExtensionId }, 
				Width = resourceWidth, 
				Height = resourceHeight, 
				Bitrate = resourceBitrate,
				Filename = resourceFilename,
				Duration = resourceDuration,
				Creative = new Creative { Id = 1, Campaign = new Campaign { Id = campaignId } }           
			};
			Ad.Creative.Resources = new List<Resource>{resource};
		}

		private void CreateAdTrackingEventsForAd()
		{
			var AdId = Ad.Id;
			var adTrackingEvents = new List<AdTrackingEvent>();
			var trackingEventName1 = TrackingEventConstants.TrackingEventNames.CreativeView;
			var trackingEventName2 = TrackingEventConstants.TrackingEventNames.FirstQuartile;
			var trackingUrl1 = "www.tracking1.com/track";
			var trackingUrl2 = "www.tracking2.com/track";

			adTrackingEvents.Add(new AdTrackingEvent
			{
				Id = 1,
				Ad = Ad,
				TrackingEvent = new TrackingEvent
				{
					Id = Lookups.TrackingEvents.HashByName[trackingEventName1],
					Name = trackingEventName1
				},
				TrackingUrl = trackingUrl1
			});
			adTrackingEvents.Add(new AdTrackingEvent
			{
				Id = 2,
				Ad = Ad,
				TrackingEvent = new TrackingEvent
				{
					Id = Lookups.TrackingEvents.HashByName[trackingEventName2],
					Name = trackingEventName2
				},
				TrackingUrl = trackingUrl2
			});		
		
			Ad.AdTrackingEvents = adTrackingEvents;
			
		}

		#endregion

	}
}
