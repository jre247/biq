using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Constants
{
	public static class CmsPublishConstants
	{
		public static class Statuses
		{
			public const string InProgress = "In Progress";
			public const string Failed = "Failed";
			public const string Timeout = "Timeout";
			public const string Canceled = "Canceled";
			public const string Completed = "Completed";
		}

		public static class Environments
		{
			public const string Dev = "DEV";
			public const string Uat = "UAT";
			public const string Pro = "PRO";
		}

		public static class Errors
		{
			public const string InvalidEnvironment = "Invalid Environment is being used.";
			public const string InaccessibleCampaign = "Campaign is not accessible.";
		}

		public class ModelInstanceJsonProperties
		{
			public const string Id = "id";
			public const string ModelName = "modelname";
			public const string BL = "_bl";
		}

		public class SettingInstanceJsonProperties
		{
			public const string Id = "id";
			public const string BL = "_bl";
		}
		
	}
}
