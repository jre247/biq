using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.CMS.Service
{
	public class SettingInstanceLookupsService : ISettingInstanceLookupsService
	{
		public SettingInstanceLookups CreateSettingInstanceLookups(CmsSettingInstance SettingInstance)
		{
			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();

			var settingId = SettingInstance.Setting_Id;
			var SettingInstanceLookupsDictionary = new Dictionary<int, SettingInstanceLookups>();

			if (!IsSettingInstanceLookupsCached())
				AddSettingInstanceLookupsDictionaryToCache(SettingInstanceLookupsDictionary);

			SettingInstanceLookupsDictionary = GetSettingInstanceLookupsDictionary();
			if (SettingInstanceLookupsDictionary.ContainsKey(settingId))
				throw new Exception("There is already a Setting Instance cached in the Request object.");

			var SettingInstanceLookups = new SettingInstanceLookups();
			SettingInstanceLookups.SettingInstance = SettingInstance;
			SettingInstanceLookups.Setting = cmsSettings.Get(SettingInstance.Setting_Id);

			SettingInstanceLookupsDictionary.Add(settingId, SettingInstanceLookups);
			CacheSettingInstanceLookupsDictionary(SettingInstanceLookupsDictionary);

			return SettingInstanceLookups;
		}

		public SettingInstanceLookups GetLookupsForSettingInstance(int settingId)
		{
			if (!IsSettingInstanceLookupsCached())
				throw new NullReferenceException("Setting Instance Lookups is not being cached.");

			var SettingInstanceLookupsDictionary = GetSettingInstanceLookupsDictionary();
			var SettingInstanceLookups = SettingInstanceLookupsDictionary[settingId];

			return SettingInstanceLookups;
		}

		public void SaveLookupsForSettingInstance(SettingInstanceLookups SettingInstanceLookups)
		{
			var SettingId = SettingInstanceLookups.SettingInstance.Setting_Id;
			if (!IsSettingInstanceLookupsCached())
				throw new NullReferenceException("Setting Instance Lookups is not being cached.");

			var SettingInstanceLookupsDictionary = GetSettingInstanceLookupsDictionary();
			SettingInstanceLookupsDictionary[SettingId] = SettingInstanceLookups;

			CacheSettingInstanceLookupsDictionary(SettingInstanceLookupsDictionary);
		}

		#region Private Methods

		private static void AddSettingInstanceLookupsDictionaryToCache(Dictionary<int, SettingInstanceLookups> SettingInstanceLookupsDictionary)
		{
			HttpContext.Current.Items.Add(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY, SettingInstanceLookupsDictionary);
		}

		private static bool IsSettingInstanceLookupsCached()
		{
			return HttpContext.Current.Items.Contains(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}

		private static Dictionary<int, SettingInstanceLookups> GetSettingInstanceLookupsDictionary()
		{
			return (Dictionary<int, SettingInstanceLookups>)HttpContext.Current.Items[SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY];
		}

		private static void CacheSettingInstanceLookupsDictionary(Dictionary<int, SettingInstanceLookups> SettingInstanceLookupsDictionary)
		{
			HttpContext.Current.Items[SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY] = SettingInstanceLookupsDictionary;
		}

		#endregion
	}
}
