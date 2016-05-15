using BrightLine.Common.Models.Enums;
using BrightLine.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Framework;
using BrightLine.Common.Services;

namespace BrightLine.Common.ViewModels.Campaigns
{
	public class CampaignSummaryViewModel
	{
		private IResourceHelper ResourceHelper { get; set; }

		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string dateUpdated { get { return DateHelper.ToString(dateUpdatedRaw); } }
		public string salesForceId { get; set; }
		public string productName { get; set; }
		public string brandName { get; set; }
		public string advertiserName { get; set; }
		public string thumbnail { get; set; }
		public bool isDeleted { get; set; }
		public bool hasAnalytics { get; set; }
		public string analyticsBeginDate { get; set; }
		public string analyticsEndDate { get; set; }
		public DateTime databaseDate { get; set; }
		public string beginDate { get { return DateHelper.ToString(beginDateRaw); } }
		public string endDate { get { return DateHelper.ToString(endDateRaw); } }
		public IEnumerable<int> platforms { get; set; }
		public IEnumerable<int> mediaPartners { get; set; }
		public string timeInterval { get; set;}

		public CampaignSummaryViewModel()
		{
			ResourceHelper = IoC.Resolve<IResourceHelper>();
		}

		public ResourceViewModel resource
		{
			get
			{
				if (resourceId.HasValue)
				{
					return ResourceHelper.GetResourceViewModel(resourceId.Value, resourceFilename, resourceName, resourceType.Value, id);
				}

				return null;
			}
		}

		public string status
		{
			get
			{
				return CampaignHelper.GetCampaignStatus(beginDate, endDate);
			}
		}

		#region JsonIgnore properties

		[JsonIgnore]
		public int? resourceId { get; set; }
		[JsonIgnore]
		public string resourceFilename { get; set; }
		[JsonIgnore]
		public string resourceName { get; set; }
		[JsonIgnore]
		public int? resourceType { get; set; }
		[JsonIgnore]
		public DateTime? dateUpdatedRaw { get; set; }
		[JsonIgnore]
		public DateTime? beginDateRaw { get; set; }
		[JsonIgnore]
		public DateTime? endDateRaw { get; set; }

		#endregion

		public static JObject ToJObject(CampaignSummaryViewModel summary)
		{
			if (summary == null)
				return null;

			var json = JObject.FromObject(summary);
			return json;
		}
	}
}