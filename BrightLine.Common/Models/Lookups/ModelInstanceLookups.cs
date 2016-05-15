using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Core;

namespace BrightLine.Common.Services
{
	/// <summary>
	/// This class will keep track of the following lookups for a specific Model Instance:
	///		1) A dictionary of resources
	///		2) A dictionary of CmsFields 
	///		3) A dictionary of model instances for each model
	///		4) The model instance itself
	/// </summary>
	public class ModelInstanceLookups
	{
		private IModelInstanceLookupsService ModelInstanceLookupsService { get;set;}
		private IResourceService Resources { get;set;}
		private IRepository<CmsModelInstance> CmsModelInstances { get;set;}

		public Dictionary<int, CmsField> ModelInstanceFieldsDictionary { get; set; }
		public CmsModelInstance ModelInstance { get; set; }
		public Dictionary<int, Resource> FieldResourcesDictionary = null;
		public Dictionary<string, FieldViewModel> ModelInstanceFieldsHash { get; set; }


		public ModelInstanceLookups()
		{
			ModelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			Resources = IoC.Resolve<IResourceService>();
			CmsModelInstances = IoC.Resolve<IRepository<CmsModelInstance>>();
		}

		/// <summary>
		/// Build a dictionary of all resources that map to fields in the viewmodel that have their value type equal to integer
		///		*note: it is assumed right now that when a viewmodel field's value is of type integer then that means it contains a resource id. 
		///		In the future this should be changed to have a strongly typed object like (resourceId: 5)
		/// </summary>
		/// <param name="viewModel"></param>
		public void BuildResourcesDictionary(ModelInstanceSaveViewModel viewModel, int modelInstanceId)
		{
			var modelInstanceLookups = ModelInstanceLookupsService.GetLookupsForModelInstance(modelInstanceId);

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

			modelInstanceLookups.FieldResourcesDictionary = Resources.Where(f => resourceIds.Contains(f.Id)).ToList().ToDictionary(x => x.Id, x => x);
			ModelInstanceLookupsService.SaveLookupsForModelInstance(modelInstanceLookups);
		}

		/// <summary>
		/// Build a dictionary of fields that map to the list of field ids from the viewmodel
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		public void BuildInstanceFieldsDictionary(ModelInstanceSaveViewModel viewModel, int modelInstanceId)
		{
			var modelInstanceLookups = ModelInstanceLookupsService.GetLookupsForModelInstance(modelInstanceId);
			var modelInstance = modelInstanceLookups.ModelInstance;

			var viewModelFieldsIds = viewModel.fields.Select(f => f.id);
			modelInstanceLookups.ModelInstanceFieldsDictionary = modelInstance.Model.CmsModelDefinition.Fields.Where(f => viewModelFieldsIds.Contains(f.Id)).ToList().ToDictionary(x => x.Id, x => x);

			ModelInstanceLookupsService.SaveLookupsForModelInstance(modelInstanceLookups);
		}

		/// <summary>
		/// This will build a dictionary of all model instance values for a specific model.
		/// The purpose of this hash is so you can quickly see in O(1) time if an instance field value is unique within all instances in a specific model.
		/// </summary>
		/// <param name="modelInstance"></param>
		/// <param name="IoC.CmsModelInstances"></param>
		/// <returns></returns>
		public Dictionary<string, FieldViewModel> GetHashForModelInstanceFieldValues(int modelInstanceId)
		{
			var modelInstanceLookups = ModelInstanceLookupsService.GetLookupsForModelInstance(modelInstanceId);
			var modelInstanceFieldsHash = modelInstanceLookups.ModelInstanceFieldsHash;

			if (modelInstanceFieldsHash != null)
				return modelInstanceFieldsHash;

			modelInstanceFieldsHash = new Dictionary<string, FieldViewModel>();
			var modelInstance = ModelInstance;

			var cmsModelInstances = CmsModelInstances.Where(m => m.Model.Id == modelInstance.Model.Id).Select(c => c.Json);
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

						//format the instance field's value to a YYYY MM dd format so that there is one common date format to 
						//compare against when checking for unique field value
						if (field.type == FieldTypeConstants.FieldTypeNames.Datetime)
							valueFinal = CmsInstanceFieldValueHelper.FormatDateString(value, InstanceConstants.DateFormats.YearMonthDay);

						modelInstanceFieldsHash.Add(valueFinal, field);
					}
				}
			}

			modelInstanceLookups.ModelInstanceFieldsHash = modelInstanceFieldsHash;
			ModelInstanceLookupsService.SaveLookupsForModelInstance(modelInstanceLookups);

			return modelInstanceFieldsHash;
		}
	}
}
