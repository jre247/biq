using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Models.Enums;

namespace BrightLine.Common.Utility
{
	public static class DateHelper
	{
		public const string DATE_FORMAT = "MM/dd/yyyy hh:mm:ss tt";

		/// <summary>
		/// To user time zone date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime ToUserTimezone(DateTime date)
		{
			return ToUserTimezone(date, null);
		}

		/// <summary>
		/// To user timezone date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime? ToUserTimezone(DateTime? date)
		{
			return ToUserTimezone(date, null);
		}

		/// <summary>
		/// To user time zone date.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="timeZoneInfo"></param>
		/// <returns></returns>
		public static DateTime ToUserTimezone(DateTime date, string timeZoneInfo)
		{
			var tzi = string.IsNullOrWhiteSpace(timeZoneInfo) ? Auth.UserTimeZoneInfo :
				TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(tz => tz.Id == timeZoneInfo);
			if (tzi == null)
				return default(DateTime);

			var converted = TimeZoneInfo.ConvertTimeFromUtc(date, tzi);
			return converted;
		}

		/// <summary>
		/// To user timezone date.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="timeZoneInfo">Allows overriding the user time zone.</param>
		/// <returns></returns>
		public static DateTime? ToUserTimezone(DateTime? date, string timeZoneInfo)
		{
			if (date == null)
				return (DateTime?)null;

			var tzi = string.IsNullOrWhiteSpace(timeZoneInfo) ? Auth.UserTimeZoneInfo :
				TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(tz => tz.Id == timeZoneInfo);
			if (tzi == null)
				return (DateTime?)null;

			var converted = TimeZoneInfo.ConvertTimeFromUtc(date.Value, tzi);
			return converted;
		}

		//http://stackoverflow.com/questions/662379/calculate-date-from-week-number
		public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
		{
			var jan1 = new DateTime(year, 1, 1);
			var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			var firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear;
			if (firstWeek <= 1)
				weekNum -= 1;

			var result = firstThursday.AddDays(weekNum * 7);
			return result.AddDays(-3);
		}

		public static DateTime AddWeeks(this DateTime from, int weeks, DayOfWeek? dayOfWeek = null)
		{
			if (weeks == 0)
				return from;

			var startDay = dayOfWeek ?? DayOfWeek.Sunday;
			int dow = ((int)startDay - (int)from.DayOfWeek + 7) % 7;
			int daysToAdd = dow + (weeks * 7);
			var next = from.AddDays(daysToAdd);
			return next;
		}

		public static DateTime? FromDateTimeId(int? dateTimeId, DateTime? minDate = null, DateTime? maxDate = null)
		{
			DateTime? date = null;
			if (!dateTimeId.HasValue)
				return date;

			var hour = 0;
			var isWeekly = (dateTimeId < 999999); // if dateTimeId is weekly, special case	
			if (dateTimeId > 1000000000) // if dateTimeId is hourly, divide by 100 to get daily
			{
				hour = dateTimeId.Value % 100 - 1; // hours are in 0-23, stored in datetime as 1-24
				dateTimeId /= 100;
			}

			if (isWeekly)
			{
				var year = dateTimeId.Value / 100;
				var week = dateTimeId.Value % 100;
				if (week == 0)
					week = 1;
				date = DateHelper.FirstDateOfWeekISO8601(year, week);
			}
			else
			{
				var year = dateTimeId.Value / 10000;
				var month = dateTimeId.Value % 10000 / 100;
				if (month == 0)
					month = 1;
				var day = dateTimeId.Value % 100;
				if (day == 0)
					day = 1;
				if (hour < 0)
					hour = 0;
				date = new DateTime(year, month, day, hour, 0, 0);
			}

			date = EnsureBounds(date, minDate, maxDate);
			return date;
		}

		public static int? ToDateTimeId(DateTime? date, DateTime? minDate = null, DateTime? maxDate = null)
		{
			int? dateTimeId = null;
			if (!date.HasValue)
				return dateTimeId;

			var year = date.Value.Year * 100 * 100 * 100;
			var month = date.Value.Month * 100 * 100;
			var day = date.Value.Day * 100;
			var hour = date.Value.Hour;

			dateTimeId = year + month + day + hour;

			dateTimeId = EnsureBounds(dateTimeId, minDate, maxDate);
			return dateTimeId;
		}

