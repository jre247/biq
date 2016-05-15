using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.ViewModels.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
    public class CampaignAdViewModel
    {
		private static readonly IResourceHelper ResourceHelper;

        public int id { get; set; }
        public string name { get; set; }
		public string lastModified { get { return DateHelper.ToString(lastModifiedRaw); } }
        public string thumbnail { get; set; }
        public bool isDeleted { get; set; }
        public int adTypeId { get; set; }
		public int? deliveryGroupId { get; set; }
        public string adTypeName { get; set; }
        public int? placementId { get; set; }
        public int? adTag { get; set; }
        public long impressions { get; set; }
        public int? platformId { get; set; }
		public int mediaPartnerId { get; set; }
        public int? creativeId { get; set; }
		public int campaignId { get; set; }
		public string beginDate { get { return DateHelper.ToString(beginDateRaw);  } }
		public string endDate { get { return DateHelper.ToString(endDateRaw); } }
		public bool isPromo { get; set; }
		public bool isReported { get;set;}
		public int? xCoordinateSd { get;set;}
		public int? xCoordinateHd { get; set; }
		public int? yCoordinateSd { get; set; }
		public int? yCoordinateHd { get; set; }
		public string adTagUrl { get;set;}
		public string impressionUrl { get; set; }
		public string clickUrl { get; set; }

		static CampaignAdViewModel()
		{
			ResourceHelper = IoC.Resolve<IResourceHelper>();
		}

		public ResourceViewModel resource
		{
			get
			{
				if (resourceId.HasValue && resourceType.HasValue)
				{
					return ResourceHelper.GetResourceViewModel(resourceId.Value, resourceFilename, resourceName, resourceType.Value, campaignId);
				}

				return null;
			}
		}

        #region JsonIgnore properties
		
		[JsonIgnore]
		public int? resourceId { get; set; }
		[JsonIgnore]
		public string resourceName { get; set; }
		[JsonIgnore]
		public string resourceFilename { get; set; }
		[JsonIgnore]
		public int? resourceType { get; set; }
		[JsonIgnore]
		public DateTime? lastModifiedRaw { get; set; }
		[JsonIgnore]
		public DateTime? beginDateRaw;
		[JsonIgnore]
		public DateTime? endDateRaw;
		

        #endregion
        
        public static JObject ToJObject(IEnumerable<CampaignAdViewModel> ads, string property)
        {
            if (ads == null)
                return null;

            var json = new JObject();
            json[property] = ParseAds(ads);
            var platforms = ads.Select(ad => ad.platformId).Where(platformId => platformId != 0).Distinct();
			var mediaPartners = ads.Select(ad => ad.mediaPartnerId).Where(mediaPartnerId => mediaPartnerId != 0).Distinct();
            json["platforms"] = JArray.FromObject(platforms);
			json["mediaPartners"] = JArray.FromObject(mediaPartners);
            return json;
        }

		public static JObject ToJObject(CampaignAdViewModel ad)
		{
			if (ad == null)
				return null;

			var json = JObject.FromObject(ad);
			return json;
		}

        #region Protected methods

        protected static JObject ParseAds(IEnumerable<CampaignAdViewModel> ads)
        {
            var adsJson = new JObject();
            foreach (var ad in ads)
            {
                var key = ad.id.ToString(CultureInfo.InvariantCulture);
                var content = JObject.FromObject(ad);
                var adJson = new JProperty(key, content);
                adsJson.Add(adJson);
            }

            var json = JObject.FromObject(adsJson);
            return json;
        }

        #endregion
    }
}