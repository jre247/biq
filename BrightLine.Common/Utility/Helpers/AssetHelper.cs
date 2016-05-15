using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public class AssetHelper
	{
		public static string GetAssetSuffix()
		{
			string env = Env.Name.ToLower();

			string assetSuffix = "";
			bool envIsLocalOrDev = Array.IndexOf(new string[] { "local", "dev" }, env) >= 0;
			bool envIsUatOrPro = Array.IndexOf(new string[] { "uat", "pro" }, env) >= 0;
			if (envIsLocalOrDev || envIsUatOrPro && System.Web.HttpContext.Current.Request["buildmode"] == "original")
			{
				assetSuffix = "original.";
			}

			return assetSuffix;
		}
	}
}
