using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Blueprints;
using BrightLine.Common.ViewModels.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BrightLine.Data;
using BrightLine.Utility;
using System.Reflection;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Utility.Blueprints;
using BrightLine.Service.BlueprintImport;

namespace BrightLine.Service
{
	public class BlueprintImportService : IBlueprintImportService
	{
		private CmsModelDefinition ModelDefinition { get;set;}
		private CmsSettingDefinition SettingDefinition { get; set; }
		private Dictionary<string, BlueprintImportModelDefinition> ModelDefinitionDictionary { get; set; }
		private int? BlueprintId { get;set;}
		private string BlueprintManifestName { get;set;}
		private BlueprintImportLookupsService BlueprintImportLookupsService { get; set; }

		public BlueprintImportService()
		{
			ModelDefinitionDictionary = new Dictionary<string, BlueprintImportModelDefinition>();
			BlueprintImportLookupsService = new BlueprintImportLookupsService();
		}

		public async Task<BoolMessageItem> ImportBlueprint(int? blueprintId, string blueprintManifestName)
		{
			var blueprintImportLookupsService = IoC.Resolve<IBlueprintImportLookupsService>();
			var settings = IoC.Resolve<ISettingsService>();

			BlueprintId = blueprintId.Value;
			BlueprintManifestName = blueprintManifestName;
			if (BlueprintId == null)
				new BoolMessageItem(false, BlueprintConstants.Errors.BLUEPRINT_DOES_NOT_EXIST);

			var client = SetupGitHubClient();

			var modelFilenames = new List<string>();
			var settingFilenames = new List<string>();

			blueprintImportLookupsService.CreateBlueprintImportLookups(blueprintManifestName);

			await ExecuteBlueprintImport(settings.GithubUserAgent, client, modelFilenames, settingFilenames);
			
			return new BoolMessageItem(true, null);
		}

		private async Task ExecuteBlueprintImport(string userAgent, GitHubClient client, List<string> modelFilenames, List<string> settingFilenames)
		{
			try
			{
				var blueprintImportPagesService = IoC.Resolve<IBlueprintImportPagesService>();
				var blueprintImportModelsService = IoC.Resolve<IBlueprintImportModelsService>();
				var blueprintImportSettingsService = IoC.Resolve<IBlueprintImportSettingsService>();
				var oltpContextHelper = IoC.Resolve<IOLTPContextHelper>();

				await GetModelsAndSettingsFilenamesFromGithub(userAgent, client, modelFilenames, settingFilenames);

				await blueprintImportPagesService.CreatePages(userAgent, BlueprintId.Value, BlueprintManifestName, client, settingFilenames);

				await blueprintImportModelsService.CreateModels(userAgent, BlueprintId.Value, BlueprintManifestName, client, modelFilenames, ModelDefinitionDictionary);

				await blueprintImportSettingsService.CreateSettings(userAgent, BlueprintId.Value, BlueprintManifestName, client, settingFilenames, ModelDefinitionDictionary);

				blueprintImportModelsService.UpdateDisplayFieldTypeForModelDefinitions(ModelDefinitionDictionary);

				blueprintImportModelsService.CreateModelReferences(ModelDefinitionDictionary);

				blueprintImportPagesService.CreatePageReferences(ModelDefinitionDictionary, BlueprintManifestName);

				oltpContextHelper.SaveOLTPContext();
			}
			catch (NotFoundException ex)
			{
				throw ex;
			}
		
		}

		/// <summary>
		/// First the app will need to make an api call to Github to get all filenames for Models and also Settings for a specific repository.
		/// </summary>
		/// <param name="blueprint"></param>
		/// <param name="userAgent"></param>
		/// <param name="client"></param>
		/// <param name="modelFilenames"></param>
		/// <param name="settingFilenames"></param>
		/// <returns></returns>
		private async Task GetModelsAndSettingsFilenamesFromGithub(string userAgent, GitHubClient client, List<string> modelFilenames, List<string> settingFilenames)
		{
			var blueprintImportModelsService = IoC.Resolve<IBlueprintImportModelsService>();
			var blueprintImportSettingsService = IoC.Resolve<IBlueprintImportSettingsService>();

			var repoFiles = await client.Repository.Content.GetAllContents(userAgent, BlueprintManifestName, "/");
			foreach (var file in repoFiles)
			{
				var filename = file.Name;

				var isModelMatch = blueprintImportModelsService.CheckIfModelFilename(filename);
				if (isModelMatch)
					modelFilenames.Add(filename);

				var isSettingMatch = blueprintImportSettingsService.CheckIfSettingsFilename(filename);
				if (isSettingMatch)
					settingFilenames.Add(filename);
			}
		}

		private GitHubClient SetupGitHubClient()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var username = settings.GithubUserAgentUsername;
			var password = settings.GithubUserAgentPassword; 

			var client = new GitHubClient(new ProductHeaderValue(settings.GithubUserAgent));
			var basicAuth = new Credentials(username, password);
			client.Credentials = basicAuth;

			return client;
		}	
	}
}
