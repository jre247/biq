using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
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
	public class BlueprintImportSettingsService : IBlueprintImportSettingsService
	{
		public async Task CreateSettings(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> settingFilenames, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary)
		{
			var blueprintImportFieldsService = IoC.Resolve<IBlueprintImportFieldsService>();

			foreach (var settingFilename in settingFilenames)
			{
				var settingContents = await client.Repository.Content.GetAllContents(userAgent, blueprintManifestName, settingFilename);
				var settingContent = settingContents[0].Content;

				var setting = JsonConvert.DeserializeObject<BlueprintImportModel>(settingContent);

				var settingDefinition = CreateSettingDefinition(blueprintId, setting);

				blueprintImportFieldsService.CreateFields(setting, modelDefinitionDictionary, null, settingDefinition);
			}
		}

		private CmsSettingDefinition CreateSettingDefinition(int blueprintId, BlueprintImportModel model)
		{
			var cmsSettingDefinitions = IoC.Resolve<IRepository<CmsSettingDefinition>>();

			var cmsSettingDefinition = new CmsSettingDefinition
			{
				Name = model.name,
				Blueprint_Id = blueprintId,
				Display = model.displayName,
			};
			cmsSettingDefinitions.Insert(cmsSettingDefinition);
			cmsSettingDefinitions.Save();

			return cmsSettingDefinition;
		}

		/// <summary>
		/// Checks if a filename contains a specific pattern that would signify that it is a file that contains Settings
		/// </summary>
		/// <param name="settingFilenames"></param>
		/// <param name="filename"></param>
		public bool CheckIfSettingsFilename(string filename)
		{
			//The pattern for a settings filename in a Github repository is: cms.settings.json
			var regex = new Regex(string.Format(@"^{0}", BlueprintImportConstants.SettingsJsonPath));
			var match = regex.Match(filename);
			return match.Success;
		}

	}
}
