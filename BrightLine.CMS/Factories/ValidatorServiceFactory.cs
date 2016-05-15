using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Factories
{
	public static class ValidatorServiceFactory
	{
		public static BaseValidatorService GetValidatorService(ValidatorServiceParams serviceParams)
		{
			BaseValidatorService validatorService = null;
			var fieldTypeId = serviceParams.CmsField.Type.Id;
			var FieldTypesIdHash = Lookups.FieldTypes.HashByName;
			var ValidationTypesIdHash = Lookups.ValidationTypes.HashByName;
			Validation validation = serviceParams.Validation;

			//Extension validator service
			if (validation != null && validation.ValidationType.Id == (int)ValidationTypesIdHash[ValidationTypeConstants.ValidationTypeNames.Extension])
			{
				validatorService = new ExtensionValidatorService(serviceParams);
			}
			//Image validator service
			else if (fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Image])
			{
				validatorService = new ImageValidatorService(serviceParams);
			}
			//Model Ref validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.RefToModel]){
				validatorService = new ModelRefValidatorService(serviceParams);
			}
			//String validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.String]){
				validatorService = new StringValidatorService(serviceParams);
			}
			//Int validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Integer]){
				validatorService = new IntValidatorService(serviceParams);
			}
			//Float validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Float]){
				validatorService = new FloatValidatorService(serviceParams);
			}
			//Bool validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Bool]){
				validatorService = new BoolValidatorService(serviceParams);
			}
			//DateTime validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Datetime]){
				validatorService = new DateTimeValidatorService(serviceParams);
			}
			//Video validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.Video]){
				validatorService = new VideoValidatorService(serviceParams);
			}
			//Page Ref validator service
			else if(fieldTypeId == FieldTypesIdHash[FieldTypeConstants.FieldTypeNames.RefToPage]){
				validatorService = new PageRefValidatorService(serviceParams);
			}
			

			return validatorService;
		}

		
	}

	


	
}
