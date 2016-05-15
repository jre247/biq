using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
	public class CampaignSettingViewModel
    {
        public string name { get; set; }
		public string displayName { get; set; }
		public List<FieldViewModel> fields { get; set; }

		public CampaignSettingViewModel()
		{

		}

		public CampaignSettingViewModel(CmsSetting cmsSetting)
		{
			name = cmsSetting.Name;
			displayName = cmsSetting.Display;
			fields = cmsSetting.CmsSettingDefinition.Fields.Select(f => new FieldViewModel(f, cmsSetting.Feature.Campaign.Id)).ToList();
		}

		public static JObject ToJObject(CampaignSettingViewModel campaignSettingViewModel)
		{
			if (campaignSettingViewModel == null)
				return null;

			var json = JObject.FromObject(campaignSettingViewModel);
			return json;
		}
    }
}