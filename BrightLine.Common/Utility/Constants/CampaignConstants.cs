using BrightLine.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Campaigns
{
	public static class CampaignConstants
	{
		public const string BLUEPRINT_SAVED = "Blueprint saved";
		public const string PREVIEW_FILE = "PreviewFile";
		public const string CONNECTED_TV_FILE = "ConnectedTVCreativeFile";
		public const string CONNECTED_TV_SUPPORT_FILE = "ConnectedTVSupportFile";
		public const int NAME_MAX_LENGTH = 255;
		public const int Generation = 1;
		
		public static class SqlProcedures
		{
			public const string CAMPAIGN_LISTING = "[dbo].[Campaigns_Listing]";
			public const string CAMPAIGN_SUMMARY = "[dbo].[Campaign_Summary]";
			public const string CAMPAIGN_PERFORMANCE_SUMMARY = "[dbo].[Campaign_PerformanceSummary]";
			public const string CAMPAIGN_CREATIVES = "[dbo].[Campaign_Creatives]";
			public const string CAMPAIGN_ADS = "[dbo].[Campaign_Ads]";
		}

		public static class Errors
		{
			public const string CAMPAIGN_NAME_MISSING = "Campaign name must be provided.";
			public const string CAMPAIGN_NAME_MAX_LENGTH = "Campaign name must be less than 255 characters.";
			public const string CAMPAIGN_NULL = "Campaign cannot be null.";
		}
	}
}
