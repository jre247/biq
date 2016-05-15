using BrightLine.CMS.Factories;
using BrightLine.CMS.Services.ModelInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BrightLine.CMS.Services
{
	public class ModelInstanceSaveService : BaseModelInstanceService, IModelInstanceSaveService
	{
		private ICmsModelInstanceService CmsModelInstances { get;set;}
		public ModelInstanceSaveService()
		{
			CmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
		}

		public BoolMessageItem Save(ModelInstanceSaveViewModel viewModel)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var cmsModels = IoC.Resolve<IRepository<CmsModel>>();
			var modelInstanceValidationService = IoC.Resolve<IModelInstanceValidationService>();

			var model = cmsModels.Get(viewModel.modelId);

			modelBoolMessage = modelInstanceValidationService.ValidateModelInstanceAccessibility(model);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			var modelInstance = CmsModelInstances.Get(viewModel.id);
			if (modelInstance == null)
				modelInstance = CreateModelInstance(model, modelInstance);

			SetupLookupsForModelInstance(viewModel, modelInstance);

			modelBoolMessage = modelInstanceValidationService.ValidateModelInstanceFields(modelInstance, viewModel);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			modelBoolMessage = SaveModelInstance(viewModel, modelInstance);
			modelBoolMessage.EntityId = modelInstance.Id;

			return modelBoolMessage;
		}


		/// <summary>
		/// This method will do the following:
		///		1) Save Model Instance fields
		///		2) construct a json representation of the model instance and save it in the Json field
		///		3) construct a json representation of the model instance to be used for publishing (Publishing happens later on when the user hits the publish button)
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		/// <returns></returns>
		public BoolMessageItem SaveModelInstance(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var modelInstancePublishedJsonService = IoC.Resolve<IModelInstancePublishedJsonService>();

			modelInstance.Name = viewModel.name;

			modelBoolMessage = SaveFields(viewModel, modelInstance);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			SaveModelInstanceJson(viewModel, modelInstance);

			modelInstancePublishedJsonService.SaveModelInstancePublishedJson(viewModel, modelInstance);

			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			return modelBoolMessage;
		}

		public BoolMessageItem SaveFields(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var modelInstanceSaveServerPropertiesService = IoC.Resolve<IModelInstanceSaveServerPropertiesService>();
			var exposeClient = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client];
			var exposeBoth = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both];
			var exposeServer = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server];

			var modelInstanceLookups = GetLookupsForModelInstance(modelInstance);

			foreach (var field in viewModel.fields)
			{
				var modelInstanceField = modelInstanceLookups.ModelInstanceFieldsDictionary[field.id];

				if (modelInstanceField.Expose == null)
					return new BoolMessageItem(false, string.Format("field with name '{0}' does not have expose property set", field.name));

				var fieldExpose = modelInstanceField.Expose.Id;

				//persist model instance server properties only if field's expose property is equal to "server" or "both"
				if (fieldExpose == exposeServer || fieldExpose == exposeBoth)
					modelBoolMessage = modelInstanceSaveServerPropertiesService.SaveField(modelInstanceField, modelInstance, field.value);

				if (!modelBoolMessage.Success)
					break;
			}
			return modelBoolMessage;
		}

		


		#region Private Methods

		/// <summary>
		/// Build all of the lookups that the model instance will need to reference. 
		/// This Lookups container includes such things as a dictionary of CmsFields that belong to this model instance, a dictionary of resources for this model instance, etc.
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		private void SetupLookupsForModelInstance(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance)
		{
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();

			modelInstanceLookupsService.CreateModelInstanceLookups(modelInstance);

			var modelInstanceLookups = GetLookupsForModelInstance(modelInstance);
			modelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, modelInstance.Id);
			modelInstanceLookups.BuildResourcesDictionary(viewModel, modelInstance.Id);
		}

		/// <summary>
		/// Constructs a json representation of the model instance and saves it on the Json field of the Model Instance
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		private void SaveModelInstanceJson(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance)
		{
			var modelInstanceJsonVm = new ModelInstanceJsonViewModel
			{
				id = modelInstance.Id,
				name = modelInstance.Name,
				fields = new List<FieldViewModel>()
			};

			var modelInstanceLookups = GetLookupsForModelInstance(modelInstance);

			foreach (var field in viewModel.fields)
			{
				var modelInstanceField = modelInstanceLookups.ModelInstanceFieldsDictionary[field.id];

				var fieldViewModel = new FieldViewModel(modelInstanceField, field.value, modelInstanceLookups.FieldResourcesDictionary, modelInstance.Model.Feature.Campaign.Id);
				modelInstanceJsonVm.fields.Add(fieldViewModel);
			}

			modelInstance.Json = JsonConvert.SerializeObject(modelInstanceJsonVm);
		}

		private CmsModelInstance CreateModelInstance(CmsModel model, CmsModelInstance modelInstance)
		{
			modelInstance = new CmsModelInstance
			{
				Model = model
			};

			CmsModelInstances.Create(modelInstance);
			return modelInstance;
		}

		#endregion

	}
}
