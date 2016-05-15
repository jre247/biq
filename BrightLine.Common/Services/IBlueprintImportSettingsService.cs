using BrightLine.Common.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportSettingsService
	{
		Task CreateSettings(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> settingFilenames, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary);
		bool CheckIfSettingsFilename(string filename);
	}
}
