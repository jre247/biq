using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Common.ViewModels.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services.Publish
{
	public class PublishInstanceFieldPropertyService
	{
		private CmsField Field;
		private List<string> ValuesToSave;
		private Dictionary<int, Resource> FieldResourcesDictionary;
		private int CampaignId;

		public PublishInstanceFieldPropertyService(CmsField field, List<string> valuesToSave, Dictionary<int, Resource> FieldResourcesDictionary, int campaignId)
		{
			this.Field = field;
			this.ValuesToSave = valuesToSave;
			this.FieldResourcesDictionary = FieldResourcesDictionary;
			this.CampaignId = campaignId;
		}

		public string GetFieldValue()
		{
			JArray values = null;
			var cmsFieldHelper = IoC.Resolve<ICmsFieldHelper>();

			if (Field.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel])
			{
				var fieldValueList = new List<ModelRefFieldValue>();
				foreach (var value in ValuesToSave)
				{
					var deserializedValue = JsonConvert.DeserializeObject<ModelRefFieldValue>(value);
					fieldValueList.Add(deserializedValue);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else if (Field.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage])
			{
				var fieldValueList = new List<PageRefFieldValue>();
				foreach (var value in ValuesToSave)
				{
					var deserializedValue = JsonConvert.DeserializeObject<PageRefFieldValue>(value);
					fieldValueList.Add(deserializedValue);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else if (cmsFieldHelper.IsFieldResource(Field))
			{
				var fieldValueList = new List<string>();
				foreach (var value in ValuesToSave)
				{
					if (value == null)
						continue;

					var fieldResource = FieldResourcesDictionary[int.Parse(value)];
					var resourceFieldValue = new ResourceViewModel(fieldResource);
					fieldValueList.Add(resourceFieldValue.filename);
				}

				values = JArray.FromObject(fieldValueList);
			}
			else
			{
				values = JArray.FromObject(ValuesToSave);
			}

			return values.ToString();
		}

	}
}
