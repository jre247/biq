using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.SqlServer
{
	public static class DataAccessConstants
	{
		public const string OLTP_CONNECTION_STRING = "OLTP";
		public const string OLAP_CONNECTION_STRING = "OLAP";

		public static class SqlParameters
		{
			public const string USER_ID = "@userId";
			public const string ADVERTISER_ID = "@advertiserId";
			public const string CAMPAIGN_ID = "@campaignId";
			public const string BEGIN_DATETIME_ID = "@beginDateTimeId";
			public const string END_DATETIME_ID = "@endDateTimeId";
			public const string TIME_INTERVAL = "@timeInterval";
			public const string BEGIN_DATE = "@beginDate";
			public const string END_DATE = "@endDate";
			public const string DATE_TIME_INTERVAL = "@timeInterval";
			public const string MediaPartner = "@mediaPartnerId";
		}
	}
}
