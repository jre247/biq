using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public static class Lookups
	{
		public static LookupsDictionary LookupsDictionary { get; set; }

		public static LookupDictionaryItem FieldTypes { get { return Lookups.LookupsDictionary.Lookups["FieldType"]; } }
		public static LookupDictionaryItem ResourceTypes { get { return Lookups.LookupsDictionary.Lookups["ResourceType"]; } }
		public static LookupDictionaryItem ImageResourceTypes { get { return Lookups.LookupsDictionary.Lookups["ResourceTypeImage"]; } }
		public static LookupDictionaryItem VideoResourceTypes { get { return Lookups.LookupsDictionary.Lookups["ResourceTypeVideo"]; } }
		public static LookupDictionaryItem FileTypes { get { return Lookups.LookupsDictionary.Lookups["FileType"]; } }
		public static LookupDictionaryItem ValidationTypes { get { return Lookups.LookupsDictionary.Lookups["ValidationType"]; } }
		public static LookupDictionaryItem Platforms  { get { return Lookups.LookupsDictionary.Lookups["Platform"]; } }
		public static LookupDictionaryItem AdTypes { get { return Lookups.LookupsDictionary.Lookups["AdType"]; } }
		public static LookupDictionaryItem Exposes { get { return Lookups.LookupsDictionary.Lookups["Expose"]; } }
		public static LookupDictionaryItem CmsRefTypes { get { return Lookups.LookupsDictionary.Lookups["CmsRefType"]; } }
		public static LookupDictionaryItem StorageSources { get { return Lookups.LookupsDictionary.Lookups["StorageSource"]; } }
		public static LookupDictionaryItem Roles { get { return Lookups.LookupsDictionary.Lookups["Role"]; } }
		public static LookupDictionaryItem CmsPublishStatuses { get { return Lookups.LookupsDictionary.Lookups["CmsPublishStatus"]; } }
		public static LookupDictionaryItem AdFunctions { get { return Lookups.LookupsDictionary.Lookups["AdFunction"]; } }
		public static LookupDictionaryItem Agencies { get { return Lookups.LookupsDictionary.Lookups["Agency"]; } }
		public static LookupDictionaryItem Products { get { return Lookups.LookupsDictionary.Lookups["Product"]; } }
		public static LookupDictionaryItem AdTypeGroups { get { return Lookups.LookupsDictionary.Lookups["AdTypeGroup"]; } }
		public static LookupDictionaryItem Metrics { get { return Lookups.LookupsDictionary.Lookups["Metric"]; } }
		public static LookupDictionaryItem Brands { get { return Lookups.LookupsDictionary.Lookups["Brand"]; } }
		public static LookupDictionaryItem Advertisers { get { return Lookups.LookupsDictionary.Lookups["Advertiser"]; } }
		public static LookupDictionaryItem TrackingEvents { get { return Lookups.LookupsDictionary.Lookups["TrackingEvent"]; } }
		public static LookupDictionaryItem Placements { get { return Lookups.LookupsDictionary.Lookups["Placement"]; } }

		/// <summary>
		/// Build up a dictionary for each lookup that will be used to map the lookup name to its id
		/// </summary>
		public static void InitializeLookupDictionaries()
		{
			var lookups = new List<LookupItem>();
			var da = new BrightLine.Common.Utility.SqlServer.DataAccess("OLTP");
			da.ExecuteReader<LookupItem>("[dbo].[GetAllLookups]", (@as) => lookups = @as);

			Lookups.LookupsDictionary = new LookupsDictionary(lookups);
		}

		public static void Refresh()
		{
			InitializeLookupDictionaries();
		}
	}
}
