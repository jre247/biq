using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class SpaService
	{
		public static Dictionary<string, Dictionary<string, object>> GetLookups()
		{
			var lookups = new Dictionary<string, Dictionary<string, object>>();

			var metrics = GetMetricsLookups();

			lookups.Add("metrics", metrics);

			return lookups;
		}

		private static Dictionary<string, object> GetMetricsLookups()
		{
			var metrics = new Dictionary<string, object>();
			foreach (var metric in Lookups.Metrics.HashById)
			{
				var metricLookup = new
				{
					id = metric.Key,
					name = metric.Value
				};

				var metricKey = metric.Value.Replace(" ", "").Replace(".", "").Replace("/", "Per");

				metrics.Add(metricKey, metricLookup);
			}
			return metrics;
		}
	}
}
