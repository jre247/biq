using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<Metric> GetMetrics()
		{
			var metrics = new List<Metric>
			{
				new Metric() { Id = 1, Name = "Total Impressions" },
				new Metric() { Id = 2, Name = "Total Clicks" },
				new Metric() { Id = 3, Name = "CTR" },
				new Metric() { Id = 4, Name = "Total Sessions" },
				new Metric() { Id = 5, Name = "Avg. Time Spent" },
				new Metric() { Id = 6, Name = "Total Bounces" },
				new Metric() { Id = 7, Name = "Unique Users" },
				new Metric() { Id = 8, Name = "% Returning Users" },
				new Metric() { Id = 9, Name = "Total Video Views" },
				new Metric() { Id = 10, Name = "Avg. Video Views/Session" },
				new Metric() { Id = 11, Name = "Total Pageviews" },
				new Metric() { Id = 12, Name = "Avg. Pageviews/Session" },
				new Metric() { Id = 13, Name = "Avg. Video Duration" },
				new Metric() { Id = 14, Name = "Video % Avg." },
				new Metric() { Id = 15, Name = "Avg. Pageview Duration" },
				new Metric() { Id = 16, Name = "Interactive Impressions" },
				new Metric() { Id = 17, Name = "Duration Sum" },
				new Metric() { Id = 18, Name = "Frequency" },
				new Metric() { Id = 19, Name = "Unique Impressions" },
				new Metric() { Id = 20, Name = "Qualified Video Views" },
			};
			return metrics;
		}

	}

}