using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.ValidationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class BlueprintImportValidationService : IBlueprintImportValidationService
	{	
		public void CreateValidations(BlueprintImportModelField field, CmsField cmsField)
		{
			if (field.validation == null)
				return;

			var validationType = field.validation.GetType();
			PropertyInfo[] validationProperties = validationType.GetProperties();

			foreach (PropertyInfo validationPropertyInfo in validationProperties)
			{
				if (!validationPropertyInfo.CanRead)
					continue;

				//skip creating a Validation record for Extension here, since there is a more involved way to import Extensions
				if (validationPropertyInfo.Name.ToLower() == BlueprintImportConstants.ValidationTypes.Extension)
					continue;

				var validationValue = validationPropertyInfo.GetValue(field.validation, null);

				if (validationValue == null)
					continue;

				var validationFieldName = validationPropertyInfo.Name;
				var validationValueAsString = validationValue.ToString();

				if (!string.IsNullOrEmpty(validationValueAsString))
					CreateValidationRecordForValidationType(validationValueAsString, cmsField, validationFieldName);
			}

			CreateExtensions(field, cmsField);

		}

		private void CreateValidationRecordForValidationType(string validationValue, CmsField cmsField, string validationTypeName)
		{
			var validations = IoC.Resolve<IValidationService>();
			var validationTypeId = Lookups.ValidationTypes.HashByName[validationTypeName];

			var validation = new Validation
			{
				Value = validationValue,
				CmsField = cmsField,
				ValidationType_Id = validationTypeId
			};
			validations.Create(validation, true);
		}

		private void CreateExtensions(BlueprintImportModelField field, CmsField cmsField)
		{
			if (field.validation == null || field.validation.extension == null)
				return;

			var extensions = field.validation.extension;

			var validations = IoC.Resolve<IValidationService>();
			var extensionValidationTypeId = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Extension];

			var validation = new Validation
			{
				ValidationType_Id = extensionValidationTypeId,
				CmsField = cmsField,
				Value = "Value will be null for Extension Validation."
			};
			validation = validations.Create(validation, true);

			foreach (var extension in extensions)
			{
				if (string.Equals(extension, FileTypeConstants.FileTypeNames.Jpeg, StringComparison.InvariantCultureIgnoreCase))
					CreateFileTypeValidation(validation, FileTypeConstants.FileTypeNames.Jpeg);
				if (string.Equals(extension, FileTypeConstants.FileTypeNames.Jpg, StringComparison.InvariantCultureIgnoreCase))
					CreateFileTypeValidation(validation, FileTypeConstants.FileTypeNames.Jpg);
				if (string.Equals(extension, FileTypeConstants.FileTypeNames.Png, StringComparison.InvariantCultureIgnoreCase))
					CreateFileTypeValidation(validation, FileTypeConstants.FileTypeNames.Png);
				if (string.Equals(extension, FileTypeConstants.FileTypeNames.Gif, StringComparison.InvariantCultureIgnoreCase))
					CreateFileTypeValidation(validation, FileTypeConstants.FileTypeNames.Gif);
				if (string.Equals(extension, FileTypeConstants.FileTypeNames.Mp4, StringComparison.InvariantCultureIgnoreCase))
					CreateFileTypeValidation(validation, FileTypeConstants.FileTypeNames.Mp4);
			}
		}

		private void CreateFileTypeValidation(Validation validation, string fileTypeName)
		{
			var fileTypeValidations = IoC.Resolve<IFileTypeValidationService>();

			var fileTypeId = Lookups.FileTypes.HashByName[fileTypeName];

			var fileTypeValidation = new FileTypeValidation
			{
				FileType_Id = fileTypeId,
				Validation = validation
			};
			fileTypeValidations.Create(fileTypeValidation, true);
		}
	}
}
