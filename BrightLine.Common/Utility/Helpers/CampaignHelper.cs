using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public static class CampaignHelper
	{
		public static IEnumerable<ILookup> GetStatusList()
		{
			var lookups = new List<ILookup>();
			var statuses = Enum.GetValues(typeof(CampaignStatus));

			foreach (var status in statuses)
			{
				var name = status.ToString();
				var id = (int)status;

				lookups.Add(new EntityLookup { Id = id, Name = name });
			}

			return lookups;
		}

		public static string GetCampaignStatus(string beginDateRaw, string endDateRaw)
		{
			DateTime? beginDate = null;
			DateTime? endDate = null;
			DateTime b, e;
			bool isValidDate = true;

			if (!string.IsNullOrWhiteSpace(beginDateRaw))
			{
				isValidDate = DateTime.TryParse(beginDateRaw, out b);
				if (!isValidDate) return null;
				beginDate = b;
			}


			if (!string.IsNullOrWhiteSpace(endDateRaw))
			{
				isValidDate = DateTime.TryParse(endDateRaw, out e);
				if (!isValidDate) return null;
				endDate = e;
			}

			return GetCampaignStatusForDates(beginDate, endDate);
		}

		public static string GetCampaignStatus(DateTime? beginDate, DateTime? endDate)
		{
			return GetCampaignStatusForDates(beginDate, endDate);
		}

		private static string GetCampaignStatusForDates(DateTime? beginDate, DateTime? endDate)
		{
			var dateHelperService = IoC.Resolve<IDateHelperService>();

			var now = dateHelperService.GetDateUtcNow();

			if (beginDate == null || beginDate > now)
				return CampaignStatus.Upcoming.ToString();
			else if (endDate < now)
				return CampaignStatus.Completed.ToString();
			else
				return CampaignStatus.Delivering.ToString();
		}


	}
}
