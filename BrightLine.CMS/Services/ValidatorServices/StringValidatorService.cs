using BrightLine.CMS.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
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
	public class StringValidatorService : BaseValidatorService
	{
		private const string FieldType = "String";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public StringValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			int validationValueAsInt = 0;
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

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not, so parse the validation value to bool
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}
			//parse validation value to int because validation value will represent either length, max length, or min length
			else
			{
				boolMessage = ParseValidationValue(out validationValueAsInt);
				if (!boolMessage.Success)
					return boolMessage;
			}

			var isValid = ValidateForOperation(validationValueAsInt, validationValueAsBool);
			if (!isValid)
				return ValidationError;

			return new BoolMessageItem(true, string.Empty);
		}

		private bool ValidateForOperation(int validationValueAsInt, bool validationValueAsBool)
		{
			var isValid = true;
			var fieldValueLength = InstanceFieldValue.Count();
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
			{
				isValid = base.ValidateUniqueFieldValue();
			}
			else if (validationTypeId == ValidationTypeLength)
			{
				if (fieldValueLength != validationValueAsInt)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMinLength)
			{
				if (fieldValueLength < validationValueAsInt)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxLength)
			{
				if (fieldValueLength > validationValueAsInt)
					isValid = false;
			}

			return isValid;
		}

		private BoolMessageItem ParseValidationValue(out int validationValueAsInt)
		{
			validationValueAsInt = 0;

			var isValidationValueValid = int.TryParse(Validation.Value, out validationValueAsInt);
			if (!isValidationValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}
	}
}
