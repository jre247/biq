using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Helpers
{
	public static class CmsInstanceFieldValueHelper
	{
		/// <summary>
		/// This method will convert a field value JToken to a specific DateTime format
		/// </summary>
		/// <param name="date"></param>
		/// <param name="dateFormat"></param>
		/// <returns></returns>
		public static string FormatDateString(JToken date, string dateFormat)
		{
			var dateAsString = date.ToObject<string>();
			var newDate = DateTime.Parse(dateAsString);

			return newDate.ToString(dateFormat);
		}

		/// <summary>
		/// This method will convert a field value DateTime to a specific DateTime format
		/// </summary>
		/// <param name="date"></param>
		/// <param name="dateFormat"></param>
		/// <returns></returns>
		public static string FormatDateString(DateTime date, string dateFormat)
		{
			return date.ToString(dateFormat);
		}
	}
}
