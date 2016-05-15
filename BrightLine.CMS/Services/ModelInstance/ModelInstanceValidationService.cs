using BrightLine.CMS.Factories;
using BrightLine.CMS.Services.ModelInstance;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Common.ViewModels.Models;
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
	public class ModelInstanceValidationService : BaseModelInstanceService, IModelInstanceValidationService
	{
		public ModelInstanceValidationService()
		{ }

		public BoolMessageItem ValidateModelInstanceAccessibility(CmsModel model)
		{
			var campaigns = IoC.Resolve<ICampaignService>();
			var creatives = IoC.Resolve<ICreativeService>();

			var boolMessage = new BoolMessageItem(true, null);
			if (model == null)
				return new BoolMessageItem(false, "Model Instance does not exist on model");

			var campaignId = model.Feature.Campaign.Id;
			var campaign = campaigns.Get(campaignId);
			if (!campaigns.IsAccessible(campaign))
				return new BoolMessageItem(false, "Campaign is not accessible");

			var creativeId = model.Feature.Creative.Id;
			var creative = creatives.Get(creativeId);
			if (creative == null)
				return new BoolMessageItem(false, "Model Instance does not exist on creative");

			return boolMessage;
		}

		public BoolMessageItem ValidateModelInstanceFields(CmsModelInstance modelInstance, ModelInstanceSaveViewModel viewModel)
		{
			var modelInstanceLookups = GetLookupsForModelInstance(modelInstance);

			var boolMessage = new BoolMessageItem(true, null);

			foreach (var field in viewModel.fields)
			{
				var modelInstanceField = modelInstanceLookups.ModelInstanceFieldsDictionary[field.id];

				if (modelInstanceField == null)
				{
					boolMessage = new BoolMessageItem(false, "Model Instance does not contain the following field: " + field.name);
					break;
				}

				boolMessage = ValidateField(viewModel, field, modelInstanceField, modelInstance);

				if (!boolMessage.Success)
					break;
			}
			return boolMessage;
		}

		protected BoolMessageItem ValidateField(ModelInstanceSaveViewModel viewModel, FieldSaveViewModel field, CmsField modelInstanceField, CmsModelInstance modelInstance)
		{
			var boolMessage = new BoolMessageItem(true, null);
			var validationTypeRequired = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required];
			var isFieldRequired = modelInstanceField.Validations.Any(f => f.ValidationType.Id == validationTypeRequired && f.Value == InstanceConstants.VALIDATION_TYPE_REQUIRED_TRUE);
			var instanceType = InstanceTypeEnum.ModelInstance;

			if (!isFieldRequired && field.value == null)
				return boolMessage;

			//a field will have one or more validations to validate for, so loop through them all and validate each
			foreach (var validation in modelInstanceField.Validations)
			{
				BoolMessageItem fieldBoolMessage = null;

				var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = viewModel, CmsField = modelInstanceField, InstanceField = field, InstanceId = modelInstance.Id, InstanceType = instanceType };
				var validatorService = ValidatorServiceFactory.GetValidatorService(validatorServiceParams);

				foreach (var fieldValue in field.value)
				{
					
					fieldBoolMessage = validatorService.Validate(fieldValue);
					if (!fieldBoolMessage.Success)
						break;
				}

				if (!fieldBoolMessage.Success)
				{
					boolMessage = fieldBoolMessage;
					break;
				}
			}
			return boolMessage;
		}

		

	}
}
