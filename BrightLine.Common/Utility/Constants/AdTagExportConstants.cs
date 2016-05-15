using BrightLine.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.AdTagExport
{
	public static class AdTagExportConstants
	{
		public const string VastPrefix = "VAST_";
		public const string AdTagUrlPostfix = "v=" + Macros.Cachebuster;

		public static class Macros
		{
			public const string Cachebuster = "%%CACHEBUSTER%%";
		}

		public static class Spreadsheet
		{
			public const string Name =  "Summary";
			public const string DataTableName = "Campaign Summary";
			
			public static class Columns
			{
				public const string AdId = "Ad Id";
				public const string TagId = "Tag Id";
				public const string Platform = "Platform";
				public const string MediaPartner = "MediaPartner";
				public const string Name = "Name";
				public const string Creative = "Creative";
				public const string AdType = "Ad Type";
				public const string AdFormat = "Ad Format";
				public const string AdFunction = "Ad Function";
				public const string BeginDate = "Begin Date";
				public const string EndDate = "End Date";
				public const string DeliveryGroup = "Delivery Group";
				public const string Placement = "Placement";
				public const string Tag = "Tag";
				public const string Impression = "Impression";
				public const string Click = "Click";
			}

		}
		
	}
}
