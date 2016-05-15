using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service.BlueprintImport
{
	public class BlueprintImportLookupsService : IBlueprintImportLookupsService
	{
		private IHttpContextHelper HttpContextHelper { get;set;}

		public BlueprintImportLookupsService()
		{
			HttpContextHelper = IoC.Resolve<IHttpContextHelper>();
		}

		public BlueprintImportLookups CreateBlueprintImportLookups(string manifestName)
		{
			var blueprintImportLookupsDictionary = new Dictionary<string, BlueprintImportLookups>();

			if (!IsBlueprintImportLookupsCached())
				AddBlueprintImportLookupsDictionaryToCache(blueprintImportLookupsDictionary);

			blueprintImportLookupsDictionary = GetBlueprintImportLookupsDictionary();
			if (blueprintImportLookupsDictionary.ContainsKey(manifestName))
				throw new Exception(string.Format("There is already a Blueprint for Manifest Name '{0}' being cached in the Request object.", manifestName));

			var blueprintImportLookups = new BlueprintImportLookups();
			blueprintImportLookups.ManifestName = manifestName;

			blueprintImportLookupsDictionary.Add(manifestName, blueprintImportLookups);
			CacheBlueprintImportLookupsDictionary(blueprintImportLookupsDictionary);

			return blueprintImportLookups;
		}

		public BlueprintImportLookups GetBlueprintImportLookups(string manifestName)
		{
			if (!IsBlueprintImportLookupsCached())
				throw new NullReferenceException("Blueprint Import Lookups is not being cached.");

			var blueprintImportLookupsDictionary = GetBlueprintImportLookupsDictionary();
			var blueprintImportLookups = blueprintImportLookupsDictionary[manifestName];

			return blueprintImportLookups;
		}

		public void SaveBlueprintImportLookups(BlueprintImportLookups blueprintImportLookups)
		{
			var manifestName = blueprintImportLookups.ManifestName;
			if (!IsBlueprintImportLookupsCached())
				throw new NullReferenceException("Blueprint Import Lookups is not being cached.");

			var blueprintImportLookupsDictionary = GetBlueprintImportLookupsDictionary();
			blueprintImportLookupsDictionary[manifestName] = blueprintImportLookups;

			CacheBlueprintImportLookupsDictionary(blueprintImportLookupsDictionary);
		}

		public void CachePageDefinition(string manifestName, PageDefinition pageDefinition)
		{
			var blueprintImportLookups = GetBlueprintImportLookups(manifestName);
			var pageDefinitions = blueprintImportLookups.PageDefinitions;
			if (pageDefinitions == null)
				pageDefinitions = new Dictionary<string, int>();

			if (!pageDefinitions.ContainsKey(pageDefinition.Name))
			{
				pageDefinitions.Add(pageDefinition.Name, pageDefinition.Id);
				blueprintImportLookups.PageDefinitions = pageDefinitions;
				SaveBlueprintImportLookups(blueprintImportLookups);
			}	
		}

		public Dictionary<string, int> GetCachedPageDefinitions(string manifestName)
		{
			var blueprintImportLookups = GetBlueprintImportLookups(manifestName);
			var pageDefinitions = blueprintImportLookups.PageDefinitions;

			return pageDefinitions;
		}

		#region Private Methods

		private void AddBlueprintImportLookupsDictionaryToCache(Dictionary<string, BlueprintImportLookups> blueprintImportLookupsDictionary)
		{
			HttpContextHelper.Items.Add(BlueprintImportConstants.BlueprintImportLookupsKey, blueprintImportLookupsDictionary);
		}

		private bool IsBlueprintImportLookupsCached()
		{
			return HttpContextHelper.Items.Contains(BlueprintImportConstants.BlueprintImportLookupsKey);
		}

		private Dictionary<string, BlueprintImportLookups> GetBlueprintImportLookupsDictionary()
		{
			return (Dictionary<string, BlueprintImportLookups>)HttpContextHelper.Items[BlueprintImportConstants.BlueprintImportLookupsKey];
		}

		private void CacheBlueprintImportLookupsDictionary(Dictionary<string, BlueprintImportLookups> blueprintImportLookupsDictionary)
		{
			HttpContextHelper.Items[BlueprintImportConstants.BlueprintImportLookupsKey] = blueprintImportLookupsDictionary;
		}

		#endregion
	}
}
