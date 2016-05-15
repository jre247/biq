using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot;
using BrightLine.Publishing.Areas.AdResponses.Services.Destination;
using BrightLine.Publishing.Areas.AdResponses.Services.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Factories
{
	public class AdTypeAdResponses
	{
		/// <summary>
		/// Return a concrete service that will build Ad Responses for a specific Ad Type (e.g. Commercial Spot)
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="targetEnv"></param>
		/// <param name="publishId"></param>
		/// <returns></returns>
		public IAdTypeAdResponses GetService(Ad ad, string targetEnv, Guid publishId)
		{
			var adType = ad.AdType.Id;
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var destination = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];
			var commercialSpot = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];

			if (adType == overlay)
				return new OverlayAdResponses(ad, targetEnv);
			else if (adType == destination)
				return new DestinationAdResponses(ad, targetEnv, publishId);
			else if (adType == commercialSpot)
				return new CommercialSpotAdResponses(ad, targetEnv);
			else
				throw new ArgumentException("Invalid Ad Type");
		}
	}
}
