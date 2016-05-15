using BrightLine.CMS.Factories;
using BrightLine.CMS.Services.SettingInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class SettingInstanceValidationService : BaseSettingInstanceService, ISettingInstanceValidationService
	{
		public SettingInstanceValidationService()
		{ }


		public BoolMessageItem ValidateSettingInstanceFields(CmsSettingInstance settingInstance, ModelInstanceSaveViewModel viewModel)
		{
			var settingInstanceLookups = GetSettingInstanceLookups(settingInstance);

			var modelBoolMessage = new BoolMessageItem(true, null);

			foreach (var field in viewModel.fields)
			{
				var settingInstanceField = settingInstanceLookups.SettingInstanceFieldsDictionary[field.id];

				if (settingInstanceField == null)
				{
					modelBoolMessage = new BoolMessageItem(false, "Setting Instance does not contain the following field: " + field.name);
					break;
				}

				modelBoolMessage = ValidateField(viewModel, field, settingInstanceField, settingInstance);

				if (!modelBoolMessage.Success)
					break;
			}
			return modelBoolMessage;
		}

		public BoolMessageItem ValidateField(ModelInstanceSaveViewModel viewModel, FieldSaveViewModel field, CmsField settingInstanceField, CmsSettingInstance settingInstance)
		{
			BoolMessageItem modelBoolMessage = new BoolMessageItem(true, null);
			var validationTypeRequired = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required];
			var isFieldRequired = settingInstanceField.Validations.Any(f => f.ValidationType.Id == validationTypeRequired && f.Value == InstanceConstants.VALIDATION_TYPE_REQUIRED_TRUE);
			var settingInstanceType = InstanceTypeEnum.SettingInstance;;

			if (!isFieldRequired && field.value == null)
				return modelBoolMessage;

			//a field will have one or more validations to validate for, so loop through them all and validate each
			foreach (var validation in settingInstanceField.Validations)
			{
				BoolMessageItem fieldBoolMessage = null;

				var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = viewModel, CmsField = settingInstanceField, InstanceField = field, InstanceId = settingInstance.Setting_Id, InstanceType = settingInstanceType };
				var validatorService = ValidatorServiceFactory.GetValidatorService(validatorServiceParams);

				foreach (var fieldValue in field.value)
				{
					fieldBoolMessage = validatorService.Validate(fieldValue);
					if (!fieldBoolMessage.Success)
						break;
				}

				if (!fieldBoolMessage.Success)
				{
					modelBoolMessage = fieldBoolMessage;
					break;
				}
			}
			return modelBoolMessage;
		}

		public BoolMessageItem ValidateSettingInstanceAccessibility(CmsSetting setting)
		{
			var boolMessage = new BoolMessageItem(true, null);
			var campaigns = IoC.Resolve<ICampaignService>();
			var creatives = IoC.Resolve<ICreativeService>();

			if (setting == null)
				return new BoolMessageItem(false, "Setting Instance does not exist on setting");

			var campaignId = setting.Feature.Campaign.Id;
			var creativeId = setting.Feature.Creative.Id;

			var campaign = campaigns.Get(campaignId);
			if (!campaigns.IsAccessible(campaign))
				return new BoolMessageItem(false, "Campaign is not accessible");

			var creative = creatives.Get(creativeId);

			if (creative == null)
				return new BoolMessageItem(false, "Setting Instance does not exist on creative");

			return boolMessage;
		}

	}
}
