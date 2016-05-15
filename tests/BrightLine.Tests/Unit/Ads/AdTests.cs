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

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class AdTests
	{
		IocRegistration Container;
		private IAdService Ads { get;set;}
		private ICreativeService Creatives { get;set;}

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();
			Container.Register<IEnvironmentHelper, EnvironmentHelper>();

			Ads = IoC.Resolve<IAdService>();
			Creatives = IoC.Resolve<ICreativeService>();
		}

		[Test(Description = "Saving an ad with a companion ad id, with incorrect ad type, will throw validation exception.")]
		[ExpectedException(typeof(ValidationException))] 
		public void Ad_With_CompanionAdId_Does_Not_Have_Correct_AdType()
		{
			var ad = Ads.Get(1);
			ad.CompanionAd = new Ad{Id = 2};
			ad.Creative.AdFunction = new AdFunction{Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand]};
			ad.Campaign.Ads = new List<Ad>{
				MockEntities.CreateAd(122, new DateTime(2012, 2, 1), new DateTime(2013, 3, 1)) 
			};

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving an ad with a companion ad id that does not key to an existing ad will throw validation exception.")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_With_CompanionAdId_Keys_To_Nonexistent_Ad()
		{
			var ad = Ads.Get(1);
			ad.CompanionAd = new Ad { Id = 1000 };
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
			SetupAdWithCommercialSpotAdType(ad);

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving an ad with a companion ad id, and a correct ad type")]
		public void Ad_With_CompanionAdId_Has_Correct_AdType()
		{
			var ad = Ads.Get(1);
			ad.CompanionAd_Id = 2;
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
			SetupAdWithCommercialSpotAdType(ad);

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving an ad with an empty Delivery Group throws error")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_With_ImageBanner_AdType_And_Empty_Delivery_Group_Throws_Error()
		{
			var ad = Ads.Get(1);
			ad.DeliveryGroup = null;
			ad.AdType = GetImageBannerAdType(ad.Placement.AdTypeGroup);

			ad.Campaign.Creatives.Add(GetCreativeForImageBannerAdType(ad));
			ad.Creative = GetCreativeForImageBannerAdType(ad);
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
			ad.Features = new List<Feature>();

			ad = Ads.Update(ad);
		}
	

		[Test(Description = "Saving an ad with an empty Delivery Group throws error")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_With_Overlay_AdType_And_Empty_Delivery_Group_Throws_Error()
		{
			var ad = Ads.Get(1);
			ad.DeliveryGroup = null; 
			ad.AdType = GetOverlayAdType(ad.Placement.AdTypeGroup);

			ad.Campaign.Creatives.Add(GetCreativeForOverlayAdType(ad));
			ad.Creative = GetCreativeForOverlayAdType(ad);
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
			ad.Features = new List<Feature>();

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving an ad with an empty Delivery Group does not throw an error")]
		public void Ad_With_VideoBanner_AdType_And_Empty_Delivery_Group_Does_Not_Throw_Error()
		{
			var ad = Ads.Get(1);
			ad.DeliveryGroup = null;

			//assign ad type group for ad's placement to be contained inside ad's ad type groups
			ad.AdType = GetVideoBannerAdType(ad.Placement.AdTypeGroup);

			ad.Campaign.Creatives.Add(GetCreativeForVideoBannerAdType(ad));
			ad.Creative = GetCreativeForVideoBannerAdType(ad);
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
			ad.Features = new List<Feature>();

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving a promo ad that is click to jump with an empty Destination Creative throws error")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_ClickToJump_With_Empty_Destination_Creative_Throw_Error()
		{
			var ad = Ads.Get(1);

			//assign ad type group for ad's placement to be contained inside ad's ad type groups
			ad.AdType = GetOverlayAdType(ad.Placement.AdTypeGroup);

			ad.Campaign.Creatives.Add(GetCreativeForVideoBannerAdType(ad));
			ad.Creative = GetCreativeForVideoBannerAdType(ad);
			ad.Creative.AdFunction = new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump] };
			ad.Features = new List<Feature>();
			ad.DestinationCreative = null;

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving a promo ad that is click to jump with an empty Destination Creative throws error")]
		public void Ad_ClickToJump_With_Destination_Creative_Does_Not_Throw_Error()
		{
			var destinationCreativeId = 45;
			var ad = Ads.Get(1);
			var destinationCreativeName = "Test";

			SetupAdWithDestinationCreative(destinationCreativeId, ad);

			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			ad = Ads.Update(ad);
		}

		[Test(Description = "Saving a promo ad that is a Commercial Spot will not Create a Destination Ad")]
		public void Ad_CommercialSpot_Does_Not_Create_Destination_Ad()
		{
			var destinationCreativeId = 45;
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV];
			var ad = Ads.Get(1);
			ad.Platform = new Platform { Id = platformId };

			SetupCommercialSpotAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			// Set Ad's AdType to be Commercial Spot
			ad.AdType = GetCommercialSpotAdType(ad.Placement.AdTypeGroup);

			ad = Ads.Update(ad);

			// Assert there's no Destination Ad for this Ad
			Assert.IsNull(ad.DestinationAd);
		}
		
		[Test(Description = "Saving a promo ad that is click to jump with a Destination Creative will create Destination Ad in the background")]
		public void Ad_ClickToJump_With_Destination_Creative_Creates_Destination_Ad()
		{
			var destinationCreativeId = 45;
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV];
			var ad = Ads.Get(1);
			ad.Platform = new Platform { Id = platformId };

			SetupAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			ad = Ads.Update(ad);

			//now test that a Destination Ad was created for this promo ad
			var destinationCreative = Creatives.GetAll().ToList().Last();
			var destinationAd = Ads.GetAll().ToList().Last();
			Assert.AreEqual(destinationAd.Creative.Id, destinationCreativeId, "Destination Ad's Creative Id is not correct.");
			Assert.AreEqual(destinationAd.Platform.Id, ad.Platform.Id, "Destination Ad's Platform Id is not correct.");
			var destinationAdNameCompare = string.Format("{0}:{1}", destinationCreative.Name, ad.Platform.Name);
			Assert.IsTrue(destinationAd.Name.Contains(destinationAdNameCompare), "Destination Ad's Name Id is not correct.");
		}

		[Test(Description = "Saving a promo ad that is click to jump with a Destination Creative will not create a duplicate Destination Ad in the background")]
		public void Ad_ClickToJump_With_Destination_Creative_Will_Not_Create_Duplicate_Destination_Ad()
		{
			var destinationCreativeId = 45;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV];
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;
			
			//update first ad to create a destination ad in the background
			var ad = Ads.Get(1);
			ad.Platform = new Platform{Id = platformId};
			SetupAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);
			ad = Ads.Update(ad);

			//update second ad to assign the same destination ad in the background
			var ad2 = Ads.Get(2);
			SetupAdWithDestinationCreative(destinationCreativeId, ad2, creativeId, destinationCreativeName);
			ad2.Platform = new Platform{Id = platformId};
			ad2 = Ads.Update(ad2);

			//now test that a duplicate Brand Destination Ad was not created for this ad's platform and destination creative
			var destinationAds = Ads.Where(a => a.Creative.Id == destinationCreativeId && a.Platform.Id == platformId).ToList();
			Assert.IsTrue(destinationAds.Count() == 1);
		}

		[Test(Description = "(BL-275) Saving a promo ad will assign Ad Format in background when updating an Ad")]
		public void Ad_AdFormat_Created_In_Background_For_Update()
		{
			var destinationCreativeId = 45;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV];
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;

			//initialize ad so that it will satisify all ad validations
			var adFormatId = 3;
			var ad = Ads.Get(1);
			ad.Platform = new Platform { Id = platformId };
			SetupAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			// assign same AdFormat for ad and ad's Creative
			var adFormat = new AdFormat { Id = adFormatId };
			ad.AdFormat = adFormat;
			ad.Creative.AdFormat = adFormat;
			
			Ads.Update(ad);

			var adFromDb = Ads.Get(1);
			Assert.AreEqual(adFromDb.AdFormat.Id, adFormatId, "Ad's AdFormat is not correct.");
		}

		[Test(Description = "(BL-275) Saving a promo ad will assign Ad Format in background when creating an Ad")]
		public void Ad_AdFormat_Created_In_Background_For_Create()
		{
			var destinationCreativeId = 45;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV];
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;

			//initialize ad so that it will satisify all ad validations
			var adFormatId = 3;
			var adName = "test";
			var campaignId = 1;
			var placementId = 1;
			var deliveryGroupId = 1;
			var ad = CreateAdInstance(platformId, adName, campaignId, placementId, deliveryGroupId);

			SetupAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			// assign same AdFormat for ad and ad's Creative
			var adFormat = new AdFormat { Id = adFormatId };
			ad.AdFormat = adFormat;
			ad.Creative.AdFormat = adFormat;

			var adFromDb = Ads.Create(ad);

			Assert.AreEqual(adFromDb.AdFormat.Id, adFormatId, "Ad's AdFormat is not correct.");
		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Saves Coorectly")]
		public void Ad_Coordinates_For_Roku_Saves()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 55;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22;

			var adFromDb = CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

			Assert.AreEqual(adFromDb.XCoordinateHd, xCoordinateHd, "Ad's X Coordinate Hd is not correct.");
			Assert.AreEqual(adFromDb.YCoordinateHd, yCoordinateHd, "Ad's X Coordinate Hd is not correct.");
			Assert.AreEqual(adFromDb.XCoordinateSd, xCoordinateSd, "Ad's X Coordinate Hd is not correct.");
			Assert.AreEqual(adFromDb.YCoordinateSd, yCoordinateSd, "Ad's X Coordinate Hd is not correct.");
		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if SD X Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_SD_X_Coordinate_Upper_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 55555;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if SD Y Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_SD_Y_Coordinate_Upper_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 55;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22222;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if HD X Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_HD_X_Coordinate_Upper_Range()
		{
			var xCoordinateHd = 103330;
			var xCoordinateSd = 55;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if HD Y Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_HD_Y_Coordinate_Upper_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 55;
			var yCoordinateHd = 22200;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if SD X Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_SD_X_Coordinate_Lower_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = -33;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if SD Y Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_SD_Y_Coordinate_Lower_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 33;
			var yCoordinateHd = 200;
			var yCoordinateSd = -22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if HD X Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_HD_X_Coordinate_Lower_Range()
		{
			var xCoordinateHd = -33;
			var xCoordinateSd = 33;
			var yCoordinateHd = 200;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

		}

		[Test(Description = "(BL-383) Ad for Roku SD/HD and X/Y Coordinates Throws Exception if HD Y Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Roku_Throws_Exception_For_HD_Y_Coordinate_Lower_Range()
		{
			var xCoordinateHd = 100;
			var xCoordinateSd = 33;
			var yCoordinateHd = -44;
			var yCoordinateSd = 22;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-383) Ad for Non Roku X/Y Coordinates Saves Coorectly")]
		public void Ad_Coordinates_For_Non_Roku_Saves()
		{
			int? xCoordinateHd = 100;
			int? xCoordinateSd = null;
			int? yCoordinateHd = 200;
			int? yCoordinateSd = null;

			var adFromDb = CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);

			Assert.AreEqual(adFromDb.XCoordinateHd, xCoordinateHd, "Ad's X Coordinate Hd is not correct.");
			Assert.AreEqual(adFromDb.YCoordinateHd, yCoordinateHd, "Ad's Y Coordinate Hd is not correct.");
			Assert.IsNull(adFromDb.XCoordinateSd, "Ad's X Coordinate Sd should be null.");
			Assert.IsNull(adFromDb.YCoordinateSd, "Ad's Y Coordinate Sd should be null.");
		}

		[Test(Description = "(BL-383) Ad for Non Roku X/Y Coordinates Throws Exception if HD X Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Non_Roku_Throws_Exception_For_HD_X_Coordinate_Above_Range()
		{
			int? xCoordinateHd = 19900;
			int? xCoordinateSd = null;
			int? yCoordinateHd = 200;
			int? yCoordinateSd = null;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-383) Ad for Non Roku X/Y Coordinates Throws Exception if HD Y Coordinate is above acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Non_Roku_Throws_Exception_For_HD_Y_Coordinate_Above_Range()
		{
			int? xCoordinateHd = 33;
			int? xCoordinateSd = null;
			int? yCoordinateHd = 233333;
			int? yCoordinateSd = null;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-383) Ad for Non Roku X/Y Coordinates Throws Exception if HD X Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Non_Roku_Throws_Exception_For_HD_X_Coordinate_Below_Range()
		{
			int? xCoordinateHd = -22;
			int? xCoordinateSd = null;
			int? yCoordinateHd = 200;
			int? yCoordinateSd = null;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-383) Ad for Non Roku X/Y Coordinates Throws Exception if HD Y Coordinate is below acceptable range")]
		[ExpectedException(typeof(ValidationException))]
		public void Ad_Coordinates_For_Non_Roku_Throws_Exception_For_HD_Y_Coordinate_Below_Range()
		{
			int? xCoordinateHd = 44;
			int? xCoordinateSd = null;
			int? yCoordinateHd = -33;
			int? yCoordinateSd = null;

			CreateAdForCoordinatesTest(xCoordinateHd, xCoordinateSd, yCoordinateHd, yCoordinateSd);
		}

		[Test(Description = "(BL-462) Ad Tracking Events has correct valuesfor new Ad")]
		public void Ad_Tracking_Events_Has_Correct_Values_For_New_Ad()
		{
			var adsRepo = IoC.Resolve<IAdService>();

			var campaigns = IoC.Resolve<ICampaignService>();
			var adId = 0;
			var campaignId = 1;
			var creativeId = 1;
			var destinationAdCreativeId = 1;
			var adFunctionId = 1;
			var placementId = 1;
			var adTypeGroupId = 1;
			var deliveryGroupId = 1;
			var trackingEventId1 = 1;
			var trackingEventId2 = 2;
			var trackingUrl1 = "www.tracking1.com/track";
			var trackingUrl2 = "www.tracking2.com/track";
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];

			SetupObjectGraphForTrackingEventsTests(campaigns, campaignId, creativeId, adTypeId, adFunctionId, placementId, adTypeGroupId);

			var viewModel = CreateAdViewModelPrimaryFieldsForTrackingEventsTests(adId, campaignId, adTypeId, creativeId, destinationAdCreativeId, placementId, deliveryGroupId);
			CreateAdTrackingEventsForAd(viewModel, adId, trackingEventId1, trackingEventId2, trackingUrl1, trackingUrl2);
			var ad = adsRepo.SaveAd(viewModel);

			var adTrackingEvents = ad.AdTrackingEvents.ToList();
			Assert.IsTrue(adTrackingEvents.Count() == 2, "Ad does not have correct number of AdTrackingEvents.");
			var adTrackingEvent1 = adTrackingEvents.ElementAt(0);
			AssertAdTrackingEventRecord(trackingEventId1, trackingUrl1, adTrackingEvent1);
			var adTrackingEvent2 = adTrackingEvents.ElementAt(1);
			AssertAdTrackingEventRecord(trackingEventId2, trackingUrl2, adTrackingEvent2);
		}

		[Test(Description = "(BL-462) Ad Tracking Events has correct values for existing Ad")]
		public void Ad_Tracking_Events_Has_Correct_Values_For_Existing_Ad()
		{
			var adsRepo = IoC.Resolve<IAdService>();

			var campaigns = IoC.Resolve<ICampaignService>();
			var adId = 1;
			var campaignId = 1;
			var creativeId = 1;
			var destinationAdCreativeId = 1;
			var adFunctionId = 1;
			var placementId = 1;
			var adTypeGroupId = 1;
			var deliveryGroupId = 1;
			var trackingEventId1 = 1;
			var trackingEventId2 = 2;
			var trackingUrl1 = "www.tracking1.com/track";
			var trackingUrl2 = "www.tracking2.com/track";
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];

			SetupObjectGraphForTrackingEventsTests(campaigns, campaignId, creativeId, adTypeId, adFunctionId, placementId, adTypeGroupId);

			var viewModel = CreateAdViewModelPrimaryFieldsForTrackingEventsTests(adId, campaignId, adTypeId, creativeId, destinationAdCreativeId, placementId, deliveryGroupId);
			CreateAdTrackingEventsForAd(viewModel, adId, trackingEventId1, trackingEventId2, trackingUrl1, trackingUrl2);
			var ad = adsRepo.SaveAd(viewModel);

			var adTrackingEvents = ad.AdTrackingEvents.ToList();
			Assert.IsTrue(adTrackingEvents.Count() == 2, "Ad does not have correct number of AdTrackingEvents.");
			var adTrackingEvent1 = adTrackingEvents.ElementAt(0);
			AssertAdTrackingEventRecord(trackingEventId1, trackingUrl1, adTrackingEvent1);
			var adTrackingEvent2 = adTrackingEvents.ElementAt(1);
			AssertAdTrackingEventRecord(trackingEventId2, trackingUrl2, adTrackingEvent2);
		}

		[Test(Description = "(BL-462) Ad Tracking Events for Ad should be null if there are no tracking events to save for Ad.")]
		public void Ad_Tracking_Events_Should_Be_Null_For_Ad()
		{
			var adsRepo = IoC.Resolve<IAdService>();

			var campaigns = IoC.Resolve<ICampaignService>();
			var adId = 1;
			var campaignId = 1;
			var creativeId = 1;
			var destinationAdCreativeId = 1;
			var adFunctionId = 1;
			var placementId = 1;
			var adTypeGroupId = 1;
			var deliveryGroupId = 1;
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];

			SetupObjectGraphForTrackingEventsTests(campaigns, campaignId, creativeId, adTypeId, adFunctionId, placementId, adTypeGroupId);

			var viewModel = CreateAdViewModelPrimaryFieldsForTrackingEventsTests(adId, campaignId, adTypeId, creativeId, destinationAdCreativeId, placementId, deliveryGroupId);
			var ad = adsRepo.SaveAd(viewModel);

			Assert.AreEqual(ad.AdTrackingEvents.Count(), 0,  "Ad's AdTrackingEvents should be null.");
		}

		#region Private Methods

		private void AssertAdTrackingEventRecord(int trackingEventId, string trackingUrl, AdTrackingEvent adTrackingEvent)
		{
			Assert.IsTrue(adTrackingEvent.TrackingEvent_Id == trackingEventId, "Ad Tracking Event Id is not correct.");
			Assert.IsTrue(adTrackingEvent.TrackingUrl == trackingUrl, "Ad Tracking Url is not correct.");
		}

		private void SetupObjectGraphForTrackingEventsTests(ICampaignService campaigns, int campaignId, int creativeId, int adTypeId, int adFunctionId, int placementId, int adTypeGroupId)
		{
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();

			// set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(campaignId);
			campaign.Ads = new List<Ad>();
			campaign.Creatives = new List<Creative>{new Creative{Id = 222, AdType = new AdType{Id = adTypeId}}}; // hard code a creative id that will not be used by any unit tests, since it just needs to be unique
	
			var creative = creatives.Get(creativeId);
			creative.AdFunction = new AdFunction { Id = adFunctionId };
			creative.Campaign = new Campaign {Id = campaignId};
			creative.AdType = new AdType{Id = adTypeId};

			var placement = placements.Get(placementId);
			placement.AdTypeGroup = new AdTypeGroup { Id = adTypeGroupId };

			var adType = adTypes.Get(adTypeId);
			adType.AdTypeGroups = new List<AdTypeGroup> { new AdTypeGroup { Id = adTypeGroupId } };

		}

		private Ad CreateAdForCoordinatesTest(int? xCoordinateHd, int? xCoordinateSd, int? yCoordinateHd, int? yCoordinateSd)
		{
			var destinationCreativeId = 45;
			var platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var destinationCreativeName = "test destination creative";
			var creativeId = 122;

			//initialize ad so that it will satisify all ad validations
			var adName = "test";
			var campaignId = 1;
			var placementId = 1;
			var deliveryGroupId = 1;
			var ad = CreateAdInstance(platformId, adName, campaignId, placementId, deliveryGroupId);

			SetupAdWithDestinationCreative(destinationCreativeId, ad, creativeId, destinationCreativeName);
			CreateDestinationCreativeForAd(destinationCreativeId, ad, destinationCreativeName);

			ad.XCoordinateHd = xCoordinateHd;
			ad.YCoordinateHd = yCoordinateHd;
			ad.XCoordinateSd = xCoordinateSd;
			ad.YCoordinateSd = yCoordinateSd;

			var adFromDb = Ads.Create(ad);
			return adFromDb;
		}

		private AdViewModel CreateAdViewModelPrimaryFieldsForTrackingEventsTests(int adId, int campaignId, int adTypeId, int creativeId, int destinationAdCreativeId, int placementId, int deliveryGroupId)
		{
			var viewModel = new AdViewModel
			{
				id = adId,
				adFormatId = 1,
				creativeId = creativeId,
				adTagId = 1,
				adTypeGroupId = 1,
				deliveryGroupId = deliveryGroupId,
				adTypeId = adTypeId,
				beginDate = "3/1/16",
				endDate = "4/1/16",
				campaignId = campaignId,
				destinationAdCreativeId = destinationAdCreativeId,
				isReported = false,
				name = "Test Ad",
				placementId = placementId,
				platformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.DirecTV],
				xCoordinateHd = 22,
				yCoordinateHd = 33,
				adTrackingEvents = new List<AdTrackingEventViewModel>()
			};
			return viewModel;
		}

		private void CreateAdTrackingEventsForAd(AdViewModel viewModel, int adId, int trackingEventId1, int trackingEventId2, string trackingUrl1, string trackingUrl2)
		{
			viewModel.adTrackingEvents = new List<AdTrackingEventViewModel>
			{
				new AdTrackingEventViewModel
				{
					id = 0,
					adId = adId,
					trackingEventId = trackingEventId1,
					trackingUrl = trackingUrl1
				},
				new AdTrackingEventViewModel
				{
					id = 0,
					adId = adId,
					trackingEventId = trackingEventId2,
					trackingUrl = trackingUrl2
				}				
			};
			
		}

		private  Ad CreateAdInstance(int platformId, string adName, int campaignId, int placementId, int deliveryGroupId)
		{
			var ad = new Ad();
			ad.Campaign = new Campaign { Id = campaignId, Creatives = new List<Creative>(), Ads = new List<Ad>() };
			ad.Placement = new Placement { Id = placementId };
			ad.Platform = new Platform { Id = platformId };
			ad.Name = adName;
			ad.DeliveryGroup = new DeliveryGroup { Id = deliveryGroupId };
			return ad;
		}

		private void SetupCommercialSpotAdWithDestinationCreative(int destinationCreativeId, Ad ad, int creativeId = 1, string destinationCreativeName = "test destination creative")
		{
			//assign ad type group for ad's placement to be contained inside ad's ad type groups
			ad.AdType = GetCommercialSpotAdType(ad.Placement.AdTypeGroup);

			//set up both ads to have the same campaign and creative
			ad.Campaign = ad.Campaign;
			ad.Campaign.Creatives.Add(GetCreativeForCommercialSpotAdType(ad));
			ad.Creative = GetCreativeForCommercialSpotAdType(ad, creativeId);

			//ad's creative needs to be click to jump for the destination creative association to happen
			ad.Creative.AdFunction = GetClickToJumpAdFunction();

			ad.Features = new List<Feature>();
			ad.DestinationCreative = destinationCreativeId;
		}

		private void SetupAdWithDestinationCreative(int destinationCreativeId, Ad ad, int creativeId = 1, string destinationCreativeName = "test destination creative")
		{
			//assign ad type group for ad's placement to be contained inside ad's ad type groups
			ad.AdType = GetOverlayAdType(ad.Placement.AdTypeGroup);

			//set up both ads to have the same campaign and creative
			ad.Campaign = ad.Campaign;
			ad.Campaign.Creatives.Add(GetCreativeForOverlayAdType(ad));
			ad.Creative = GetCreativeForOverlayAdType(ad, creativeId);

			//ad's creative needs to be click to jump for the destination creative association to happen
			ad.Creative.AdFunction = GetClickToJumpAdFunction();

			ad.Features = new List<Feature>();
			ad.DestinationCreative = destinationCreativeId;
		}

		private void CreateDestinationCreativeForAd(int destinationCreativeId, Ad ad, string destinationCreativeName)
		{
			//create a destination creative that the ad will associate with
			var destinationCreative = new Creative { Id = destinationCreativeId, Campaign = ad.Campaign, Features = new List<Feature>(), AdType = GetBrandDestinationAdType(null), AdFunction = GetOnDemandAdFunction() };
			destinationCreative.Campaign.Creatives.Add(destinationCreative);
			destinationCreative.Name = destinationCreativeName;
			Creatives.Create(destinationCreative);
		}


		private  AdFunction GetClickToJumpAdFunction()
		{
			return new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump] };
		}

		private  AdFunction GetOnDemandAdFunction()
		{
			return new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand] };
		}

		private Creative GetCreativeForCommercialSpotAdType(Ad ad, int creativeId = 1)
		{
			return new Creative { Id = creativeId, AdType = GetCommercialSpotAdType(ad.Placement.AdTypeGroup), Features = new List<Feature>(), Campaign = ad.Campaign };
		}

		private  Creative GetCreativeForOverlayAdType(Ad ad, int creativeId = 1)
		{
			return new Creative { Id = creativeId, AdType = GetOverlayAdType(ad.Placement.AdTypeGroup), Features = new List<Feature>(), Campaign = ad.Campaign };
		}

		private  Creative GetCreativeForBrandDestinationAdType(Ad ad)
		{
			return new Creative { AdType = GetBrandDestinationAdType(ad.Placement.AdTypeGroup), Features = new List<Feature>(), Campaign = ad.Campaign };
		}	

		private  AdType GetBrandDestinationAdType(AdTypeGroup adTypeGroup)
		{
			return new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeGroups = new List<AdTypeGroup> { adTypeGroup }, IsPromo = false };
		}

		private  AdType GetOverlayAdType(AdTypeGroup adTypeGroup)
		{
			return new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay], AdTypeGroups = new List<AdTypeGroup> { adTypeGroup } };
		}

		private AdType GetCommercialSpotAdType(AdTypeGroup adTypeGroup)
		{
			return new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeGroups = new List<AdTypeGroup> { adTypeGroup } };
		}

		private  Creative GetCreativeForImageBannerAdType(Ad ad)
		{
			return new Creative { AdType = GetImageBannerAdType(ad.Placement.AdTypeGroup), Features = new List<Feature>(), Campaign = ad.Campaign };
		}

		private  AdType GetImageBannerAdType(AdTypeGroup adTypeGroup)
		{
			return new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner], AdTypeGroups = new List<AdTypeGroup> { adTypeGroup } };
		}

		private  Creative GetCreativeForVideoBannerAdType(Ad ad)
		{
			return new Creative { AdType = GetVideoBannerAdType(ad.Placement.AdTypeGroup), Features = new List<Feature>(), Campaign = ad.Campaign };
		}

		private  AdType GetVideoBannerAdType(AdTypeGroup adTypeGroup)
		{
			return new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.VideoBanner], AdTypeGroups = new List<AdTypeGroup> { adTypeGroup}, };
		}

		private void SetupAdWithCommercialSpotAdType(Ad ad)
		{
			ad.AdType = GetCommercialSpotAdType();
			ad.Campaign.Ads = new List<Ad>{
				MockEntities.CreateAd(122, new DateTime(2012, 2, 1), new DateTime(2013, 3, 1)) 
			};
			ad.Campaign.Creatives.Add(new Creative { AdType = GetCommercialSpotAdType() }); //need to add a creative with adType equal to commerical spot 
			ad.Creative.AdType = GetCommercialSpotAdType();
		}

		private AdType GetCommercialSpotAdType()
		{
			return new AdType{Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeGroups = new List<AdTypeGroup>{new AdTypeGroup{Id = 1}}};
		}

		#endregion

	}
}
