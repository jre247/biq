using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Helpers;
using BrightLine.CMS.Services;

namespace BrightLine.CMS.Service
{
	public class DateTimeValidatorService : BaseValidatorService
	{
		private const string FieldType = "DateTime";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public DateTimeValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			DateTime fieldValueAsDateTime = DateTime.MinValue, validationValueAsDateTime = DateTime.MinValue;
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

			boolMessage = ParseFieldValue(ref fieldValueAsDateTime);
			if (!boolMessage.Success)
				return boolMessage;

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not, so parse the validation value to bool
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}
			//if Validation Type is not unique then just simply convert the validation value to a datetime object to then perform some operations on the field value against the validation value
			else
			{
				boolMessage = ParseValidationValueToDateTime(ref validationValueAsDateTime);
				if (!boolMessage.Success)
					return boolMessage;
			}

			var isValid = ValidateForOperation(fieldValueAsDateTime, validationValueAsDateTime, validationValueAsBool);
			if (!isValid)
				return ValidationError;

			return new BoolMessageItem(true, string.Empty);
		}


		private bool ValidateForOperation(DateTime fieldValueAsDateTime, DateTime validationValueAsDateTime, bool validationValueAsBool)
		{
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
			{
				//format the instance field's value to a YYYY MM dd format so that there is one common date format to 
				//compare against when checking for unique field value
				InstanceFieldValue = CmsInstanceFieldValueHelper.FormatDateString(fieldValueAsDateTime, InstanceConstants.DateFormats.YearMonthDay);
				
				isValid = base.ValidateUniqueFieldValue();
			}
			else if (validationTypeId == ValidationTypeMaxDate)
			{
				if (fieldValueAsDateTime > validationValueAsDateTime)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMinDate)
			{
				if (fieldValueAsDateTime < validationValueAsDateTime)
					isValid = false;
			}

			return isValid;
		}

		private BoolMessageItem ParseFieldValue(ref DateTime fieldValueParsed)
		{
			fieldValueParsed = DateTime.MinValue;

			var isFieldValueValid = DateTime.TryParse(InstanceFieldValue, out fieldValueParsed);
			if (!isFieldValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}

		private BoolMessageItem ParseValidationValueToDateTime(ref DateTime validationValueParsed)
		{
			validationValueParsed = DateTime.MinValue;

			var isValidationValueValid = DateTime.TryParse(Validation.Value, out validationValueParsed);
			if (!isValidationValueValid)
				return ValidationParseError;

			return new BoolMessageItem(true, null);
		}



		
	}
}
