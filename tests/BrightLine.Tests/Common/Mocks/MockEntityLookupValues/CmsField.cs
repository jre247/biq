using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<CmsField> CreateCmsFields()
		{
			var recs = new List<CmsField>()
				{
					new CmsField{
						Id = 1,
						Name = "Test 1",
						CmsModelDefinition = new CmsModelDefinition{Id = 1092}
					}
					
				};

			return recs;
		}

		public static CmsField GetFieldForModelRef(string requiredValidationValue = "false", int cmsRefTypeId = 1, string cmsRefTypeName = "known", int modelDefinitionId = 1, string modelDefinitionName = "choice", string modelDefinitionDisplayName = "Choice")
		{
			var cmsField = MockEntities.GetCmsFieldForType("choices", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel], "refToModel", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", requiredValidationValue, true);
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));
			cmsField.Validations.Add(MockEntities.GetValidationForType(2, "500", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MAX_LENGTH));
			cmsField.CmsRef = MockEntities.GetCmsRef(1, cmsRefTypeId, cmsRefTypeName, modelDefinitionId, modelDefinitionName, modelDefinitionDisplayName);

			return cmsField;
		}

		public static CmsField GetFieldForPageRef(string requiredValidationValue = "true", int cmsRefTypeId = 1, string cmsRefTypeName = "known", int modelDefinitionId = 1, string modelDefinitionName = "test page", string modelDefinitionDisplayName = "Test Page")
		{
			var cmsField = MockEntities.GetCmsFieldForType("pages", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage], "refToPage", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", requiredValidationValue, true);
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));
			cmsField.Validations.Add(MockEntities.GetValidationForType(2, "500", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MAX_LENGTH));
			cmsField.CmsRef = MockEntities.GetCmsRef(1, cmsRefTypeId, cmsRefTypeName, modelDefinitionId, modelDefinitionName, modelDefinitionDisplayName);

			return cmsField;
		}

		public static CmsField GetFieldForRefTypeWithoutRequiredValidator()
		{
			var cmsField = MockEntities.GetCmsFieldForTypeWithoutRequired("choices", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel], FieldTypeConstants.FieldTypeNames.RefToModel, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", true);
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "10", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MAX_LENGTH));
			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			cmsField.CmsRef = MockEntities.GetCmsRef(1, CmsRefTypeknownId, "known", 1, "choice", "Choice");

			return cmsField;
		}

		public static CmsField GetFieldWithRequiredValidatorFalse(int fieldId, string fieldName, string displayName, string description, int fieldTypeId, string fieldTypeName, int validationTypeId, string validationTypeSystemType, string validationTypeName)
		{
			var cmsField = MockEntities.GetCmsFieldForType(fieldName, fieldTypeId, fieldTypeName, fieldId, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "false");
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "1", validationTypeId, validationTypeSystemType, validationTypeName));

			return cmsField;
		}

		public static CmsField GetFieldWithoutRequiredValidator(int fieldId, string fieldName, string displayName, string description, int fieldTypeId, string fieldTypeName, int validationTypeId, string validationTypeSystemType, string validationTypeName, string validationValue = "1")
		{
			var cmsField = MockEntities.GetCmsFieldForTypeWithoutRequired(fieldName, fieldTypeId, fieldTypeName, fieldId, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both");
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, validationValue, validationTypeId, validationTypeSystemType, validationTypeName));

			return cmsField;
		}



		public static string GetFieldValueForRefType(string model, int instanceId)
		{
			return string.Format("{{ \"model\": \"{0}\", \"instanceId\": \"{1}\" }}", model, instanceId); ;
		}

		public static string GetFieldValueForPageRef(int pageId)
		{
			return string.Format("{{ \"pageId\": \"{0}\" }}", pageId);
		}

		public static string GetFieldValueForCommonType(List<string> value)
		{
			var valueAsJsonArray = GetFieldValueAsJasonArray(value);

			return string.Format("{{ \"value\": {0} }}", valueAsJsonArray);
		}

		public static string GetFieldValueAsJasonArray(List<string> value)
		{
			return string.Format("[\"{0}\",\"{1}\"]", value[0], value[1]);
		}

		public static FieldSaveViewModel GetFieldSaveVmForPageRef(int pageId, string name = null)
		{
			var valueJson = GetFieldValueForPageRef(pageId);

			return new FieldSaveViewModel
			{
				id = 1,
				name = name ?? Guid.NewGuid().ToString(), //name needs to be unique when saving model instance published json
				value = new List<string> { valueJson }
			};
		}

		public static FieldSaveViewModel GetFieldSaveVmForResource(string resourceId, string name = null)
		{
			return new FieldSaveViewModel
			{
				id = 1,
				name = name ?? Guid.NewGuid().ToString(), //name needs to be unique when saving model instance published json
				value = new List<string> { resourceId }
			};
		}

		public static FieldViewModel GetFieldViewModelForRefModel(string model, int instanceId, int fieldId = 1, string fieldName = "choices", string fieldDisplayName = "Choices", string fieldDescription = "A list of choices to choose from for the question",
		  bool isList = true, int cmsRefTypeId = 1, string cmsRefTypeName = "known", int modelDefinitionId = 1, string modelDefinitionName = "test page", string modelDefinitionDisplayName = "Test Page")
		{
			var valueVm = new ModelRefFieldValue(model, instanceId);

			var fieldViewModel = new FieldViewModel
			{
				id = fieldId,
				name = fieldName,
				displayName = fieldDisplayName,
				description = fieldDescription,
				list = isList,
				type = "refToModel",
				validations = new Dictionary<string, string>{
					{ValidationTypeNameConstants.REQUIRED,  "true"}
				},
				values = JArray.FromObject(new List<ModelRefFieldValue> { valueVm }),
				refModel = new RefViewModel(MockEntities.GetCmsRef(1, cmsRefTypeId, cmsRefTypeName, modelDefinitionId, modelDefinitionName, modelDefinitionDisplayName))
			};

			return fieldViewModel;
		}

		public static FieldViewModel GetFieldViewModelForRefPage(int pageId, int fieldId = 1, string fieldName = "choices", string fieldDisplayName = "Choices", string fieldDescription = "A list of choices to choose from for the question",
		  bool isList = true, int cmsRefTypeId = 1, string cmsRefTypeName = "known", int modelDefinitionId = 1, string modelDefinitionName = "test page", string modelDefinitionDisplayName = "Test Page")
		{
			var valueVm = new PageRefFieldValue(pageId);

			var fieldViewModel = new FieldViewModel
			{
				id = fieldId,
				name = fieldName,
				displayName = fieldDisplayName,
				description = fieldDescription,
				list = isList,
				type = "refToPage",
				validations = new Dictionary<string, string>{
					{ValidationTypeNameConstants.REQUIRED,  "true"}
				},
				values = JArray.FromObject(new List<PageRefFieldValue> { valueVm }),
				refModel = new RefViewModel(MockEntities.GetCmsRef(1, cmsRefTypeId, cmsRefTypeName, modelDefinitionId, modelDefinitionName, modelDefinitionDisplayName))
			};

			return fieldViewModel;
		}

		public static FieldViewModel GetFieldViewModelForImage(FileType extension, int resourceId = 1, string filename = "test.png", string url = "test/test.png", int fieldId = 1, string fieldName = "choices", string fieldDisplayName = "Choices", string fieldDescription = "A list of choices to choose from for the question",
		  bool isList = true, int cmsRefTypeId = 1, string cmsRefTypeName = "known", int modelDefinitionId = 1, string modelDefinitionName = "test page", string modelDefinitionDisplayName = "Test Page", int campaignId = 1, string resourceName = "test name")
		{
			var resource = new Resource { Id = resourceId, Name = resourceName, Filename = filename, Url = url, Extension = extension, ResourceType = new ResourceType { Id = 1 }, Creative = new Creative { Id = 1, Campaign = new Campaign { Id = campaignId } } };

			var fieldViewModel = new FieldViewModel
			{
				id = fieldId,
				name = fieldName,
				displayName = fieldDisplayName,
				description = fieldDescription,
				list = isList,
				type = FieldTypeConstants.FieldTypeNames.Image,
				validations = new Dictionary<string, string>{
					{ValidationTypeNameConstants.REQUIRED,  "true"}
				},
				values = JArray.FromObject(new List<ResourceViewModel> { new ResourceViewModel(resource) }),
			};

			return fieldViewModel;
		}

		public static FieldSaveViewModel GetFieldSaveVmForRefType(string model, int instanceId, string name = null)
		{
			var valueJson = GetFieldValueForRefType(model, instanceId);

			return new FieldSaveViewModel
			{
				id = 1,
				name =  name ?? Guid.NewGuid().ToString(),
				value = new List<string> { valueJson }
			};
		}

		public static FieldSaveViewModel GetFieldSaveViewModelWithEmptyInput(int id = 1, string name = null, string value = null)
		{
			return new FieldSaveViewModel
			{
				id = id,
				name = name,
				value = null
			};
		}

		public static FieldSaveViewModel GetFieldSaveViewModelWithInput(int id, string value, string name = null)
		{
			return new FieldSaveViewModel
			{
				id = id,
				name = name ?? Guid.NewGuid().ToString(),
				value = new List<string> { value }
			};
		}

		public static FieldViewModel GetStaticFieldViewModel()
		{
			return new FieldViewModel
			{
				id = 1,
				name = "choices",
				displayName = "Choices",
				description = "A list of choices to choose from for the question",
				type = FieldTypeConstants.FieldTypeNames.RefToModel, //TODO: use enum
				list = true,
				refModel = new RefViewModel
				{
					model = "choice",
					type = "known" //TODO: use enum
				},
				validations = new Dictionary<string, string>{
							{ValidationTypeNameConstants.REQUIRED,  "true"}
						},
				values = new JArray { "choice 1" }
			};
		}

		public static FieldViewModel GetFieldViewModel(int fieldId, string fieldName, string fieldType, string fieldValue, string fieldValue2 = null)
		{
			return new FieldViewModel
			{
				id = fieldId,
				name = fieldName,
				displayName = "test",
				description = "test",
				type = fieldType,
				list = false,
				validations = new Dictionary<string, string>{
					{ValidationTypeNameConstants.REQUIRED,  "true"}
				},
				values = new JArray { fieldValue, fieldValue2 }
			};
		}

		public static CmsField GetCmsFieldForNumberType(string fieldName)
		{
			var field1 = MockEntities.GetCmsFieldForType(fieldName, (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			field1.Validations.Add(MockEntities.GetValidationForType(1, "445", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MAX_WIDTH));
			field1.Validations.Add(MockEntities.GetValidationForType(2, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MIN_WIDTH));

			return field1;
		}

		public static CmsField GetCmsFieldForType(string fieldName, int fieldTypeId, string fieldTypeName, int fieldId = 1, int exposeId = 2, string exposeName = "Server", string isRequired = "true", bool list = false, string displayName = null, string description = null)
		{
			return new CmsField
			{
				Id = fieldId,
				Name = fieldName,
				DisplayName = displayName,
				Description = description,
				List = list,
				Type = new FieldType
				{
					Id = fieldTypeId,
					Name = fieldTypeName
				},
				Expose = new Expose
				{
					Id = exposeId,
					Name = exposeName
				},
				Validations = new List<Validation>
				{
					MockEntities.GetValidationInstanceForRequired(isRequired)
				}
			};
		}

		public static CmsField GetCmsFieldForTypeWithoutRequired(string fieldName = "test", int fieldTypeId = 1, string fieldTypeName = FieldTypeConstants.FieldTypeNames.Image, int fieldId = 1, int exposeId = 1, string exposeName = "Server", bool list = false)
		{
			return new CmsField
			{
				Id = fieldId,
				Name = fieldName,
				List = list,
				Type = new FieldType
				{
					Id = fieldTypeId,
					Name = fieldTypeName
				},
				Expose = new Expose
				{
					Id = exposeId,
					Name = exposeName
				},
				Validations = new List<Validation>()
			};
		}

	}

}