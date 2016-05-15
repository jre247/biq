using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.Analytics.Campaign
{
	public class CampaignAnalyticsSettings
	{
		#region Properties

		#region Initialization/Finalization

		public CampaignAnalyticsSettings()
		{
			DayDelay = -1;
			HourFloor = 6;
			HourFloorTimezone = "Eastern Standard Time";
			CacheMinuteDuration = 120;
		}

		#endregion

		#region Campaign Accessibility

		/// <summary>
		/// The delay (in hours) between a campaign's launch date and the acessibility of the dashboard
		/// </summary>
		public int DayDelay { get; private set; }
		
		/// <summary>
		/// The minimum hour of the day before the dashboard should apply the DayDelay
		/// </summary>
		public int HourFloor { get; private set; }

		/// <summary>
		/// The timezone that the HourFloor should be compared in
		/// </summary>
		public string HourFloorTimezone { get; private set; }

		/// <summary>
		/// The duration (in minutes) that an analytics service cache key/value pair should be saved
		/// </summary>
		public int CacheMinuteDuration { get; private set; }

		
		#endregion

		#endregion
	}
}
