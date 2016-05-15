using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Platform;
using BrightLine.Publishing.Areas.AdResponses.AdTypes.PlatformAdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightline.Publishing.Areas.AdResponses.Helpers;
using BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot.Platforms;

namespace BrightLine.Publishing.Areas.AdResponses.Factories
{
	public class PlatformCommercialSpotAdResponses
	{
		/// <summary>
		/// Get a concrete service that will build Ad Responses for a Commercial Spot and Platform combination (e.g. Ad Responses for Commercial Spot and Roku)
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="targetEnv"></param>
		/// <returns></returns>
		public IPlatformAdResponseService GetService(Ad ad, string targetEnv)
		{
			var platform = ad.Platform.Id;
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];

			if (platform == roku)
				return new RokuCommercialSpotAdResponse(ad, targetEnv);
			else if (PlatformHelper.IsValidHtml5(platform))
				return new Html5CommercialSpotAdResponse(ad, targetEnv);
			else
				throw new ArgumentException("Invalid Platform.");
		}

		
	}
}
