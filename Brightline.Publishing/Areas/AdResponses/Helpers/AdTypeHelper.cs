using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightline.Publishing.Areas.AdResponses.Helpers
{
	public class AdTypeHelper
	{
		/// <summary>
		/// Get a hash that contains Ad Types that are allowed for publishing
		/// </summary>
		/// <returns></returns>
		public static HashSet<int> GetAllowedAdTypesHash()
		{
			var commercialSpot = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var brandDestination = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];
			var adTypesAllowed = new HashSet<int> { commercialSpot, overlay, brandDestination };

			return adTypesAllowed;
		}
	}
}
