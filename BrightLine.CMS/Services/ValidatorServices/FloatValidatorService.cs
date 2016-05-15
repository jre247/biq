using BrightLine.CMS.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Service
{
	public class FloatValidatorService : BaseValidatorService
	{
		private const string FieldType = "Float";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public FloatValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			float fieldValueAsFloat = 0, validationValueAsFloat = 0;
			bool validationValueAsBool = true;
			var validationTypeId = Validation.ValidationType.Id;

			boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			if (validationTypeId == ValidationTypeRequired)
			{
				boolMessage = base.ValidateForRequired();
				return boolMessage;
			}

			boolMessage = ParseFieldValue(ref fieldValueAsFloat);
			if (!boolMessage.Success)
				return boolMessage;

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not, so parse the validation value to bool
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}
			//if Validation Type is not unique then just simply convert the validation value to a float object to then perform some operations on the field value against the validation value
			else
			{
				boolMessage = ParseValidationValueToFloat(ref validationValueAsFloat);
				if (!boolMessage.Success)
					return boolMessage;
			}


			var isValid = ValidateForOperation(fieldValueAsFloat, validationValueAsFloat, validationValueAsBool);
			if (!isValid)
				return ValidationError;

			return new BoolMessageItem(true, string.Empty);
		}

		private bool ValidateForOperation(float fieldValueAsInt, float validationValueAsFloat, bool validationValueAsBool)
		{
			return base.ValidateForNumberOperation(fieldValueAsInt, validationValueAsFloat);
		}

		private BoolMessageItem ParseFieldValue(ref float fieldValueAsFloat)
		{
			fieldValueAsFloat = 0;

			var isFieldValueValid = float.TryParse(InstanceFieldValue, out fieldValueAsFloat);
			if (!isFieldValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}

		private BoolMessageItem ParseValidationValueToFloat(ref float validationValueAsFloat)
		{
			validationValueAsFloat = 0;

			var isValidationValueValid = float.TryParse(Validation.Value, out validationValueAsFloat);
			if (!isValidationValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}
	}
}
