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
using BrightLine.Publishing.Areas.AdResponses.Services.Destination.Platforms;

namespace BrightLine.Publishing.Areas.AdResponses.Factories
{
	public class PlatformDestinationAdResponses
	{
		/// <summary>
		/// Get a concrete service that will build Ad Responses for Destination and Platform combination (e.g. Ad Responses for Destination and Roku)
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="targetEnv"></param>
		/// <param name="publishedId"></param>
		/// <returns></returns>
		public IPlatformAdResponseService GetService(Ad ad, string targetEnv, Guid publishedId)
		{
			var platform = ad.Platform.Id;
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
		
			if (platform == roku)
				return new RokuDestinationAdResponse(ad, targetEnv, publishedId);
			else if (PlatformHelper.IsValidHtml5(platform))
				return new Html5DestinationAdResponse(ad, targetEnv);
			else
				throw new ArgumentException("Invalid Platform.");
		}
	}
}
