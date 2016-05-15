using BrightLine.CMS.Services.SettingInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services.Publish
{
	public class SettingInstancePublishedJsonService : BaseSettingInstanceService, ISettingInstancePublishedJsonService
	{
		private SettingInstanceLookups SettingInstanceLookups;

		public SettingInstancePublishedJsonService()
		{ }

		public void SaveSettingInstancePublishedJson(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance)
		{
			SettingInstanceLookups = GetSettingInstanceLookups(settingInstance);

			var campaignId = SettingInstanceLookups.Setting.Feature.Campaign.Id;
			var settingInstancePublishedJsonVm = new SettingInstancePublishedJsonViewModel();
	
			AddSettingInstanceIdProperty(settingInstance, settingInstancePublishedJsonVm);

			AddSettingInstanceFieldProperties(viewModel, settingInstancePublishedJsonVm, campaignId);

			settingInstance.PublishedJson = JsonConvert.SerializeObject(settingInstancePublishedJsonVm.Properties);
		}

		public object DeserializeSettingnstancePropertyValue(KeyValuePair<string, object> item)
		{
			object value = null;
			string key = item.Key;

			if (key == CmsPublishConstants.ModelInstanceJsonProperties.Id)
			{
				// No need to deserialize the value, just get the raw value
				value = item.Value;
			}
			else
			{
				// If the key is not BL or Id then the value will initially be an array of strings. 
				var valueAsArray = JsonConvert.DeserializeObject<string[]>(item.Value.ToString());

				if (valueAsArray.Count() == 0)
					value = null;
				// If the array only has more than one item then return the whole array
				else if (valueAsArray.Count() > 1)
					value = valueAsArray;
				// Only return the first item in the array, since the array only has one item in it
				else
					value = valueAsArray.ElementAt(0);
			}

			return value;
		}

		private void AddSettingInstanceFieldProperties(ModelInstanceSaveViewModel viewModel, SettingInstancePublishedJsonViewModel settingInstancePublishedJsonVm, int campaignId)
		{
			foreach (var field in viewModel.fields)
			{
				var settingInstanceField = SettingInstanceLookups.SettingInstanceFieldsDictionary[field.id];

				var publishInstanceFieldPropertyService = new PublishInstanceFieldPropertyService(settingInstanceField, field.value, SettingInstanceLookups.FieldResourcesDictionary, campaignId);
				var propertyValue = publishInstanceFieldPropertyService.GetFieldValue();

				settingInstancePublishedJsonVm.Properties.Add(field.name, propertyValue);
			}
		}

		private static void AddSettingInstanceIdProperty(CmsSettingInstance settingInstance, SettingInstancePublishedJsonViewModel settingInstancePublishedJsonVm)
		{
			settingInstancePublishedJsonVm.Properties.Add(CmsPublishConstants.SettingInstanceJsonProperties.Id, settingInstance.Id.ToString());
		}

	}
}
