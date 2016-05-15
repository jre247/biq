using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.ViewModels.Ads;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
    public class AdViewModel
    {
		public int id { get; set; }
		public string name { get; set; }
		public string beginDate { get; set; }
		public string endDate { get; set; }
		public int? companionAdId { get; set; }
		public int? destinationCreativeId { get; set; }
		public int? destinationAdId { get; set; }
		public int? destinationAdCreativeId { get;set;}
		public int deliveryGroupId { get; set; }
		public int adTagId { get; set; }
		public int adTypeId { get; set; }
		public int creativeId { get; set; }
		public int adTypeGroupId { get; set; }
		public int adFormatId { get; set; }
		public int platformId { get; set; }
		public int placementId { get; set; }
		public int campaignId { get; set; }
		public bool isReported { get; set; }
		public int? xCoordinateSd { get; set; }
		public int? xCoordinateHd { get; set; }
		public int? yCoordinateSd { get; set; }
		public int? yCoordinateHd { get; set; }
		public IEnumerable<AdTrackingEventViewModel> adTrackingEvents { get; set; }

		public AdViewModel()
		{ }

		public AdViewModel(Ad ad)
		{
			id = ad.Id;
			name = ad.Name;
			beginDate = DateHelper.ToString(ad.BeginDate);
			endDate = DateHelper.ToString(ad.EndDate);
			isReported = ad.IsReported;
			companionAdId = ad.CompanionAd != null ? ad.CompanionAd.Id : (int?)null;
			destinationAdId = ad.DestinationAd != null ? ad.DestinationAd.Id : (int?)null;
			destinationAdCreativeId = ad.DestinationAd != null ? ad.DestinationAd.Creative.Id : (int?)null;
			deliveryGroupId = ad.DeliveryGroup.Id;
			adTagId = ad.AdTag.Id;
			adTypeId = ad.AdType.Id;
			creativeId = ad.Creative.Id;
			adTypeGroupId = ad.AdTypeGroup.Id;
			adFormatId = ad.AdFormat.Id;
			platformId = ad.Platform.Id;
			placementId = ad.Placement.Id;
			campaignId = ad.Campaign.Id;
			xCoordinateSd = ad.XCoordinateSd;
			xCoordinateHd = ad.XCoordinateHd;
			yCoordinateSd = ad.YCoordinateSd;
			yCoordinateHd = ad.YCoordinateHd;
			adTrackingEvents = ad.AdTrackingEvents.Select(a => new AdTrackingEventViewModel{
				id = a.Id,
				adId = a.Ad.Id,
				trackingEventId = a.TrackingEvent.Id,
				trackingUrl = a.TrackingUrl
			}).ToList();
		}

		public static JObject ToJObject(AdViewModel ad)
		{
			if (ad == null)
				return null;

			var json = JObject.FromObject(ad);
			return json;
		}

		
    }
}