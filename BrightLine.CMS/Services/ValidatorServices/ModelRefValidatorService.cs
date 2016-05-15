using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Service
{
	public class ModelRefValidatorService : BaseValidatorService
	{
		private const string MODEL_INSTANCE_INCORRECT_FORMAT = "Field with type 'ref' and with id {0} has one or more model instance values that are not in the correct format";
		private const string MODEL_INSTANCE_INVALID_FOR_MODEL = "Field with type 'ref' and id {0} references a model instance with id {1} that don't exist for model with name {2}";
		private const string MODEL_INSTANCE_DOES_NOT_EXIST = "Field with type 'ref' and with id {0} references a model instance with id {1} that don't exist";
		private const string MODEL_INSTANCE_INVALID_FOR_CREATIVE = "Field with type 'ref' and  with id {0} references a model instance with id {1} that don't exist on the same creative as the current instance with id {2}";
		private const string MODEL_INSTANCE_PAGE_ID_INVALID = "Page Id does not exist for field.";

		public ModelRefValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		/// <summary>
		/// process for validating ref type:
		///		1) Model Reference:
		///			a) validate instanceId and model (these two properties exist in the field's value)
		///			 *If field cms ref type is known, then validate each field value with the following logic:
		///				*the instance id exists for that model 
		///				*note: the value for a ref field consists of: model, instanceId
		///				*we need to know that that instanceId actually references the correct model
		///			 *If field cms ref type is unknown, then validate each field value with the following logic:
		///				*both the current instance and the instance being referenced link to the same creative
		///			b) validate against specific validation type that's passed in (i.e. required, int, datetime, etc.)
		/// </summary>
		/// <param name="validation"></param>
		/// <param name="fieldValue"></param>
		/// <param name="field"></param>
		/// <param name="viewModel"></param>
		/// <param name="_cmsModelInstanceRepo"></param>
		/// <param name="modelInstanceField"></param>
		/// <param name="modelInstance"></param>
		/// <returns></returns>
		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			var validationTypeId = Validation.ValidationType.Id;
			var boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			boolMessage = new BoolMessageItem(true, null);
			bool validationValueAsBool = true;

			//convert field value from json string to strongly typed obj
			var fieldValueScrubbed = InstanceFieldValue.Replace("\r\n", "").Replace("\t", ""); //remove tabs and new lines from value string in order to make correct json format

			if (CmsField.Type.Id != Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel])
				return new BoolMessageItem(false, "Field does not have Type of Model Ref.");

			boolMessage = ValidateModelReference(ref validationValueAsBool, fieldValueScrubbed);
			
			return boolMessage;
		}

		private BoolMessageItem ValidateModelReference(ref bool validationValueAsBool, string fieldValueScrubbed)
		{
			var validationTypeId = Validation.ValidationType.Id;
			var boolMessage = BoolMessageItem.GetSuccessMessage();

			if (validationTypeId == ValidationTypeRequired)
			{
				boolMessage = base.ValidateForRequired();
				return boolMessage;
			}

			var refValue = JsonConvert.DeserializeObject<ModelRefFieldValue>(fieldValueScrubbed);
			if (refValue == null)
				return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INCORRECT_FORMAT, InstanceField.id));

			boolMessage = ValidateForModelReferenceType(refValue);
			if (!boolMessage.Success)
				return boolMessage;

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not, so parse the validation value to bool
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}

			var isValid = base.ValidateForRefOperation();
			if (!isValid)
				return new BoolMessageItem(false, "validation failed for field of type 'ref'");

			return boolMessage;
		}

		/// <summary>
		/// Will validate for one of the following reference types:
		///		1) Unknown:
		///			*this means the model is not in the same feature, but is in the same creative
		///			*also note: model must reference correct model
		///		2) Known:
		///			*this means model is in the same feature
		///			*makes sure that referenced model name and instance id match for an existing instance
		/// </summary>
		/// <param name="field"></param>
		/// <param name="viewModel"></param>
		/// <param name="_cmsModelInstanceRepo"></param>
		/// <param name="modelInstanceField"></param>
		/// <param name="modelInstance"></param>
		/// <param name="refValue"></param>
		/// <returns></returns>
		private BoolMessageItem ValidateForModelReferenceType(ModelRefFieldValue refValue)
		{
			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var boolMessage = new BoolMessageItem(true, null);
			var modelInstanceId = base.ModelInstanceLookups.ModelInstance.Id;
			var creativeId = base.ModelInstanceLookups.ModelInstance.Model.Feature.Creative.Id;

			//validate for unknown reference instance
			var CmsRefTypeUnknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Unknown];
			if (CmsField.CmsRef.CmsRefType.Id == CmsRefTypeUnknownId)
			{
				var modelInstances = cmsModelInstances.Where(f => f.Id == refValue.instanceId && f.Id != modelInstanceId);
				if (modelInstances == null)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_DOES_NOT_EXIST, InstanceField.id, refValue.instanceId));

				var modelInstanceCompare = modelInstances.SingleOrDefault();
				if (modelInstanceCompare == null)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_DOES_NOT_EXIST, InstanceField.id, refValue.instanceId));

				if (modelInstanceCompare == null)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_DOES_NOT_EXIST, InstanceField.id, refValue.instanceId, InstanceViewModel.id));

				var creativeIdCompare = modelInstanceCompare.Model.Feature.Creative.Id;

				//validate the model instance's referenced model and also the original model instance both reference the same creative
				if (creativeId != creativeIdCompare)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INVALID_FOR_CREATIVE, InstanceField.id, refValue.instanceId, InstanceViewModel.id));

				boolMessage = ValidateForModelDefinitionReference(refValue, modelInstanceCompare);
					
			}
			//validate for known reference instance
			else
			{
				var modelInstances = cmsModelInstances.Where(f => f.Id == refValue.instanceId && f.Model.Name == refValue.model && f.Id != modelInstanceId);
				if (modelInstances == null)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INVALID_FOR_MODEL, InstanceField.id, refValue.instanceId, refValue.model));

				var modelInstanceRefence = modelInstances.SingleOrDefault();
				if (modelInstanceRefence == null)
					return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INVALID_FOR_MODEL, InstanceField.id, refValue.instanceId, refValue.model));

				boolMessage = ValidateForModelDefinitionReference(refValue, modelInstanceRefence);
			}

			return boolMessage;
		}

		/// <summary>
		/// Validate that both the original instance and the referenced instance reference the same model definition
		/// </summary>
		/// <param name="field"></param>
		/// <param name="modelInstanceField"></param>
		/// <param name="refValue"></param>
		/// <param name="modelInstanceCompare"></param>
		/// <returns></returns>
		private BoolMessageItem ValidateForModelDefinitionReference(ModelRefFieldValue refValue, CmsModelInstance modelInstanceCompare)
		{
			var boolMessage = new BoolMessageItem(true, null);

			var modelDefinitionId = CmsField.CmsRef.CmsModelDefinition.Id;
			var modelDefinitionIdCompare = modelInstanceCompare.Model.CmsModelDefinition.Id;
			if (modelDefinitionId != modelDefinitionIdCompare)
				boolMessage = new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INVALID_FOR_MODEL, InstanceField.id, refValue.instanceId, refValue.model));

			return boolMessage;
		}

		
	}
}
