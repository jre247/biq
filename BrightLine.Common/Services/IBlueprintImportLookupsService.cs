using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportLookupsService
	{
		BlueprintImportLookups CreateBlueprintImportLookups(string manifestName);
		BlueprintImportLookups GetBlueprintImportLookups(string manifestName);
		void SaveBlueprintImportLookups(BlueprintImportLookups blueprintImportLookups);
		void CachePageDefinition(string manifestName, PageDefinition pageDefinition);
		Dictionary<string, int> GetCachedPageDefinitions(string manifestName);
	}
}
