using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
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
	public class BoolValidatorService : BaseValidatorService
	{
		private const string FieldType = "Bool";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public BoolValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		/// <summary>
		/// Steps for validating:
		///		1) Check if the Validation Type is equal to "required", and if so check if the field value is null or not
		///			a) If Validation Type is equal to "required" then method exited after this step is completed
		///		2) Try converting the following things below to bool, and if the conversion fails then return from method:
		///			a) Field Value
		///			b) Validation Value
		///		3) Check if Validation Type is equal to "unique", and if so check if the instance value is unique among all instances of this instance's model
		///		3) Return if validation is successful or not
		/// </summary>
		/// <param name="validation"></param>
		/// <param name="fieldValue"></param>
		/// <param name="field"></param>
		/// <param name="viewModel"></param>
		/// <param name="modelInstanceField"></param>
		/// <param name="modelInstance"></param>
		/// <returns></returns>
		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			bool fieldValueAsBool = true, validationValueAsBool = true;
			var validationTypeId = Validation.ValidationType.Id;

			boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			if (validationTypeId == ValidationTypeRequired)
			{
				boolMessage = base.ValidateForRequired();
				return boolMessage;
			}

			boolMessage = ParseFieldValue(ref fieldValueAsBool);
			if (!boolMessage.Success)
				return boolMessage;

			//convert the validation value to a bool object to then perform some operations on the field value against the validation value
			boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
			if (!boolMessage.Success)
				return boolMessage;

			var isValid = ValidateForOperation(validationValueAsBool);
			if (!isValid)
				return ValidationError;

			return new BoolMessageItem(true, string.Empty);
		}

		private bool ValidateForOperation(bool validationValue)
		{
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
				isValid = base.ValidateUniqueFieldValue();

			return isValid;
		}

		private BoolMessageItem ParseFieldValue(ref bool fieldValueParsed)
		{
			fieldValueParsed = true;

			var isFieldValueValid = bool.TryParse(InstanceFieldValue, out fieldValueParsed);
			if (!isFieldValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}
	}
}
