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
using BrightLine.Common.ViewModels.AdTrackingUrl.AdServer;
using Moq;
using BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot.Platforms;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CommercialSpotFireTvAdReponseTests
	{
		#region Variables

		IocRegistration Container;
		int AdTagId = 12;
		int AdId = 1;
		Ad Ad;
		string TargetEnv = PublishConstants.TargetEnvironments.Production;
		string PlatformManifestName = PlatformConstants.ManifestNames.FireTV;
		string PlatformName = PlatformConstants.PlatformNames.FireTV;
		int PlatformId;
		string AdTypeManifestName = AdTypeConstants.ManifestNames.CommercialSpot;
		string AdTypeName = AdTypeConstants.AdTypeNames.CommercialSpot;
		int AdTypeId;
		int CompanionAdId = 23;
		int CompanionAdTagId = 24;
		int ResourceId = 1;
		int ResourceTypeId;
		string ResourceManifestName = ResourceTypeConstants.ManifestNames.HdImage;
		int ResourceExtensionId;
		int ResourceWidth = 22;
		int ResourceHeight = 11;
		string ResourceMd5Hash = "abc123-32309-aa";
		string ResourceFilename = "file1.mp4";
		string ResourceName = "file1";
		int CampaignId = 1;
		int CreativeId = 1;
		int ResourceDuration = 160;
		int ResourceBitrate = 1000;

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

			PlatformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.FireTV];
			AdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];

			ResourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo];
			ResourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];

			SetupObjectGraph();
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-629) Commercial Spot FireTv Ad Responses Count is correct")]
		public void CommercialSpot_Html5_AdResponses_Count_Is_Correct()
		{
			// Act
			var service = new Html5CommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			Assert.IsNotNull(adResponses.AdResponseBody.Default, "Ad Responses Default should be null.");
			Assert.IsNull(adResponses.AdResponseBody.RAF, "Ad Responses RAF should be null.");
			Assert.IsNull(adResponses.AdResponseBody.DI, "Ad Responses DI should be null.");
		}

		[Test(Description = "(BL-462) Commercial Spot FireTv Ad Response key is correct")]
		public void CommercialSpot_FireTv_AdResponse_Key_Is_Correct()
		{
			// Act
			var service = new Html5CommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			var key = adResponses.Key;
			Assert.AreEqual(key, string.Format("{0}_{1}", PublishConstants.TargetEnvironments.Production, AdTagId), "Ad Response Key is not correct.");
		}

		[Test(Description = "(BL-462) Commercial Spot FireTv Ad Response metadata is correct")]
		public void CommercialSpot_FireTv_AdResponse_Metadata_Is_Correct()
		{
			// Act
			var service = new Html5CommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

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

			AssertMetadataResources(adResponses);

			AssertTrackingUrlsMetaData(adResponses);
		}

		[Test(Description = "(BL-462) Commercial Spot Fire Tv Ad Response Poco is correct")]
		public void CommercialSpot_FireTv_AdResponseData_VAST_Xml_Is_Correct()
		{
			// Act
			var service = new Html5CommercialSpotAdResponse(Ad, TargetEnv);
			var adResponses = service.GetAdResponse();

			// Assert
			var adResponseXml = adResponses.AdResponseBody.Default.ToString();
			var adResponseXmlDecoded = HttpUtility.HtmlDecode(adResponseXml);
			var adResponsXmlCompare = LoadAdResponseResource("Publishing.AdResponses.CommercialSpot.Html5.Html5AdResponse.xml");
			Assert.That(string.Compare(adResponseXmlDecoded, adResponsXmlCompare, StringComparison.OrdinalIgnoreCase) == 0, "Ad Reponse Xml does not match reference.");
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
				AdType = new AdType { Id = AdTypeId, Name = AdTypeName, ManifestName = AdTypeManifestName },
				AdTag = new AdTag { Id = AdTagId },
				CompanionAd = new Ad { Id = CompanionAdId, AdTag = new AdTag { Id = CompanionAdTagId } },
				Creative = new Creative { Id = CreativeId, Name = creativeName, Description = creativeDescription },
				Platform = new Platform { Id = PlatformId, Name = PlatformName, ManifestName = PlatformManifestName },
				Campaign = new Campaign { Id = CampaignId },
				XCoordinateHd = xCoordinateHd,
				YCoordinateHd = yCoordinateHd,
				XCoordinateSd = xCoordinateSd,
				YCoordinateSd = yCoordinateSd
			};

			SetupObjectGraphForAd(CampaignId);
			CreateAdTrackingEventsForAd();
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

			// set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(campaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(Ad);

			var resource = new Resource
			{
				Id = resourceId,
				ResourceType = new ResourceType { Id = resourceTypeId, ManifestName = ResourceManifestName },
				Extension = new FileType { Id = resourceExtensionId },
				Width = ResourceWidth,
				Name = ResourceName,
				Height = ResourceHeight,
				Bitrate = ResourceBitrate,
				MD5Hash = ResourceMd5Hash,
				Filename = ResourceFilename,
				Duration = ResourceDuration,
				Creative = new Creative { Id = 1, Campaign = new Campaign { Id = campaignId } }
			};
			Ad.Creative.Resources = new List<Resource> { resource };
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

		private static string LoadAdResponseResource(string resourcePath)
		{
			var xml = ResourceLoader.ReadText(resourcePath);
			var xmlTrimmed = Regex.Replace(xml, @"\t|\n|\r", ""); // remove new lines, tabs, and returns from text
			return xmlTrimmed;
		}

		#endregion


	}
}
