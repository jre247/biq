using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Constants
{
	public class RedisPubSubConstants
	{
		public class Channels
		{
			public static string CacheClear { get; private set; }
			public static string RefreshBrs { get; private set; }

			static Channels()
			{
				string targetEnvironment = null;

				// Set the Target Environment that will be used to prepend to any constants below
				if (Env.IsProd)
					targetEnvironment = TargetEnvironments.Production;
				else if (Env.IsUat)
					targetEnvironment = TargetEnvironments.Uat;
				else
					targetEnvironment = TargetEnvironments.Develop;

				CacheClear = string.Format("{0}_cache-clear", targetEnvironment);
				RefreshBrs = string.Format("{0}_refresh-brs", targetEnvironment);
			}
		}

		public class TargetEnvironments
		{
			public const string Develop = "dev";
			public const string Uat = "uat";
			public const string Production = "pro";
		}

		public class BrsFiles
		{
			public class Names
			{
				public const string Main = "brs-main";
				public const string RokuDirectIntegration = "brs-roku-direct-integration";
			}
		}
	}
}
