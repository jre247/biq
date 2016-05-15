using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Constants
{
	public static class PublishConstants
	{	
		public const int Sequence = 1;

		public class AdResponseRokuSdkPrefixes
		{
			public const string RAF = "_raf";
			public const string DI = "_di";
		}

		public class ResponseTypes
		{
			public const string Xml = "application/xml";
			public const string Json = "application/json";
		}

		public class TargetEnvironments
		{
			public const string Develop = "dev";
			public const string Uat = "uat";
			public const string Production = "pro";
		}

		public class AdResponseDictionaryKeys
		{
			public const string Promotional = "promotional";
			public const string Destination = "destination";
		}

		public class DestinationAdResponse
		{
			public class Roku
			{
				public class MappedEnvironments
				{
					public const string Develop = "develop";
					public const string Uat = "uat";
					public const string Production = "production";
				}
			}		
		}
	}
}
