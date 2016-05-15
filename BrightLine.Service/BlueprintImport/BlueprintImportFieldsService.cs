using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class BlueprintImportFieldsService : IBlueprintImportFieldsService
	{
		private Dictionary<string, BlueprintImportModelDefinition> ModelDefinitionDictionary { get; set; } 

		public void CreateFields(BlueprintImportModel model, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary, CmsModelDefinition modelDefinition = null,CmsSettingDefinition settingDefinition = null)
		{
			ModelDefinitionDictionary = modelDefinitionDictionary;
			var isModelDefinition = modelDefinition != null;
			var isSettingDefinition = settingDefinition != null;
			var cmsFieldsDictionary = new Dictionary<string, CmsField>();
			var rawFieldsDictionary = new Dictionary<string, BlueprintImportModelField>();


			foreach (var field in model.fields)
				CreateField(modelDefinition, settingDefinition, isModelDefinition, isSettingDefinition, cmsFieldsDictionary, rawFieldsDictionary, field);

			//add fields dictionaries to model definition container to reference later
			if (isModelDefinition)
			{
				var modelDefinitionContainer = modelDefinitionDictionary[modelDefinition.Name.ToLower()];
				modelDefinitionContainer.CmsFieldsDictionary = cmsFieldsDictionary;
				modelDefinitionContainer.RawFieldsDictionary = rawFieldsDictionary;
			}
		}

		private void CreateField(CmsModelDefinition modelDefinition, CmsSettingDefinition settingDefinition, bool isModelDefinition, bool isSettingDefinition, Dictionary<string, CmsField> cmsFieldsDictionary, Dictionary<string, BlueprintImportModelField> rawFieldsDictionary, BlueprintImportModelField field)
		{
			var cmsField = new CmsField();
			int expose = -1;

			//Don't retrieve FieldType for ref here because refToModel or refToPage field types will be set for this field later in blueprint import execution
			//	*note: you can reference the following places to see when FieldType is set for refToModel and refToPage: 
				//	1) CreateModelReferences method in BlueprintImportModelService or 
				//	2) CreatePageReferences method in BlueprintImportPagesService
			if (field.type != FieldTypeConstants.FieldTypeNames.Ref)
				cmsField.Type_Id = Lookups.FieldTypes.HashByName[field.type];
				
			if (string.IsNullOrEmpty(field.expose))
				expose = Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client];
			else
			{
				var exposeNameFormatted = BlueprintImportHelper.ConvertStringToFirstLetterUppercase(field.expose);
				expose = Lookups.Exposes.HashByName[exposeNameFormatted];	
			}
					

			cmsField.Name = field.name;
			cmsField.Description = field.description;
			cmsField.List = !string.IsNullOrEmpty(field.list) ? bool.Parse(field.list) : false;
			cmsField.Display = field.displayName;
			cmsField.CmsModelDefinition = isModelDefinition ? modelDefinition : null;
			cmsField.CmsSettingDefinition = isSettingDefinition ? settingDefinition : null;
			cmsField.Expose_Id = expose;

			var cmsFields = IoC.Resolve<ICmsFieldService>();
			var blueprintImportValidationService = IoC.Resolve<IBlueprintImportValidationService>();

			cmsField = cmsFields.Create(cmsField);

			blueprintImportValidationService.CreateValidations(field, cmsField);

			cmsFieldsDictionary.Add(cmsField.Name, cmsField);
			rawFieldsDictionary.Add(cmsField.Name, field);
		}
	}
}
