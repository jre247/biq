using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Web.Helpers;
using Newtonsoft.Json.Linq;
using BrightLine.Service;
using BrightLine.Common.ViewModels;
using System.Globalization;
using BrightLine.Common.Utility;
using BrightLine.Common.Resources;
using System.Security.Authentication;
using BrightLine.Common.Services;


namespace BrightLine.Web.Areas.Campaigns.Controllers
{
	[RoutePrefix("api/v2/campaigns")]
	[CamelCase]
	public class CampaignAnalyticsApiController : ApiController
	{
		private ICampaignService Campaigns { get;set;}

		public CampaignAnalyticsApiController()
		{
			Campaigns = IoC.Resolve<ICampaignService>();
		}

		/// <summary>
		/// Returns a ChartViewModel in JSON format for a specified campaign, timeframe, and interval
		/// </summary>
		/// <param name="id">The id of the Campaign.</param>
		/// <param name="bd1">The begin date in an integer format YYYYMMDD.</param>
		/// <param name="ed1">The end date in an interger format YYYYMMDD.</param>
		/// <param name="m1">The id of the primary Metric.</param>
		/// <param name="m2">The id of the second Metric</param>
		/// <param name="int">The string interval of the time series.</param>
		/// <param name="filters">The string filters to filter the result set by</param>
		/// <returns>JOBject of ChartViewModel</returns>
		[GET("{id:int}/analytics/chart")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Chart(int id, int? bd1 = null, int? ed1 = null, int? m1 = null, int? m2 = null, string @int = null, string filters = null)
		{
			try
			{
				var campaign = GetCampaignIfAccessible(id);
				
				MetricEnum? primaryMetric = null;
				if (m1.HasValue && Enum.IsDefined(typeof(MetricEnum), m1.Value)) 
				{
					primaryMetric = (MetricEnum)m1.Value;
				}

				MetricEnum? secondaryMetric = null;
				if (m2.HasValue && Enum.IsDefined(typeof(MetricEnum), m2.Value)) 
				{
					secondaryMetric = (MetricEnum)m2.Value;
				}

				var svc = new CampaignAnalyticsService();
				var queryFilters = svc.GetFilters(campaign, bd1, ed1, null, filters, @int, primaryMetric, secondaryMetric);

				var data = svc.GetChartViewModel(queryFilters);

				return JObject.FromObject(data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = CommonResources.GenericErrorMessage });
			}
		}

		[GET("{id:int}/analytics/overview")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Overview(int id, int? bd1 = null, int? ed1 = null, string dims = null, string filters = null)
		{
			try
			{
				var campaign = GetCampaignIfAccessible(id);

				var svc = new CampaignAnalyticsService();
				var queryFilters = svc.GetFilters(campaign, bd1, ed1, dims, filters, null);
				var data = svc.GetOverviewViewModel(queryFilters);

				return JObject.FromObject(data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not retrieve campaign analytics overview.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("{id:int}/analytics/page")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Page(int id, int? bd1 = null, int? ed1 = null, string filters = null) 
		{
			try
			{
				if (!Auth.IsUserInRole(AuthConstants.Roles.Employee))
					throw new AuthenticationException(string.Format("User {0} attempting unauthorized access of Page Analytics", Auth.UserEmail));

				var campaign = GetCampaignIfAccessible(id);

				var svc = new CampaignAnalyticsService();
				var queryFilters = svc.GetFilters(campaign, bd1, ed1, null, filters, null);
				var data = svc.GetFeatureOverview(queryFilters);

				return JObject.FromObject(data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = CommonResources.GenericErrorMessage });
			}
		}

		[GET("{id:int}/analytics/video")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Video(int id, int? bd1 = null, int? ed1 = null, string filters = null)
		{
			try
			{
				if (!Auth.IsUserInRole(AuthConstants.Roles.Employee))
					throw new AuthenticationException(string.Format("User {0} attempting unauthorized access of Video Analytics", Auth.UserEmail));
				
				var campaign = GetCampaignIfAccessible(id);

				var svc = new CampaignAnalyticsService();
				var queryFilters = svc.GetFilters(campaign, bd1, ed1, null, filters, null);

				var data = svc.GetThemeOverview(queryFilters);

				return JObject.FromObject(data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = CommonResources.GenericErrorMessage });
			}
		}

		[GET("{id:int}/videos")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject Videos(int id)
		{
			try
			{
				var campaign = GetCampaignIfAccessible(id);

				var svc = new CampaignAnalyticsService();

				var data = svc.GetVideoViewModels(id);

				var videos = new JObject();
				foreach (var video in data)
				{
					videos[video.VideoKey.ToString()] = JObject.FromObject(new VideoViewModelBase { name = video.Name, totalRunTime = video.Length * 1000 });
				}

				var json = new JObject();
				json["videos"] = videos;
				return json;
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = CommonResources.GenericErrorMessage });
			}
		}

		[GET("{id:int}/analytics/datetime")]
		[AcceptVerbs("GET")]
		[HttpGet]
		public JObject CampaignDateTime(int id, int? bd1 = null, int? ed1 = null, string @int = null)
		{
			try
			{
				var campaign = Campaigns.Get(id);
				if (!Campaigns.IsAccessible(campaign))
					throw new Exception("Campaign inaccessible.");
				
				var svc = new CampaignAnalyticsService();

				var begin = bd1.HasValue ? DateHelper.ParseDateId(bd1.Value) : svc.GetBeginDate(campaign, null);
				var end = ed1.HasValue ? DateHelper.ParseDateId(ed1.Value) : svc.GetEndDate(campaign, null);
				var interval = svc.GetInterval(@int, begin, end);
				var dateParts = DateHelper.GetDatePartsInRange(begin, end, interval);
				var data = new JObject();
				
				data["beginDate"] = begin.ToString();
				data["endDate"] = end.ToString();
				data["interval"] = interval.ToString();
				data["xAxis"] = new JArray(dateParts.Select(o => o.Begin.ToString()).ToArray());
				
				var dps = from l in dateParts
						  group l by l.Key into g
						  let dp = g.First()
						  select new JProperty(dp.Key.ToString(), JObject.FromObject(new DateTimeDayPartViewModel { begin = dp.Begin.ToString(), end = dp.End.ToString() } ));

				data["dateParts"] = new JObject(dps.ToArray());
				return data;
				
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = CommonResources.GenericErrorMessage });
			}
		}

		#region Private Methods
		
		/// <summary>
		/// Gets a Campaign by Id and determines if campaign exists, is accessible, and is viewable in the dashboard
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private Campaign GetCampaignIfAccessible(int id)
		{
			var campaign = Campaigns.Get(id);
			if (!Campaigns.IsAccessible(campaign))
				throw new Exception("Campaign inaccessible.");

			var svc = new CampaignAnalyticsService();

			if (!svc.CampaignAnalyticsAccessible(campaign))
				throw new Exception("Campaign inaccessible.");

			return campaign;
		}

		#endregion
	}

	public class DateTimeDayPartViewModel
	{
		public string begin { get; set; }
		public string end { get; set; }
	}

	public class VideoViewModelBase
	{
		public string name { get; set; }
		public int totalRunTime { get; set; }
	}
}
