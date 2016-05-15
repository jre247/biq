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
	public class CampaignCreativeViewModel
	{
		private IResourceHelper ResourceHelper { get; set; }

		public int id { get; set; }
		public string name { get; set; }
		public int height { get; set; }
		public int width { get; set; }
		public int campaignId { get;set;}

		public int? size
		{
			get
			{
				return resourceRaw != null ? resourceRaw.Size : null;
			}
		}

		public string lastModified
		{
			get
			{
				if (!lastModifiedRaw.HasValue)
					return null;

				return DateHelper.ToUserTimezone(lastModifiedRaw.Value).ToString(DateHelper.DATE_FORMAT);
			}
		}

		public ResourceViewModel resource
		{
			get
			{
				if (resourceId.HasValue)
				{
					return ResourceHelper.GetResourceViewModel(resourceId.Value, resourceFilename, resourceName, resourceType.Value, campaignId);
				}

				return null;
			}
		}

		public bool isDeleted { get; set; }
		public int? adTypeId { get; set; }
		public string adTypeName { get; set; }
		public int? adFunctionId { get; set; }
		public string adFunctionName { get; set; }
		public IEnumerable<int> features { get; set; }
		public bool isPromo { get; set; }

		public CampaignCreativeViewModel()
		{
			ResourceHelper = IoC.Resolve<IResourceHelper>();
		}

		#region JsonIgnore properties

		[JsonIgnore]
		public int? resourceId { get;set;}
		[JsonIgnore]
		public string resourceName { get;set;}
		[JsonIgnore]
		public string resourceFilename { get;set;}
		[JsonIgnore]
		public int? resourceType { get;set;}
		[JsonIgnore]
		public Resource resourceRaw { get; set; }
		[JsonIgnore]
		public DateTime? lastModifiedRaw { get; set; }

		
		

		#endregion

		public static JObject ToJObject(IEnumerable<CampaignCreativeViewModel> creatives, string property)
		{
			if (creatives == null)
				return null;

			var json = new JObject();
			json[property] = ParseCreatives(creatives);
			return json;
		}

		#region Private methods

		
		private static JObject ParseCreatives(IEnumerable<CampaignCreativeViewModel> creatives)
		{
			var creativesJson = new JObject();
			if (creatives == null)
				return creativesJson;

			foreach (var creative in creatives)
			{
				var key = creative.id.ToString(CultureInfo.InvariantCulture);
				var content = creative;
				var jo = JObject.FromObject(content);
				var creativeJson = new JProperty(key, jo);
				creativesJson.Add(creativeJson);
			}

			var json = JObject.FromObject(creativesJson);
			return json;
		}

		#endregion
	}
}