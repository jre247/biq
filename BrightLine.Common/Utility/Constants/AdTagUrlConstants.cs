using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Constants
{
	public class AdTagUrlConstants
	{
		public const string RokuAdIdMacro = "Roku_Ad_Id=ROKU_ADS_APP_ID";

		public class AdServer
		{
			public const string AdServerIdMacro = "%%UNIQUE_ID%%";
			public const string DurationType = "impression";

			public class Types
			{
				public const string Duration = "duration";
				public const string Impression = "impression";
			}

			public class PercentComplete
			{
				public const int FirstQuartile = 25;
				public const int Midpoint = 50;
				public const int ThirdQuartile = 75;
				public const int Complete = 100;
			}
		}

		public class IQ
		{
			public class Types
			{
				public const string AdClick = "ad_click";
				public const string Impression = "impression";
			}
		}	

		public class Macros
		{
			public const string Redirect = "redir=%%REDIR%%";
		}

		public class QueryParams
		{
			public const string MBList = "mblist";
		}
	}
}