		public static int? ToDateTimeId(int? dateTimeId, DateTime? minDate = null, DateTime? maxDate = null)
		{
			if (!dateTimeId.HasValue)
				return dateTimeId;

			var isWeekly = (dateTimeId < 999999); // if dateTimeId is weekly, special case	
			if (dateTimeId > 999999 && dateTimeId < 99999999) // if dateTimeId is daily, multiply by 100 to get hourly
				dateTimeId *= 100;

			if (isWeekly)
			{
				var year = dateTimeId.Value / 100;
				var week = dateTimeId.Value % 100;
				var date = DateHelper.FirstDateOfWeekISO8601(year, week);
				dateTimeId = DateHelper.ToDateTimeId(date);
			}

			dateTimeId = EnsureBounds(dateTimeId, minDate, maxDate);
			return dateTimeId;
		}

		public static string ToUserTimezoneString(int? dateTimeId, DateTime? minDate = null, DateTime? maxDate = null, string format = DateHelper.DATE_FORMAT)
		{
			var dt = DateHelper.FromDateTimeId(dateTimeId, minDate, maxDate);
			return DateHelper.ToUserTimezoneString(dt, minDate, maxDate, format);
		}

		public static string ToUserTimezoneString(DateTime? date, DateTime? minDate = null, DateTime? maxDate = null, string format = DateHelper.DATE_FORMAT)
		{
			var dt = ToUserTimezone(date, null);
			if (dt == null)
				return "";

			return DateHelper.ToString(dt.Value, minDate, maxDate, format);
		}

		public static string ToString(DateTime date, DateTime? minDate = null, DateTime? maxDate = null, string format = DateHelper.DATE_FORMAT)
		{
			date = EnsureBounds(date, minDate, maxDate);
			return date.ToString(format);
		}

		public static string ToString(DateTime? date, DateTime? minDate = null, DateTime? maxDate = null, string format = DateHelper.DATE_FORMAT)
		{
			if (date == null)
				return "";

			return DateHelper.ToString(date.Value, minDate, maxDate, format);
		}

		public static string ToDateTimeIdString(int? dateTimeId, DateTime? minDate = null, DateTime? maxDate = null, string format = DateHelper.DATE_FORMAT)
		{
			var dt = DateHelper.FromDateTimeId(dateTimeId);
			return DateHelper.ToString(dt, minDate, maxDate, format);
		}

		public static string ToEndDateTimeIdString(int? dateTimeId, AnalyticsInterval interval, DateTime? minDate = null, DateTime? maxDate = null)
		{
			var nextDate = DateHelper.FromDateTimeId(dateTimeId);
			if (nextDate == null)
				return null;

			var date = nextDate.Value;
			switch (interval)
			{
				case AnalyticsInterval.Hour:
					date = date.AddHours(1).AddMinutes(-1);
					break;
				case AnalyticsInterval.Day:
					date = date.AddDays(1).AddMinutes(-1);
					break;
				case AnalyticsInterval.Week:
					date = date.AddWeeks(1, DayOfWeek.Monday).AddMinutes(-1);
					break;
				case AnalyticsInterval.Month:
					date = date.AddMonths(1).AddMinutes(-1);
					break;
			};

			if (date > DateTime.UtcNow)
				date = DateTime.UtcNow.Date.AddMinutes(-1);

			return DateHelper.ToString(date, minDate, maxDate);
		}

		/// <summary>
		/// Gets the next occurence of a given DayOfWeek after specified DateTime
		/// </summary>
		/// <param name="date"></param>
		/// <param name="dayOfWeek"></param>
		/// <returns></returns>
		public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
		{
			int start = (int)date.DayOfWeek;
			int target = (int)dayOfWeek;
			if (target <= start)
				target += 7;
			return date.AddDays(target - start);
		}

		/// <summary>
		/// Gets the first day of the month of a specified DateTime
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		/// Gets the last day of the month of a specified DateTime
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime LastDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
		}

		public static IEnumerable<DateTimePart> GetDatePartsInRange(DateTime begin, DateTime end, DateIntervalEnum interval)
		{
			IEnumerable<DateTimePart> returnValue;
			switch (interval)
			{
				case DateIntervalEnum.Hour:
					returnValue = EachHour(begin, end);
					break;
				case DateIntervalEnum.Day:
					returnValue = EachDay(begin, end);
					break;
				case DateIntervalEnum.Week:
					returnValue = EachWeek(begin, end);
					break;
				case DateIntervalEnum.Month:
					returnValue = EachMonth(begin, end);
					break;
				default:
					throw new Exception(string.Format("Invalid interval: {0}", interval));
			}

			return returnValue;
		}

