using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.ViewModels;
using BrightLine.Common.Utility;
using BrightLine.Common.Models;

namespace BrightLine.Common.Services
{
	public interface ICampaignAnalyticsService
	{
		DashboardFilters GetFilters(Campaign campaign, int? bd1, int? bd2, string dims, string filters, string interval = null, 
			MetricEnum? primaryMetric = null, MetricEnum? secondaryMetric = null);
		ChartViewModel GetChartViewModel(DashboardFilters filters);
		DimensionResult GetOverviewViewModel(DashboardFilters filters);
		DimensionResult GetFeatureOverview(DashboardFilters filters);
		DimensionResult GetThemeOverview(DashboardFilters filters);
		List<VideoViewModel> GetVideoViewModels(int campaignId);
		DateTime GetEndDate(Campaign campaign, DateTime? end);
		DateTime GetBeginDate(Campaign campaign, DateTime? begin);
		DateIntervalEnum GetInterval(string dateInterval, DateTime begin, DateTime end);
		bool CampaignAnalyticsAccessible(Campaign campaign);
		bool CampaignAnalyticsAccessible(DateTime? campaignBeginDate);
	}
}
