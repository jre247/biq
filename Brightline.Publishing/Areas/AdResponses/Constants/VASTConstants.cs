using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Constants
{
	public static class VASTConstants
	{
		public const string DeliveryProgressive = "progressive";
		public const string ApiFramework = "brightline";
		public const string AdSystem = "BrightLine";
		public const string CreativeType = "application/json";
		public const string BrightlineTrackingEventsNode = "Brightline_Tracking_Events";

		public class Macros
		{
			public const string BrightlineTrackingEvents = "%%Brightline_Tracking_Events%%";
		}

		public class MediaFileTypes
		{
			public const string mp4 = "video/mp4";
		}


		public class TrackingUrlKeys
		{
			public const string Start = "start";
			public const string FirstQuartile = "firstQuartile";
			public const string Midpoint = "midpoint";
			public const string ThirdQuartile = "thirdQuartile";
			public const string Complete = "complete";
		}
	}
}
