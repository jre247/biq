using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public class SettingInstanceLookups
	{
		public Dictionary<int, CmsField> SettingInstanceFieldsDictionary { get; set; }
		public CmsSettingInstance SettingInstance { get; set; }
		public Dictionary<int, Resource> FieldResourcesDictionary = null;
		public Dictionary<string, FieldViewModel> SettingInstanceFieldsHash { get; set; }
		public CmsSetting Setting { get;set;}

		public void BuildResourcesDictionary(ModelInstanceSaveViewModel viewModel)
		{
			var resourceIds = new List<int>();
			var fieldValues = viewModel.fields.SelectMany(f => f.value).ToList();
			foreach (var fieldValue in fieldValues)
			{
				int resourceId;
				var isParseValid = int.TryParse(fieldValue, out resourceId);
				if (!isParseValid)
					continue;

				resourceIds.Add(resourceId);
			}

			var resources = IoC.Resolve<IResourceService>();

			FieldResourcesDictionary = resources.Where(f => resourceIds.Contains(f.Id)).ToList().ToDictionary(x => x.Id, x => x);
		}


		/// <summary>
		/// This will build a dictionary of all model instance values for a specific model.
		/// The purpose of this hash is so you can quickly see in O(1) time if an instance field value is unique within all instances in a specific model.
		/// </summary>
		/// <param name="modelInstance"></param>
		/// <param name="_cmsModelInstanceRepo"></param>
		/// <returns></returns>
		public Dictionary<string, FieldViewModel> GetHashForSettingInstanceFieldValues()
		{
			if (SettingInstanceFieldsHash != null)
				return SettingInstanceFieldsHash;

			SettingInstanceFieldsHash = new Dictionary<string, FieldViewModel>();

			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();

			var cmsModelInstances = cmsSettingInstances.Where(m => m.Setting_Id == Setting.Id).Select(c => c.Json);
			if (cmsModelInstances == null)
				return null;

			var modelInstances = cmsModelInstances.ToList();

			foreach (var instance in modelInstances)
			{
				if (instance == null)
					continue;

				var fieldsDictionary = new Dictionary<string, FieldViewModel>();
				var deserializedInstance = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(instance);

				foreach (var field in deserializedInstance.fields)
				{
					foreach (var value in field.values)
					{
						if (value.Type == JTokenType.Null)
							continue;

						var valueFinal = value.ToObject<string>();

						//format the setting instance field's value to a YYYY MM dd format so that there is one common date format to compare against
						//when doing things like checking for unique field value
						if (field.type == FieldTypeConstants.FieldTypeNames.Datetime)
							valueFinal = CmsInstanceFieldValueHelper.FormatDateString(value, InstanceConstants.DateFormats.YearMonthDay);

						SettingInstanceFieldsHash.Add(valueFinal, field);
					}
				}
			}

			return SettingInstanceFieldsHash;
		}

		/// <summary>
		/// Build a dictionary of fields that map to the list of field ids from the viewmodel
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		public void BuildInstanceFieldsDictionary(ModelInstanceSaveViewModel viewModel, CmsSettingInstance modelInstance)
		{
			var viewModelFieldsIds = viewModel.fields.Select(f => f.id);
			SettingInstanceFieldsDictionary = Setting.CmsSettingDefinition.Fields.Where(f => viewModelFieldsIds.Contains(f.Id)).ToList().ToDictionary(x => x.Id, x => x);
		}


	}
}
