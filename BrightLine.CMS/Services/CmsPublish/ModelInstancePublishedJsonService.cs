using BrightLine.CMS.Services.ModelInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services.Publish
{
	public class ModelInstancePublishedJsonService : BaseModelInstanceService, IModelInstancePublishedJsonService
	{
		private ModelInstanceLookups ModelInstanceLookups { get;set;}

		public void SaveModelInstancePublishedJson(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance)
		{
			ModelInstanceLookups = GetLookupsForModelInstance(modelInstance);

			var modelInstancePublishedJsonVm = new ModelInstancePublishedJsonViewModel();
			var campaignId = modelInstance.Model.Feature.Campaign.Id;

			AddPrimaryProperties(modelInstance, modelInstancePublishedJsonVm);

			AddBLProperty(modelInstance, modelInstancePublishedJsonVm);

			AddModelInstanceFieldProperties(viewModel, modelInstancePublishedJsonVm, campaignId);

			modelInstance.PublishedJson = JsonConvert.SerializeObject(modelInstancePublishedJsonVm.Properties);
		}

		public object DeserializeModelInstancePropertyValue(KeyValuePair<string, object> item)
		{
			object value = null;
			string key = item.Key;

			if (key == CmsPublishConstants.ModelInstanceJsonProperties.Id ||
				key == CmsPublishConstants.ModelInstanceJsonProperties.ModelName)
			{
				// No need to deserialize the value, just get the raw value
				value = item.Value;
			}
			else if (key == CmsPublishConstants.ModelInstanceJsonProperties.BL)
			{
				value = JsonConvert.DeserializeObject<BLViewModel>(item.Value.ToString());
			}
			else
			{
				// Check if values are of type Model Ref 
				var isValueModelRef = ModelRefFieldValue.IsValidModelRef(item.Value.ToString());

				if (isValueModelRef)
				{
					value = JsonConvert.DeserializeObject<List<ModelRefFieldValue>>(item.Value.ToString());
				}
				// Value is just regular string array
				else
				{
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
				
			}

			return value;
		}

		private void AddModelInstanceFieldProperties(ModelInstanceSaveViewModel viewModel, ModelInstancePublishedJsonViewModel modelInstancePublishedJsonVm, int campaignId)
		{
			var fieldsResourceDictionary = ModelInstanceLookups.FieldResourcesDictionary;
			var modelInstanceFieldsDictionary = ModelInstanceLookups.ModelInstanceFieldsDictionary;

			foreach (var field in viewModel.fields)
			{
				var modelInstanceField = modelInstanceFieldsDictionary[field.id];

				var publishInstanceFieldPropertyService = new PublishInstanceFieldPropertyService(modelInstanceField, field.value, fieldsResourceDictionary, campaignId);
				var propertyValue = publishInstanceFieldPropertyService.GetFieldValue();

				modelInstancePublishedJsonVm.Properties.Add(field.name, propertyValue);
			}
		}

		private static void AddBLProperty(CmsModelInstance modelInstance, ModelInstancePublishedJsonViewModel modelInstancePublishedJsonVm)
		{
			var blViewModel = BLViewModel.Parse(modelInstance);
			var blJObject = BLViewModel.ToJObject(blViewModel);
			modelInstancePublishedJsonVm.Properties.Add(CmsPublishConstants.ModelInstanceJsonProperties.BL, blJObject.ToString());
		}

		/// <summary>
		/// Add Id and Name primary properties
		/// </summary>
		/// <param name="modelInstance"></param>
		/// <param name="modelInstancePublishedJsonVm"></param>
		private static void AddPrimaryProperties(CmsModelInstance modelInstance, ModelInstancePublishedJsonViewModel modelInstancePublishedJsonVm)
		{
			modelInstancePublishedJsonVm.Properties.Add(CmsPublishConstants.ModelInstanceJsonProperties.Id, modelInstance.Id.ToString());
			modelInstancePublishedJsonVm.Properties.Add(CmsPublishConstants.ModelInstanceJsonProperties.ModelName, modelInstance.Model.Name);
		}
	}
}
