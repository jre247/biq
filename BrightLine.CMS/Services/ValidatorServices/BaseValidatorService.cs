using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.CMS.Service
{
	public abstract class BaseValidatorService
	{
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, "Validation failed: field value is not valid Bool");
		protected ModelInstanceLookups ModelInstanceLookups { get;set;}
		protected SettingInstanceLookups SettingInstanceLookups { get;set;}
		public Validation Validation { get; set; }
		public string InstanceFieldValue { get; set; }
		public FieldSaveViewModel InstanceField { get; set; }
		public ModelInstanceSaveViewModel InstanceViewModel { get; set; }
		public CmsField CmsField { get; set; }
		internal InstanceTypeEnum InstanceType { get; set; }
		protected int InstanceId {get;set;}

		public readonly Dictionary<string, int> ValidationTypes;

		//these validation types are convenience variables that children classes can use instead of referencing the more verbose syntax when retrieving a validation type from the lookups config
		public readonly int ValidationTypeRequired, ValidationTypeUnique, ValidationTypeMinFloat, ValidationTypeMaxFloat, ValidationTypeLength, ValidationTypeMinWidth, ValidationTypeMaxWidth, ValidationTypeMinHeight, ValidationTypeMaxHeight, ValidationTypeMinLength, ValidationTypeMaxLength,
			ValidationTypeMaxDate, ValidationTypeMinDate, ValidationTypeMaxVideoSize, ValidationTypeMaxImageSize, ValidationVideoDuration, ValidationTypeHeight, ValidationTypeWidth;

		public BaseValidatorService(ValidatorServiceParams serviceParams)
		{
			this.InstanceId = serviceParams.InstanceId;
			this.InstanceType = serviceParams.InstanceType;
			this.Validation = serviceParams.Validation;
			this.InstanceField = serviceParams.InstanceField;
			this.InstanceFieldValue = serviceParams.InstanceFieldValue;
			this.InstanceViewModel = serviceParams.InstanceViewModel;
			this.CmsField = serviceParams.CmsField;

			ValidationTypes = Lookups.ValidationTypes.HashByName;

			//set up validation type variables that children classes can reference instead of using the more verbose syntax when getting a validation type from lookups config
			ValidationTypeRequired = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.Required];
			ValidationTypeUnique = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.Unique];
			ValidationTypeMinFloat = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MinFloat];
			ValidationTypeMaxFloat = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxFloat];
			ValidationTypeWidth = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.Width];
			ValidationTypeMinWidth = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MinWidth];
			ValidationTypeMaxWidth = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxWidth];
			ValidationTypeHeight = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.Height];
			ValidationTypeMinHeight = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MinHeight];
			ValidationTypeMaxHeight = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxHeight];
			ValidationTypeLength = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.Length];
			ValidationTypeMinLength = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MinLength];
			ValidationTypeMaxLength = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxLength];
			ValidationTypeMaxDate = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxDatetime];
			ValidationTypeMinDate = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MinDatetime];
			ValidationTypeMaxVideoSize = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxVideoSize];
			ValidationTypeMaxImageSize = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxImageSize];
			ValidationVideoDuration = ValidationTypes[ValidationTypeConstants.ValidationTypeNames.MaxVideoDuration];

			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();

			if (InstanceType == InstanceTypeEnum.ModelInstance)
				ModelInstanceLookups = modelInstanceLookupsService.GetLookupsForModelInstance(InstanceId);
			else
				SettingInstanceLookups = settingInstanceLookupsService.GetLookupsForSettingInstance(InstanceId);
		}

		public virtual BoolMessageItem Validate(string InstanceFieldValue)
		{
			return new BoolMessageItem(true, null);
		}

		/// <summary>
		/// Validate to make sure an instance field value is unique among all instances in a specific model. 
		/// </summary>
		/// <param name="fieldValue"></param>
		/// <param name="validationValue"></param>
		/// <param name="field"></param>
		/// <param name="modelInstance"></param>
		/// <returns></returns>
		protected bool ValidateUniqueFieldValue()
		{
			Dictionary<string, FieldViewModel> instanceFieldsDictionary = null;
			if (InstanceType == InstanceTypeEnum.ModelInstance)
				instanceFieldsDictionary = ModelInstanceLookups.GetHashForModelInstanceFieldValues(InstanceId);
			else
				instanceFieldsDictionary = SettingInstanceLookups.GetHashForSettingInstanceFieldValues();

			//see if any item in the instance field dictionary has a field value equal to the current field value
			var fieldValueExists = instanceFieldsDictionary.Any(f => f.Key.ToLower() == InstanceFieldValue.ToLower());

			return !fieldValueExists;
		}

		protected BoolMessageItem ParseValidationValueToBool(ref bool validationValueAsBool)
		{
			validationValueAsBool = true;

			var isValidationValueValid = bool.TryParse(Validation.Value, out validationValueAsBool);
			if (!isValidationValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}

		/// <summary>
		/// This method will check if a field's validations doesn't contain a required validation, and the user input is null/empty then validator service will return true and exit
		/// </summary>
		/// <param name="fieldValue"></param>
		/// <param name="InstanceField"></param>
		/// <returns></returns>
		protected BoolMessageItem ValidateIfFieldValueNotRequired()
		{
			BoolMessageItem boolMessageItem = null;

			//check if field validations doesn't have required 
			var requiredValidationType = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required];
			if (!CmsField.Validations.Any(v => v.ValidationType.Id == requiredValidationType))
			{
				if (string.IsNullOrEmpty(InstanceFieldValue))
					boolMessageItem = new BoolMessageItem(true, null);
			}

			return boolMessageItem;
		}

		protected bool ValidateForNumberOperation(float fieldValueAsInt, float validationValueAsInt)
		{
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
			{
				InstanceFieldValue = fieldValueAsInt.ToString();
				isValid = ValidateUniqueFieldValue();
			}
			else if (validationTypeId == ValidationTypeLength ||
				validationTypeId == ValidationTypeHeight ||
				validationTypeId == ValidationTypeWidth)
			{
				if (fieldValueAsInt != validationValueAsInt)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMinFloat ||
				validationTypeId == ValidationTypeMinWidth ||
				validationTypeId == ValidationTypeMinHeight)
			{
				if (fieldValueAsInt < validationValueAsInt)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxHeight ||
				validationTypeId == ValidationTypeMaxWidth ||
				validationTypeId == ValidationTypeMaxFloat)
			{
				if (fieldValueAsInt > validationValueAsInt)
					isValid = false;
			}

			return isValid;
		}

		


		protected BoolMessageItem ValidateForRequired()
		{
			if (string.IsNullOrEmpty(InstanceFieldValue))
				return new BoolMessageItem(false, "Required field is null.");
			
			return new BoolMessageItem(true, null);

		}

		/// <summary>
		/// Will validate for one of the following validation operations:
		///		1) Length
		///		2) Max Length
		///		3) Min Length
		///		4) Required
		/// </summary>
		/// <param name="validation"></param>
		/// <param name="field"></param>
		/// <param name="fieldValue"></param>
		/// <returns></returns>
		protected bool ValidateForRefOperation()
		{
			var valuesLength = InstanceField.value.Count();
			var validationValueLength = 0;
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
			{
				isValid = ValidateUniqueFieldValue();
			}
			else if (validationTypeId == ValidationTypeMinLength)
			{
				validationValueLength = int.Parse(Validation.Value.ToString());
				if (valuesLength < validationValueLength)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxLength)
			{
				validationValueLength = int.Parse(Validation.Value.ToString());
				if (valuesLength > validationValueLength)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeLength)
			{
				validationValueLength = int.Parse(Validation.Value.ToString());
				if (valuesLength != validationValueLength)
					isValid = false;
			}

			return isValid;
		}
	}

	

}
