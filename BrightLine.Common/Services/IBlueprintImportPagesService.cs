using BrightLine.Common.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportPagesService
	{
		void CreatePageReferences(Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary, string manifestName);
		Task CreatePages(string userAgent, int blueprintId, string blueprintManifestName, GitHubClient client, List<string> modelFilenames);
	}
}
