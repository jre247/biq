using BrightLine.Common.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportModelsService
	{
		Task CreateModels(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> modelFilenames, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary);
		void CreateModelReferences(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary);
		void UpdateDisplayFieldTypeForModelDefinitions(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary);
		bool CheckIfModelFilename(string filename);
	}
}
