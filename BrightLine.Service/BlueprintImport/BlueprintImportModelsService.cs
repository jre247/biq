using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Core;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class BlueprintImportModelsService : IBlueprintImportModelsService
	{
		private ICmsModelDefinitionService CmsModelDefinitions { get;set;}

		public BlueprintImportModelsService()
		{
			CmsModelDefinitions = IoC.Resolve<ICmsModelDefinitionService>();
		}

		public async Task CreateModels(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> modelFilenames, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary)
		{
			var blueprintImportFieldsService = IoC.Resolve<IBlueprintImportFieldsService>();

			foreach (var modelFilename in modelFilenames)
			{
				var modelContents = await client.Repository.Content.GetAllContents(userAgent, blueprintManifestName, modelFilename);
				var modelContent = modelContents[0].Content;

				var model = JsonConvert.DeserializeObject<BlueprintImportModel>(modelContent);

				var modelDefinition = CreateModelDefinition(blueprintId, model);
				var modelDefinitionContainer = new BlueprintImportModelDefinition { CmsModelDefinition = modelDefinition };
				modelDefinitionDictionary.Add(modelDefinition.Name, modelDefinitionContainer);

				blueprintImportFieldsService.CreateFields(model, modelDefinitionDictionary, modelDefinition);
			}
		}

		/// <summary>
		//	The default display field is "display". In this case, there is no existing CmsField with a name equal to "display". 
		//	A model definition can have a display field be an actual CmsField, in which case you'll have to retrieve the FieldType of that CmsField and set
		//	the model definition's DisplayFieldType property to be equal to that CmsField's FieldType (e.g. integer, string, etc.).
		/// </summary>
		public void UpdateDisplayFieldTypeForModelDefinitions(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary)
		{
			var defaultDisplayField = "display";
			int displayFieldType = -1;

			foreach (var modelDefinitionItem in modelDefinitionDictionary)
			{
				var modelDefinition = modelDefinitionItem.Value.CmsModelDefinition;

				var displayFieldName = modelDefinition.DisplayFieldName;

				if (string.IsNullOrEmpty(displayFieldName))
					continue;

				//If model definition's DisplayName is "display" then set the CmsModelDefinition's display field type to be "string"
				if (string.Equals(defaultDisplayField, displayFieldName, StringComparison.OrdinalIgnoreCase))
				{
					displayFieldType = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image];
				}
				//If model definition's DisplayFieldName is not "display" then find the CmsField that corresponds to the CmsModelDefinition's DisplayName
				//and then use the FieldType of that CmsField to set the CmsModelDefinition's DisplayFieldTypeId
				else
				{
					var cmsFieldsDictionary = modelDefinitionItem.Value.CmsFieldsDictionary;
					var cmsField = cmsFieldsDictionary[displayFieldName];
					displayFieldType = cmsField.Type_Id.Value;
				}

				modelDefinition.DisplayFieldType_Id = displayFieldType;
				CmsModelDefinitions.Update(modelDefinition, true);

			}
		}

		public void CreateModelReferences(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary)
		{
			var cmsFields = IoC.Resolve<ICmsFieldService>();
			var cmsRefs = IoC.Resolve<IRepository<CmsRef>>();

			foreach (var model in modelDefinitionDictionary)
			{
				foreach (var fieldItem in model.Value.RawFieldsDictionary)
				{
					var field = fieldItem.Value;

					if (field.@ref == null)
						continue;

					//only check for Model Ref, not Page Ref
					if (field.@ref.model == null)
						continue;

					//create CmsRef record
					var cmsModelReferenceName = field.@ref.model;
					var cmsModelDefinition = modelDefinitionDictionary[cmsModelReferenceName].CmsModelDefinition;
					var cmsRefTypeName = field.@ref.type;
					var cmsRefType = Lookups.CmsRefTypes.HashByName[cmsRefTypeName];
					var cmsRef = new CmsRef
					{
						CmsModelDefinition = cmsModelDefinition,
						CmsRefType_Id = cmsRefType
					};
					cmsRefs.Insert(cmsRef);

					//set CmsRef_Id property for CmsField record
					var cmsField = model.Value.CmsFieldsDictionary[fieldItem.Key];
					cmsField.CmsRef = cmsRef;
					var fieldTypeModelRefId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel];
					cmsField.Type_Id = fieldTypeModelRefId;
					cmsFields.Update(cmsField, true);
				}
			}
		}

		/// <summary>
		///  Checks if a filename contains a specific pattern that would signify that it is a model that is part of the repository
		/// </summary>
		/// <param name="modelFilenames"></param>
		/// <param name="filename"></param>
		public bool CheckIfModelFilename(string filename)
		{
			//The pattern for a model filename in a Github repository is: cms.*.model.json
			var regex = new Regex(@"^cms.*.\.model.json");
			var match = regex.Match(filename);
			return match.Success;
			
		}

		private CmsModelDefinition CreateModelDefinition(int blueprintId, BlueprintImportModel model)
		{
			var cmsModelDefinition = new CmsModelDefinition
			{
				Name = model.name,
				Blueprint_Id = blueprintId,
				Display = model.displayName,
				DisplayFieldName = model.displayField
			};
			cmsModelDefinition = CmsModelDefinitions.Create(cmsModelDefinition, true);

			return cmsModelDefinition;
		}
	}
}
