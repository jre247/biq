using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class ModelInstanceRetrievalService : IModelInstanceRetrievalService
	{
		public JObject Get(int modelInstanceId)
		{
			JObject modelInstanceJson = null;
			var cmsModelInstancesRepo = IoC.Resolve<ICmsModelInstanceService>();

			var cmsModelInstance = cmsModelInstancesRepo.Get(modelInstanceId);
			if (cmsModelInstance != null)
			{
				var modelInstance = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(cmsModelInstance.Json);
				modelInstanceJson = JObject.FromObject(modelInstance);
			}

			return modelInstanceJson;
		}

		public Dictionary<int, ModelInstanceListViewModel> GetModelInstancesForModel(int modelId, bool verbose = false)
		{
			var modelInstances = new Dictionary<int, ModelInstanceListViewModel>();
			var cmsModelInstancesRepo = IoC.Resolve<ICmsModelInstanceService>();
			var cmsModels = IoC.Resolve<IRepository<CmsModel>>();

			var model = cmsModels.Get(modelId);
			if (model == null)
				return null;

			var cmsModelInstances = cmsModelInstancesRepo.Where(m => m.Model.Id == modelId);
			if (cmsModelInstances != null)
			{
				//get displayField and displayFieldType in addition to raw instance data
				if (verbose)
				{
					modelInstances = BuildModelInstanceDictionaryForVerbose(cmsModelInstances);
				}
				//get raw instance data
				else
				{
					modelInstances = cmsModelInstances.Select(m => new ModelInstanceListViewModel
					{
						id = m.Id,
						display = m.Display,
						name = m.Name
					}).ToList().ToDictionary(c => c.id, c => c);
				}
			}

			return modelInstances;
		}

		#region Private Methods

		/// <summary>
		/// Builds a dictionary of model instance that have added verbose properties, such as: displayField, displayFieldType, and fields
		/// </summary>
		/// <param name="modelInstances"></param>
		/// <param name="cmsModelInstances"></param>
		private Dictionary<int, ModelInstanceListViewModel> BuildModelInstanceDictionaryForVerbose(IQueryable<CmsModelInstance> cmsModelInstances)
		{
			var modelInstances = new Dictionary<int, ModelInstanceListViewModel>();

			var modelInstanceList = cmsModelInstances.Select(m => new
			{
				Id = m.Id,
				Display = m.Display,
				Json = m.Json,
				Name = m.Name,
				DisplayFieldName = m.Model.CmsModelDefinition.DisplayFieldName,
				DisplayFieldType = m.Model.CmsModelDefinition.DisplayFieldType.Name
			}).ToList();

			//add fields to each instance and build up instance dictionary
			foreach (var instance in modelInstanceList)
			{
				var instanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(instance.Json);
				var fields = instanceDeserialized.fields;

				var instanceVm = new ModelInstanceListViewModel
				{
					id = instance.Id,
					display = instance.Display,
					fields = fields,
					name = instance.Name,
					displayField = instance.DisplayFieldName,
					displayFieldType = instance.DisplayFieldType
				};

				modelInstances.Add(instance.Id, instanceVm);
			}

			return modelInstances;
		}

		#endregion
	}
}
