using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.ViewModels.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class FieldViewModel
	{
		private CmsField modelInstanceField;
		private List<string> list1;
		private Dictionary<int, Resource> _fieldResourcesDictionary;

		public int id { get; set; }
		public string name { get; set; }
		public string displayName { get; set; }
		public string description { get; set; }
		public string type { get; set; }
		public bool list { get; set; }
		public RefViewModel refModel { get; set; }
		public Dictionary<string, string> validations { get; set; }
		public JArray values { get; set; }

		public FieldViewModel()
		{ }

		public FieldViewModel(CmsField cmsField, List<string> valuesToSet = null)
		{
			InitializeFieldViewModel(cmsField, valuesToSet, null);
		}

		public FieldViewModel(CmsField cmsField, int campaignId, List<string> valuesToSet = null)
		{
			InitializeFieldViewModel(cmsField, valuesToSet, campaignId);
		}

		public FieldViewModel(CmsField cmsField, List<string> valuesToSet, Dictionary<int, Resource> _fieldResourcesDictionary, int campaignId)
		{
			this._fieldResourcesDictionary = _fieldResourcesDictionary;

			InitializeFieldViewModel(cmsField, valuesToSet, campaignId);
		}

		private void InitializeFieldViewModel(CmsField cmsField, List<string> valuesToSet, int? campaignId)
		{
			id = cmsField.Id;
			name = cmsField.Name;
			displayName = cmsField.DisplayName;
			description = cmsField.Description;
			type = cmsField.Type.Name;
			list = cmsField.List;

			if (valuesToSet != null)
				SetFieldValues(cmsField, valuesToSet, campaignId);

			if (cmsField.CmsRef != null)
				refModel = new RefViewModel(cmsField.CmsRef);

			var validationsList = cmsField.Validations.Select(v => new ValidationViewModel(v)).ToList();
			validations = validationsList.ToDictionary(f => f.name, f => f.value);
		}

		/// <summary>
		/// Convert Field Value to its appropriate view model
		///		*note: Field with type Model Ref will have its Field Value have a specific view model
		///		*note: Field with type Page Ref will have its Field Value have a specific view model
		///		*note: Field with type Image or Video will have its Field Value have a specific view model
		/// </summary>
		/// <param name="cmsField"></param>
		/// <param name="valuesToSave"></param>
		private void SetFieldValues(CmsField cmsField, List<string> valuesToSave, int? campaignId)
		{
			if (values != null)
				return;

			if (cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel])
			{
				var fieldValueList = new List<ModelRefFieldValue>();
				foreach (var value in valuesToSave)
				{
					var deserializedValue =  JsonConvert.DeserializeObject<ModelRefFieldValue>(value);
					fieldValueList.Add(deserializedValue);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else if (cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage])
			{
				var fieldValueList = new List<PageRefFieldValue>();
				foreach (var value in valuesToSave)
				{
					var deserializedValue = JsonConvert.DeserializeObject<PageRefFieldValue>(value);
					fieldValueList.Add(deserializedValue);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else if (IsFieldResource(cmsField))
			{
				var fieldValueList = new List<ResourceViewModel>();
				foreach (var value in valuesToSave)
				{
					if (value == null)
						continue;

					var fieldResource = _fieldResourcesDictionary[int.Parse(value)];
					var resourceFieldValue = new ResourceViewModel(fieldResource);
					fieldValueList.Add(resourceFieldValue);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else
			{
				values = JArray.FromObject(valuesToSave);
			}
		}

		private bool IsFieldResource(CmsField cmsField)
		{
			return cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image] || cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Video];
		}
	}
}
