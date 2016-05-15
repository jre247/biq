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
	public class IntValidatorService : BaseValidatorService
	{
		private const string FieldType = "Int";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public IntValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			int fieldValueAsInt = 0, validationValueAsInt = 0;
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

			boolMessage = ParseFieldValue(ref fieldValueAsInt);
			if (!boolMessage.Success)
				return boolMessage;

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not, so parse the validation value to bool
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}
			//if Validation Type is not unique then just simply convert the validation value to a int object to then perform some operations on the field value against the validation value
			else
			{
				boolMessage = ParseValidationValueToint(ref validationValueAsInt);
				if (!boolMessage.Success)
					return boolMessage;
			}


			var isValid = ValidateForOperation(fieldValueAsInt, validationValueAsInt, validationValueAsBool);
			if (!isValid)
				return ValidationError;

			return new BoolMessageItem(true, string.Empty);
		}

		private bool ValidateForOperation(int fieldValueAsInt, int validationValueAsInt, bool validationValueAsBool)
		{
			return base.ValidateForNumberOperation(fieldValueAsInt, validationValueAsInt);
		}

		private BoolMessageItem ParseFieldValue(ref int fieldValueAsInt)
		{
			fieldValueAsInt = 0;

			var isFieldValueValid = int.TryParse(InstanceFieldValue, out fieldValueAsInt);
			if (!isFieldValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}

		private BoolMessageItem ParseValidationValueToint(ref int validationValueAsInt)
		{
			validationValueAsInt = 0;

			var isValidationValueValid = int.TryParse(Validation.Value, out validationValueAsInt);
			if (!isValidationValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}

		

		
	}
}
