using BrightLine.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.AdType
{
	public static class AdTypeConstants
	{
		public static class AdTypeIds
		{
			public const int BrandDestination = 10010;
			public const int DedicatedBrandApp = 10015;
			public const int OnScreenSurvey = 10019;
			public const int Overlay = 10017;
		}

		public static class AdTypeNames
		{
			public const string BrandDestination = "Brand Destination";
			public const string Overlay = "Overlay";
			public const string CommercialSpot = "Commercial Spot";
			public const string ImageBanner = "Image Banner";
			public const string VideoBanner = "Video Banner";
			public const string DedicatedBrandApp = "Dedicated Brand App";
			
		}

		public static class ManifestNames
		{
			 public const string CommercialSpot = "commercial-spot";
			 public const string Overlay = "overlay";
			 public const string BrandDestination = "brand-destination";
		}
		
	}
}
