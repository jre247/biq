using BrightLine.CMS.Services.Publish;
using BrightLine.CMS.Services.SettingInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class SettingInstanceSaveService : BaseSettingInstanceService, ISettingInstanceSaveService
	{
		private IRepository<CmsSettingInstance> CmsSettingInstances { get;set;}

		public SettingInstanceSaveService()
		{ 
			CmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
		}

		public BoolMessageItem Save(ModelInstanceSaveViewModel viewModel)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();
			var settingInstanceValidationService = IoC.Resolve<ISettingInstanceValidationService>();

			var setting = cmsSettings.Get(viewModel.settingId);
			if(setting == null)
				throw new NullReferenceException("CmsSetting record does not exist for Id.");

			modelBoolMessage = settingInstanceValidationService.ValidateSettingInstanceAccessibility(setting);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			var settingInstance = CmsSettingInstances.Get(viewModel.id);
			if (settingInstance == null)
				settingInstance = CreateSettingInstance(settingInstance, setting);

			if (settingInstance.IsDeleted == true)
				return new BoolMessageItem(false, "This Model Instance has been marked for delete.");

			SetupLookupsForSettingInstance(viewModel, settingInstance);

			modelBoolMessage = settingInstanceValidationService.ValidateSettingInstanceFields(settingInstance, viewModel);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			modelBoolMessage = SaveSettingInstance(viewModel, settingInstance);
			modelBoolMessage.EntityId = settingInstance.Id;

			return modelBoolMessage;
		}

		/// <summary>
		/// This method will do the following:
		///		1) Save Setting Instance fields
		///		2) construct a json representation of the setting instance and save it in the Json field
		///		3) construct a json representation of the setting instance to be used for publishing (Publishing happens later on when the user hits the publish button)
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		/// <returns></returns>
		public BoolMessageItem SaveSettingInstance(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var settingInstancePublishedJsonService = IoC.Resolve<ISettingInstancePublishedJsonService>();

			settingInstance.Name = viewModel.name;

			modelBoolMessage = SaveFields(viewModel, settingInstance);
			if (!modelBoolMessage.Success)
				return modelBoolMessage;

			SaveSettingInstanceJson(viewModel, settingInstance);

			settingInstancePublishedJsonService.SaveSettingInstancePublishedJson(viewModel, settingInstance);

			return modelBoolMessage;
		}

		public BoolMessageItem SaveFields(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance)
		{
			var modelBoolMessage = new BoolMessageItem(true, null);
			var exposeClient = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client];
			var exposeBoth = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both];

			foreach (var field in viewModel.fields)
			{
				var settingInstanceLookups = GetSettingInstanceLookups(settingInstance);
				var settingInstanceField = settingInstanceLookups.SettingInstanceFieldsDictionary[field.id];

				if (settingInstanceField.Expose == null)
					return new BoolMessageItem(false, string.Format("field with name '{0}' does not have expose property set", field.name));

				var fieldExpose = settingInstanceField.Expose.Id;

				//add field to model instance json
				var fieldViewModel = new FieldViewModel(settingInstanceField, field.value, settingInstanceLookups.FieldResourcesDictionary, settingInstanceLookups.Setting.Feature.Creative.Campaign.Id);

				if (!modelBoolMessage.Success)
					break;
			}
			return modelBoolMessage;
		}

		#region Private Methods

		/// <summary>
		/// Build all of the lookups that the setting instance will need to reference. 
		/// This Lookups container includes such things as a dictionary of CmsFields that belong to this model instance, a dictionary of resources for this setting instance, etc.
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="settingInstance"></param>
		private void SetupLookupsForSettingInstance(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance)
		{
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();
			settingInstanceLookupsService.CreateSettingInstanceLookups(settingInstance);

			var settingInstanceLookups = GetSettingInstanceLookups(settingInstance);
			settingInstanceLookups.BuildInstanceFieldsDictionary(viewModel, settingInstance);
			settingInstanceLookups.BuildResourcesDictionary(viewModel);
		}


		private CmsSettingInstance CreateSettingInstance(CmsSettingInstance settingInstance, CmsSetting setting)
		{
			settingInstance = new CmsSettingInstance
			{
				Setting_Id = setting.Id,
				Name = setting.Name
			};

			CmsSettingInstances.Insert(settingInstance);
			return settingInstance;
		}

		/// <summary>
		/// Constructs a json representation of the Setting Instance and saves it on the Json field of the Setting Instance
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="modelInstance"></param>
		private void SaveSettingInstanceJson(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance)
		{
			var settingInstanceLookups = GetSettingInstanceLookups(settingInstance);

			var campaignId = settingInstanceLookups.Setting.Feature.Creative.Campaign.Id;
			var settingInstanceJsonVm = new ModelInstanceJsonViewModel
			{
				id = settingInstance.Id,
				name = settingInstance.Name,
				fields = new List<FieldViewModel>()
			};

			foreach (var field in viewModel.fields)
			{
				var settingInstanceField = settingInstanceLookups.SettingInstanceFieldsDictionary[field.id];

				var fieldViewModel = new FieldViewModel(settingInstanceField, field.value, settingInstanceLookups.FieldResourcesDictionary, campaignId);
				settingInstanceJsonVm.fields.Add(fieldViewModel);
			}

			settingInstance.Json = JsonConvert.SerializeObject(settingInstanceJsonVm);
		}

		#endregion
	}
}
