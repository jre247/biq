using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Blueprints;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Core;
using BrightLine.Service.BlueprintImport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class BlueprintImportPagesService : IBlueprintImportPagesService
	{
		private BlueprintImportLookupsService BlueprintImportLookupsService {get;set;}

		public BlueprintImportPagesService()
		{
			BlueprintImportLookupsService = new BlueprintImportLookupsService();
		}

		public void CreatePageReferences(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary, string manifestName)
		{
			var cmsRefs = IoC.Resolve<IRepository<CmsRef>>();
			var cmsFields = IoC.Resolve<ICmsFieldService>();

			foreach (var model in modelDefinitionDictionary)
			{
				foreach (var fieldItem in model.Value.RawFieldsDictionary)
				{
					var field = fieldItem.Value;

					if (field.@ref == null)
						continue;

					//only check for Page Ref, not Model Ref
					if (field.@ref.page == null)
						continue;

					//create CmsRef record
					var pageReferenceName = field.@ref.page;
					var pageDefinitions = BlueprintImportLookupsService.GetCachedPageDefinitions(manifestName);
					var pageDefinitionId = pageDefinitions[pageReferenceName];
					var cmsRefTypeName = field.@ref.type;
					var cmsRefType = Lookups.CmsRefTypes.HashByName[cmsRefTypeName];
					var cmsRef = new CmsRef
					{
						PageDefinition_Id = pageDefinitionId,
						CmsRefType_Id = cmsRefType
					};
					cmsRefs.Insert(cmsRef);

					//set CmsRef_Id property for CmsField record
					var cmsField = model.Value.CmsFieldsDictionary[fieldItem.Key];
					var fieldTypePageRefId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage];
					cmsField.Type_Id = fieldTypePageRefId;
					cmsField.CmsRef = cmsRef;
					cmsFields.Update(cmsField, true);
				}
			}
		}


		/// <summary>
		/// Import all pages from the Github repository
		///		*note: the filename in each repository for pages is "_pages.json"
		/// </summary>
		/// <param name="userAgent"></param>
		/// <param name="blueprintId"></param>
		/// <param name="blueprintManifestName"></param>
		/// <param name="client"></param>
		/// <param name="modelFilenames"></param>
		/// <returns></returns>
		public async Task CreatePages(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> modelFilenames)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var pageDefinitions = IoC.Resolve<IPageDefinitionService>();

			var pagesPath = settings.GithubRepositoryPagesPath;
			var pagesContents = await client.Repository.Content.GetAllContents(userAgent, blueprintManifestName, pagesPath);
			var pagesContent = pagesContents[0].Content;

			var pagesAsJson = JArray.Parse(pagesContent);
			foreach (var pageAsJson in pagesAsJson)
			{
				var pageAsJsonString = pageAsJson.ToString();
				var page = JsonConvert.DeserializeObject<BlueprintImportPage>(pageAsJsonString);

				if(page.pageType != BlueprintConstants.PAGE_TYPE_PAGE)
					continue;

				var pageDefinition = new PageDefinition
				{
					Key = int.Parse(page.key),
					Name = page.displayName,
					Blueprint_Id = blueprintId
				};
				pageDefinition = pageDefinitions.Create(pageDefinition);

				BlueprintImportLookupsService.CachePageDefinition(blueprintManifestName, pageDefinition);			
			}
		}	
	}
}