		public static DateIntervalEnum GetIntervalFromString(string dateInterval)
		{
			string val = string.Empty;
			if (!string.IsNullOrEmpty(dateInterval))
			{
				val = dateInterval.ToLower().Trim();
			}

			switch (val)
			{
				case "second":
					return DateIntervalEnum.Second;
				case "minute":
					return DateIntervalEnum.Minute;
				case "hour":
					return DateIntervalEnum.Hour;
				case "day":
					return DateIntervalEnum.Day;
				case "week":
					return DateIntervalEnum.Week;
				case "month":
					return DateIntervalEnum.Month;
				case "quarter":
					return DateIntervalEnum.Quarter;
				case "year":
					return DateIntervalEnum.Year;
			}

			throw new Exception(string.Format("Unknown time interval: {0}", val));
		}

		/// <summary>
		/// Parse an interger in YYYYMMDD format into a DateTime
		/// </summary>
		/// <param name="dayId"></param>
		/// <returns></returns>
		public static DateTime ParseDateId(int dayId)
		{
			var val = DateTime.ParseExact(dayId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
			return val;
		}

		#region Private methods

		private static int? EnsureBounds(int? dateTimeId, DateTime? minDate, DateTime? maxDate)
		{
			if (!dateTimeId.HasValue)
				return dateTimeId;

			if (!minDate.HasValue && !maxDate.HasValue)
				return dateTimeId;

			dateTimeId = EnsureBounds(dateTimeId.Value, minDate, maxDate);
			return dateTimeId;
		}

		private static int EnsureBounds(int dateTimeId, DateTime? minDate, DateTime? maxDate)
		{
			if (!minDate.HasValue && !maxDate.HasValue)
				return dateTimeId;

			var date = DateHelper.FromDateTimeId(dateTimeId);
			date = EnsureBounds(date, minDate, maxDate);
			var dt = DateHelper.ToDateTimeId(date);
			return dt.HasValue ? dt.Value : 0;
		}

		private static DateTime? EnsureBounds(DateTime? date, DateTime? minDate, DateTime? maxDate)
		{
			if (!date.HasValue)
				return date;

			if (!minDate.HasValue && !maxDate.HasValue)
				return date;

			date = EnsureBounds(date.Value, minDate, maxDate);
			return date;
		}

		private static DateTime EnsureBounds(DateTime date, DateTime? minDate, DateTime? maxDate)
		{
			if (!minDate.HasValue && !maxDate.HasValue)
				return date;

			if (minDate.HasValue && date < minDate.Value)
				return minDate.Value;
			if (maxDate.HasValue && date > maxDate.Value)
				return maxDate.Value;

			return date;
		}


		private static IEnumerable<DateTimePart> EachHour(DateTime begin, DateTime end)
		{
			for (var part = begin.Date; part.Date <= end.Date; part = part.AddHours(1))
			{
				var returnValue = new DateTimePart
				{
					Key = part,
					Begin = part,
					End = part.AddHours(1).AddTicks(-1)
				};

				yield return returnValue;
			}
		}

		private static IEnumerable<DateTimePart> EachDay(DateTime begin, DateTime end)
		{
			for (var part = begin.Date; part.Date <= end.Date; part = part.AddDays(1))
			{
				var returnValue = new DateTimePart
				{
					Key = part,
					Begin = part,
					End = part.AddDays(1).AddTicks(-1)
				};

				yield return returnValue;
			}
		}

		private static IEnumerable<DateTimePart> EachWeek(DateTime begin, DateTime end)
		{
			var count = 0;
			for (var part = begin.Date; part.Date <= end.Date; part = part.AddDays(1))
			{
				if (count == 0 || part.DayOfWeek == DayOfWeek.Monday)
				{
					var returnValue = new DateTimePart
					{
						Key = part,
						Begin = part,
						End = part.Next(DayOfWeek.Monday).AddTicks(-1)
					};

					count++;
					yield return returnValue;
				}
			}
		}

		private static IEnumerable<DateTimePart> EachMonth(DateTime begin, DateTime end)
		{
			var count = 0;
			for (var part = begin.Date; part.Date <= end.Date; part = part.AddDays(1))
			{
				if (count == 0 || part.Day == 1)
				{
					var returnValue = new DateTimePart
					{
						Key = part,
						Begin = part,
						End = part.LastDayOfMonth().AddDays(1).AddTicks(-1)
					};

					count++;
					yield return returnValue;
				}
			}
		}

		#endregion
	}

	public class DateTimePart
	{
		public DateTime Key { get; set; }
		public DateTime Begin { get; set; }
		public DateTime End { get; set; }
	}

	public enum DateIntervalEnum
	{
		Second = 1,
		Minute = 2,
		Hour = 3,
		Day = 4,
		Week = 5,
		Month = 6,
		Quarter = 7,
		Year = 8
	}
}
