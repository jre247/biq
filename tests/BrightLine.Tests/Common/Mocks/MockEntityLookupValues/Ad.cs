using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		/// <summary>
		/// Gets ad formats for testing.
		/// </summary>
		/// <returns></returns>
		public static List<AdFormat> GetAdFormats()
		{
			var items = new List<AdFormat>()
				{
					new AdFormat() {Id = 1, Name = "Horizontal"},
					new AdFormat() {Id = 2, Name = "Vertical"},
					new AdFormat() {Id = 3, Name = "Square"},
					new AdFormat() {Id = 4, Name = "Player"},
					new AdFormat() {Id = 5, Name = "Menu"},
					new AdFormat() {Id = 6, Name = "Miscellaneous"},
					new AdFormat() {Id = 7, Name = "Full-Screen"},
					new AdFormat() {Id = 8, Name = "Pop Up"},
					new AdFormat() {Id = 9, Name = "Single Page"},
					new AdFormat() {Id = 10, Name = "Multi Page"},
					new AdFormat() {Id = 11, Name = "Overlay"},
					new AdFormat() {Id = 12, Name = "Pre Roll"},
					new AdFormat() {Id = 13, Name = "Mid Roll"},
					new AdFormat() {Id = 14, Name = "Post Roll"},
					new AdFormat() {Id = 15, Name = "MediaPartner Guide"},
					new AdFormat() {Id = 16, Name = "Store"},
					new AdFormat() {Id = 17, Name = ":15"},
					new AdFormat() {Id = 18, Name = ":30"},
					new AdFormat() {Id = 19, Name = ":60"}
				};
			return items;
		}


		/// <summary>
		/// Gets ad functions for testing.
		/// </summary>
		/// <returns></returns>
		public static List<AdFunction> GetAdFunctions()
		{
			var items = new List<AdFunction>()
				{
					new AdFunction() {Id = 1, Name = "Not Clickable"},
					new AdFunction() {Id = 2, Name = "Interactive"},
					new AdFunction() {Id = 3, Name = "Click to Video"},
					new AdFunction() {Id = 4, Name = "Click to Jump"},
				};
			return items;
		}


		/// <summary>
		/// Gets adtype groups for testing.
		/// </summary>
		/// <returns></returns>
		public static List<AdTypeGroup> GetAdTypeGroup()
		{
			var items = new List<AdTypeGroup>()
				{
					new AdTypeGroup() {Id = 10001, Name = "Banner"},
					new AdTypeGroup() {Id = 10002, Name = "Skin"},
					new AdTypeGroup() {Id = 10003, Name = "Roadblock"},
					new AdTypeGroup() {Id = 10004, Name = "Destination"},
					new AdTypeGroup() {Id = 10005, Name = "In-Video"},
					new AdTypeGroup() {Id = 10006, Name = "Listing"},
					new AdTypeGroup() {Id = 10007, Name = "Commercial Spot"}
				};
			return items;
		}

		public static List<Ad> CreateAds()
		{
			var recs = new List<Ad>()
				{
					CreateAd(1, "ad 1", new DateTime(2012, 2, 1), new DateTime(2013, 3, 1)),
					CreateAd(2, "ad 2", new DateTime(2012, 4, 1), new DateTime(2013, 5, 1))
					
				};

			return recs;
		}

		public static Ad CreateAd(int adId, DateTime beginDate, DateTime endDate)
		{
			return new Ad
			{
				Id = adId,
				BeginDate = beginDate,
				EndDate = endDate
			};
		}

		public static Ad CreateAd(int adId, int adTypeId, DateTime beginDate, DateTime endDate)
		{
			return new Ad
			{
				Id = adId,
				BeginDate = beginDate,
				EndDate = endDate,
				AdType = new AdType { Id = adTypeId }
			};
		}

		public static Ad CreateAd(int adId, string adName, DateTime beginDate, DateTime endDate, int campaignId = 1, int adFormatId = 1, string adFormatName = "Format 1", 
			int adTagId = 1, int platformId = 1, string plaformName = "Roku", int placementId = 25, string placementName = "placement 1", int MediaPartnerId = 1, string MediaPartnerName = "MediaPartner 1",
			int creativeId = 1, string creativeName = "creative 1", int adTypeId = 10001, string adTypeName = "ad type 1", int adFunctionId = 1, string adFunctionName = "ad function 1",
			int deliveryGroupId = 1, string deliveryGroupName = "group 1", int creativeAdType = 1)
		{
			var adTypeGroups = new List<AdTypeGroup>{ new AdTypeGroup{Id = 1}};
			var adType = new AdType { Id = adTypeId, AdTypeGroups = adTypeGroups };
			var campaign = new Campaign { Id = campaignId, Ads = new List<Ad>(), Creatives = new List<Creative> { new Creative { Id = 1, Features = new List<Feature> { new Feature { Id = 1 } }, AdType = adType } } };

			return new Ad()
			{
				Id = adId,
				Name = adName,
				Campaign = campaign,
				AdFormat = new AdFormat { Id = adFormatId, Name = adFormatName },
				AdTag = new AdTag { Id = adTagId },
				Platform = new Platform { Id = platformId, Name = plaformName },
				Placement = new Placement { Id = placementId, Name = placementName, AdTypeGroup = new AdTypeGroup{Id = 1}, MediaPartner = new MediaPartner { Id = MediaPartnerId, Name = MediaPartnerName } },
				Creative = new Creative { Id = creativeId, Name = creativeName, AdType = adType, Campaign = campaign, Features = new List<Feature>{new Feature{Id = 1}} },
				AdType = adType,
				AdFunction = new AdFunction { Id = adFunctionId, Name = adFunctionName },
				BeginDate = beginDate,
				EndDate = endDate,
				DeliveryGroup = new DeliveryGroup { Id = deliveryGroupId, Name = deliveryGroupName }
			};
		}

		public static List<AdType> CreateAdTypes()
		{
			var items = new List<AdType>()
				{
					new AdType() {Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], Name = AdTypeConstants.AdTypeNames.BrandDestination},
					new AdType() {Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay], Name = AdTypeConstants.AdTypeNames.Overlay}
				};
			return items;
		}

		public static List<AdTypeGroup> CreateAdTypeGroups()
		{
			var items = new List<AdTypeGroup>()
				{
					new AdTypeGroup() {Id = Lookups.AdTypeGroups.HashByName[AdTypeGroupConstants.AdTypeGroupNames.Destination], Name = AdTypeGroupConstants.AdTypeGroupNames.Destination},
				};
			return items;
		}

		public static List<AdTag> CreateAdTags()
		{
			var items = new List<AdTag>()
				{
					new AdTag() {Id = 1},
					new AdTag() {Id = 2},
				};
			return items;
		}
		
	}

}