using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightline.Publishing.Areas.AdResponses.Helpers
{
	public class PlatformHelper
	{
		#region Public Methods

		/// <summary>
		/// Get a hash that contains Platforms that are allowed for publishing
		/// </summary>
		/// <returns></returns>
		public static HashSet<int> GetAllowedPlatformsHash()
		{
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var fireTv = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.FireTV];
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];			
			var platformsAllowed = new HashSet<int>{roku, fireTv, samsung};

			return platformsAllowed;
		}

		/// <summary>
		/// Determine if a Platform is Html5 and also valid
		/// </summary>
		/// <param name="platform"></param>
		/// <returns></returns>
		public static bool IsValidHtml5(int platform)
		{
			var html5Platforms = GetAllowedHtml5Platforms();

			return html5Platforms.Contains(platform);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Get a hash of allowed Html5 Platforms
		/// </summary>
		/// <returns></returns>
		private static HashSet<int> GetAllowedHtml5Platforms()
		{
			var fireTv = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.FireTV];
			var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];
			var html5Platforms = new HashSet<int> { fireTv, samsung };

			return html5Platforms;
		}

		#endregion
	}
}
