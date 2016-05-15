using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility.SqlServer;
using System.Data;
using BrightLine.Common.ViewModels;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using BrightLine.Common.Framework;
using BrightLine.Service.Analytics.Campaign;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Constants;

namespace BrightLine.Service
{
	public class CampaignAnalyticsService : ICampaignAnalyticsService
	{
		private readonly int CACHE_DURATION;
		private CampaignAnalyticsSettings _settings;

		#region Initialization/Finalization
		
		public CampaignAnalyticsService()
		{
			_settings = new CampaignAnalyticsSettings();
			CACHE_DURATION = _settings.CacheMinuteDuration;
		}

		#endregion

		#region public methods

		public ChartViewModel GetChartViewModel(DashboardFilters filters)
		{

			// 1. Get summaries
			var summaries = GetSeries(filters);

			// 2. Filter summaries
			var filtered = BaseAdSummary.ApplyFilters<SeriesSummary>(summaries, filters);

			// 3. Group results by DateTimeKey
			var grouped = filtered.GroupBy(o => o.DateTimeKey).OrderBy(o => o.Key);

			var data = new ChartViewModel
			{
				metric = new Dictionary<string, SeriesMetricDetail>()
			};

			data.metric.Add(((int)filters.PrimaryMetric).ToString(), GetSeriesMetricDetail(filters.PrimaryMetric, grouped));
			data.metric.Add(((int)filters.SecondaryMetric).ToString(), GetSeriesMetricDetail(filters.SecondaryMetric, grouped));

			return data;
		}

		public DimensionResult GetOverviewViewModel(DashboardFilters filters)
		{

			// 1. Get AdSummaries 
			var summaries = GetOverviewSummaries(filters);
			
			// 2. Filter AdSummaries
			var filtered = BaseAdSummary.ApplyFilters<OverviewAdSummary>(summaries, filters);

			// 3. Get list of Grouping combinations
			var initialNode = filters.Groups.GroupNode;
			var nodes = Groups.GetGroupings(initialNode);
			
			// Reverse the nodes so that depedent nodes follow their parents
			nodes.Reverse(); 
			
			// 4. Group the summaries by each grouping combination and store them in a dictionary
			var values = new Dictionary<GroupNode, Dictionary<string, Grouping>>();

			foreach (var node in nodes)
			{
				var result = GroupSummaries(node.GroupKey, filtered);
				values.Add(node, result);
			}
			
			// 5. Convert the dictionary of grouping combinations into a a single recursive DimensionResult 
			var rv = GetDimensionResult(values);
			return rv;
		}

		public DimensionResult GetFeatureOverview(DashboardFilters filters)
		{
			// 1. Get summaries 
			var pageSummaries = GetPageSummaries(filters);

			// 2. Filter summaries
			var filtered = BaseAdSummary.ApplyFilters<PageAdSummary>(pageSummaries, filters);

			var returnValue = new DimensionResult
			{
				dimension = new Dimension
				{
					name = "feature"
				},
				values = new List<DimensionResult>()
			};

			var featurePageGrouping =
				from feature in filtered
				group feature by feature.FeatureKey into featureGroup
				from pageGroup in
					(from page in featureGroup
					 group page by page.PageKey)
				group pageGroup by featureGroup.Key;
			
			foreach (var feature in featurePageGrouping.OrderByDescending(o => o.Sum(f => f.Sum(p => p.PageviewCount))))
			{
				var res = new DimensionResult {
					dimension = new Dimension {
						name = "feature"
					},
					id = feature.Key.ToString(),
					metric = new Dictionary<string, MetricDetail>
					{
						{ "11", new MetricDetail { total = feature.Sum(o => o.Sum(p => p.PageviewCount)) } }, 
						{ "9", new MetricDetail { total = feature.Sum(o => o.Sum(p => p.VideoViewCount)) } }, 
						{ "15", new MetricDetail { total = feature.Sum(o => o.Sum(p => p.PageviewQualifiedCount)) > 0 ? 
							((double)feature.Sum(p => p.Sum(o=> o.PageviewDurationSum) / (double)feature.Sum(m => m.Sum(o => o.PageviewQualifiedCount))))*1000 : 0 } },	
					},
					values = new List<DimensionResult>()
				};

				foreach (var page in feature.OrderByDescending(p => p.Sum(o => o.PageviewCount)))
				{
					res.values.Add(new DimensionResult {
						dimension = new Dimension
						{
							name = "page"
						},
						id = page.Key.ToString(),
						metric = new Dictionary<string, MetricDetail>
						{
							{ "11", new MetricDetail { total = page.Sum(o => o.PageviewCount) } }, 
							{ "9", new MetricDetail { total = page.Sum(o => o.VideoViewCount) } }, 
							{ "15", new MetricDetail { total = page.Sum(o => o.PageviewQualifiedCount) > 0 ? 
							((double)page.Sum(o => o.PageviewDurationSum) / (double)page.Sum(o => o.PageviewQualifiedCount))*1000 : 0 } },	
						}
					});
				}

				returnValue.values.Add(res);
			}

			
			return returnValue;
		}

		public DimensionResult GetThemeOverview(DashboardFilters filters)
		{
			// 1. Get summaries 
			var summaries = GetVideoSummaries(filters);

			// 2. Filter summaries
			var filtered = BaseAdSummary.ApplyFilters<VideoAdSummary>(summaries, filters);

			var featurePageGrouping =
				from theme in filtered
				group theme by theme.Theme into themeGroup
				from videos in
					(from video in themeGroup
					 group video by video.Name)
				group videos by themeGroup.Key;

			var returnValue = new DimensionResult
			{
				dimension = new Dimension
				{
					name = "theme"
				},
				values = new List<DimensionResult>()
			};

			foreach (var theme in filtered.GroupBy(o => o.Theme).OrderByDescending(o => o.Sum(t => t.VideoViewCount)))
			{
				var res = new DimensionResult
				{
					dimension = new Dimension
					{
						name = "theme"
					},
					id = theme.Key,
					metric = new Dictionary<string, MetricDetail>
					{
						{ "9", new MetricDetail { total = theme.Sum(o => o.VideoViewCount)} },	
					},
					values = new List<DimensionResult>()
				};

				var qualifiedCount = theme.Where(o => o.DecileComplete > 0).Sum(o => o.VideoViewCount);
				res.metric.Add("20", new MetricDetail { total = qualifiedCount });
				var weightedPercentComplete = 
					qualifiedCount > 0 
						? ((double)theme.Where(o => o.DecileComplete > 0).Sum(o => o.VideoViewCount * o.DecileComplete) / (double)qualifiedCount) /100 
						: 0;

				res.metric.Add("14", new MetricDetail { total = weightedPercentComplete });

				returnValue.values.Add(res);

				foreach (var video in theme.GroupBy(o => o.VideoKey).OrderByDescending(o => o.Sum(v => v.VideoViewCount)))
				{
					var res2 = new DimensionResult
					{
						id = video.Key.ToString(),
						dimension = new Dimension {
							name = "video"
						},
						metric = new Dictionary<string,MetricDetail>
						{
							{ "9", new MetricDetail { total = video.Sum(o => o.VideoViewCount)} },		   
						}
					};

					qualifiedCount = video.Where(o => o.DecileComplete > 0).Sum(o => o.VideoViewCount);
					res2.metric.Add("20", new MetricDetail { total = qualifiedCount });
					weightedPercentComplete =
						qualifiedCount > 0
							? ((double)video.Where(o => o.DecileComplete > 0).Sum(o => o.VideoViewCount * o.DecileComplete) / (double)qualifiedCount) / 100
							: 0;
					res2.metric.Add("14", new MetricDetail { total = weightedPercentComplete });

					res.values.Add(res2);
				}
			}

			return returnValue;
		}

		public List<VideoViewModel> GetVideoViewModels(int campaignId)
		{
			List<VideoViewModel> results = new List<VideoViewModel>();

			var da = new DataAccess(DataAccessConstants.OLAP_CONNECTION_STRING);
			da.AddParameter("@cId", campaignId, DbType.Int32);

			da.ExecuteReader<VideoViewModel>("[dbo].[CampaignDashboard_Videos]", (summary) => results = summary);

			return results;
		}

		public DashboardFilters GetFilters(Campaign campaign, int? bd1, int? ed1, string dims, string filters, string intervalString, MetricEnum? primaryMetric = null, MetricEnum? secondaryMetric = null)
		{
			var mediaPartners = IoC.Resolve<IMediaPartnerService>();

			if (!Auth.IsUserInRole(AuthConstants.Roles.Employee))
			{
				
				// Restrict Media Parter access to only Ads that belong to the user's Media Partner or children Media Partners
				if (Auth.IsUserInRole(AuthConstants.Roles.MediaPartner)) 
				{
					if (Auth.UserModel.MediaPartner != null)
					{
						var mediaPartnerId = Auth.UserModel.MediaPartner.Id;
						var mediaPartnerIds = mediaPartners.Where(o => o.Parent_Id == mediaPartnerId).Select(o => o.Id).ToArray();

						filters += string.Format(";mediapartner:{0}{1}", mediaPartnerId, mediaPartnerIds.Length > 0 ? string.Format(",{0}", string.Join(",", mediaPartnerIds)) : string.Empty);
					}
				}

				if (!primaryMetric.HasValue)
					primaryMetric = MetricEnum.CTR;
				
				if (!secondaryMetric.HasValue)
					secondaryMetric = MetricEnum.AvgTimeSpent;

				// Restrict Metrics from Agency Partner and Media Partner
				var allowedMetrics = new List<MetricEnum>()
				{
					MetricEnum.InteractiveImpressions,
					MetricEnum.TotalClicks,
					MetricEnum.CTR,
					MetricEnum.AvgTimeSpent,
					MetricEnum.SpotImpressions
				};

				if (!allowedMetrics.Contains(primaryMetric.Value))
					throw new Exception(string.Format("User {0} attempted to access blocked metric: {1}", Auth.UserEmail, primaryMetric.Value));

				if (!allowedMetrics.Contains(secondaryMetric.Value))
					throw new Exception(string.Format("User {0} attempted to access blocked metric: {1}", Auth.UserEmail, secondaryMetric.Value));
			} 
			else
			{
				if (primaryMetric == null)
					primaryMetric = MetricEnum.TotalSessions;
				
				if (secondaryMetric == null)
					secondaryMetric = MetricEnum.AvgTimeSpent;
			}

			// Restrict Platforms dimension/filter to Employee Role
			if (!string.IsNullOrWhiteSpace(dims) && dims.Contains("platform") && !Auth.IsUserInRole(AuthConstants.Roles.Employee))
					throw new Exception(string.Format("User {0} attempting access Platform Dimension", Auth.UserEmail));
			
			if (!string.IsNullOrWhiteSpace(filters) && filters.Contains("platform") && !Auth.IsUserInRole(AuthConstants.Roles.Employee))
					throw new Exception(string.Format("User {0} attempting access Platform Filter", Auth.UserEmail));

			var begin = GetBeginDate(campaign, bd1.HasValue ? DateHelper.ParseDateId(bd1.Value) : (DateTime?)null);
			var end = GetEndDate(campaign, ed1.HasValue ? DateHelper.ParseDateId(ed1.Value) : (DateTime?)null);
			var interval = GetInterval(intervalString, begin, end);

			var returnValue = new DashboardFilters(campaign, begin, end, dims, filters, interval, primaryMetric.Value, secondaryMetric.Value);

			return returnValue;
		}

		public DateTime GetBeginDate(Campaign campaign, DateTime? begin)
		{
			if (begin.HasValue)
			{
				return begin.Value;
			}
			else
			{
				if (campaign.BeginDate.HasValue)
				{
					return campaign.BeginDate.Value;
				}

				throw new Exception("Campaign inaccessible.");
				
			}
		}

		public DateTime GetEndDate(Campaign campaign, DateTime? end)
		{
			var limit = GetAnalyticsWindowFloor();

			if (end.HasValue)
			{
				return end > limit ? limit : end.Value.AddDays(1).AddTicks(-1);
			}

			if (!campaign.EndDate.HasValue)
				return limit;

			if (campaign.EndDate.Value > limit)
				return limit;

			return campaign.EndDate.Value.AddDays(1).AddTicks(-1);
			
		}

		public DateIntervalEnum GetInterval(string dateInterval, DateTime begin, DateTime end)
		{
			if (!string.IsNullOrWhiteSpace(dateInterval))
				return DateHelper.GetIntervalFromString(dateInterval);

			// The interval was not specified or is not a valid DateIntervalEnum
			// Calculate the interval from the time range specified
			var dayCount = end.AddDays(1).Subtract(begin).Days;

			if (dayCount < 2)
			{
				return DateIntervalEnum.Hour;
			}

			if (dayCount > 1 && dayCount < 32)
			{
				return DateIntervalEnum.Day;
			}

			if (dayCount >= 32 && dayCount < 181)
			{
				return DateIntervalEnum.Week;
			}

			return DateIntervalEnum.Month;
		}

		public bool CampaignAnalyticsAccessible(Campaign campaign)
		{
			if (!campaign.BeginDate.HasValue)
				return false;
			return CampaignBeginDateIsBeforeFloor(campaign.BeginDate.Value);
			
		}

		public bool CampaignAnalyticsAccessible(DateTime? campaignBeginDate)
		{
			if (!campaignBeginDate.HasValue)
				return false;
			return CampaignBeginDateIsBeforeFloor(campaignBeginDate.Value);
		}

		#endregion

		#region Private Helpers

		private static Dictionary<string, Grouping> GroupSummaries(string groupByKey, IEnumerable<OverviewAdSummary> summaries)
		{
			var returnValue = new Dictionary<string, Grouping>();
			
			// Dnyamically group the Summaries by a grouping string, where the grouping string 
			// is a comma-separated list of properties wrapped in a new() string
			// e.g. new(PlatformKey,ChannelKey) will group Summaries by Platform and Channel

			var eq = summaries
					.AsQueryable()
					.GroupBy(groupByKey, "it", null)
					.Select("new(it.Key as Key, it as Result)");


			var groupings = (from dynamic dat in eq select dat).ToList();

			foreach (var group in groupings)
			{
				var key = group.Key.ToString();
				var g = group.Result as IEnumerable<OverviewAdSummary>;

				var ov = new Grouping
				{
					metric = new Dictionary<string, MetricDetail>
							{
								{ "21", new MetricDetail { total = g.Sum(m => m.SpotImpressionCount) } },
								{ "16", new MetricDetail { total = g.Sum(m => m.InteractiveImpressionCount) } },
								{ "2", new MetricDetail { total = g.Sum(m => m.ClickCount) } },
								{ "17", new MetricDetail { total = g.Sum(m => m.SessionDurationSum) } },
								{ "3", new MetricDetail { total = g.Sum(m => m.InteractiveImpressionCount) > 0 ? (double)g.Sum(m => m.ClickCount) / (double)g.Sum(m => m.InteractiveImpressionCount) : 0 } },
								{ "4", new MetricDetail { total = g.Sum(m => m.SessionCount) } }, 
								{ "5", new MetricDetail { total = g.Sum(m => m.SessionCount) > 0 ? ((double)g.Sum(m => m.SessionDurationSum) / (double)g.Sum(m => m.SessionCount))*1000 : 0 } },
								{ "9", new MetricDetail { total = g.Sum(m => m.VideoViewCount) } }, 
								{ "11", new MetricDetail { total = g.Sum(m => m.PageviewCount) } }, 
								{ "12", new MetricDetail { total = g.Sum(m => m.SessionCount) > 0 ? ((double)g.Sum(m => m.PageviewCount) / (double)g.Sum(m => m.SessionCount)) : 0 } },
								{ "10", new MetricDetail { total = g.Sum(m => m.SessionCount) > 0 ? ((double)g.Sum(m => m.VideoViewCount) / (double)g.Sum(m => m.SessionCount)) : 0 } },
								{ "22", new MetricDetail { total = g.Sum(m => m.QualifiedVideoViewCount) > 0 ? ((double)g.Sum(m => m.CompletedVideoViewCount) / (double)g.Sum(m => m.QualifiedVideoViewCount)) : 0 } },
							}
				};

				returnValue.Add(key, ov);
			}

			return returnValue;
		}

		private DimensionResult GetDimensionResult(Dictionary<GroupNode,Dictionary<string, Grouping>> nodeResults)
		{
			DimensionResult returnValue = null;
			
			foreach (var kvp in nodeResults)
			{
				var node = kvp.Key;
				var items = kvp.Value;
				DimensionResult resultToAssign = null;

				if (returnValue == null)
				{
					returnValue = new DimensionResult();
					resultToAssign = returnValue;
					PopulateResult(resultToAssign, node, items);
				} 
				else
				{
					// foreach in items, find parent value with the current item's parent dimension/key

					foreach (var item in items)
					{
						var groupParts = item.Key.Split(' ');
						var rootNodes = groupParts.Take(groupParts.Count() - 1).ToArray();
						var rootKey = string.Join(" ", rootNodes);

						resultToAssign = GetGroupingNode(returnValue, item.Key);
						PopulateResult(resultToAssign, node, items.Where(o => o.Key.Contains(rootKey)).ToDictionary( o => o.Key, s => s.Value));
					}
				}
			}

			return returnValue;
		}

		private DimensionResult GetGroupingNode(DimensionResult tree, string parentKeys)
		{
			// {CampaignKey=20198, ChannelKey=47}
			// Split on "," and discard last value (current key)
			var rawKeys = parentKeys.Split(',');
			var final = rawKeys.Take(rawKeys.Count() - 1).ToArray();
			var searchParts = new Dictionary<string, string>();

			foreach (var key in final)
			{
				//CampaignKey=20198
				var parts = key.Split(new[] { "Key" }, StringSplitOptions.None); //=> ['Campaign','Key=20198']
				var dimension = parts[0].Replace("{", string.Empty).Trim();
				var id = parts[1].Split(new[] { "=" }, StringSplitOptions.None)[1].Trim();
				searchParts.Add(dimension, id);
			}

			DimensionResult point = tree;

			foreach (var dim in searchParts)
			{
				point = point.values.Where(o => o.dimension.name == dim.Key && o.id == dim.Value).First();
			}

			return point;
		}

		/// <summary>
		/// Populates the Dimension and Values of a DimensionResult
		/// </summary>
		/// <param name="result"></param>
		/// <param name="node"></param>
		/// <param name="groupingResults"></param>
		private void PopulateResult(DimensionResult result, GroupNode node, Dictionary<string, Grouping> groupingResults)
		{
			var dimName = node.DimName;
			var dimension = new Dimension
			{
				name = node.DimName
			};

			if (result.dimension == null)
			{
				result.dimension = dimension;
			}
			
			
			var dimValues = new List<DimensionResult>();

			foreach (var val in groupingResults)
			{
				var rawKey = val.Key;
				var keyValues = rawKey.Split(new[] { dimName }, StringSplitOptions.None);
				var roughkey = keyValues[keyValues.Length - 1]; //=> Key=25}
				var id = roughkey.Replace("Key=", string.Empty).Replace("}", string.Empty);


				dimValues.Add(new DimensionResult
				{
					dimension = dimension,
					id = id,
					metric = val.Value.metric
				});
			}

			result.values = dimValues;
			
		}


		private SeriesMetricDetail GetSeriesMetricDetail(MetricEnum metric, IOrderedEnumerable<IGrouping<int, SeriesSummary>> grouped)
		{
			var returnValue = new SeriesMetricDetail();

			if (metric == MetricEnum.InteractiveImpressions)
			{
				returnValue = new SeriesMetricDetail
					{
						total = grouped.Select(o => o.Sum(s => s.InteractiveImpressionCount)),
						values = grouped.Select(o => o.Sum(s => s.InteractiveImpressionCount)).Cast<object>().ToArray()
					};

			} 
			else if (metric == MetricEnum.SpotImpressions)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(o => o.Sum(s => s.SpotImpressionCount)),
					values = grouped.Select(o => o.Sum(s => s.SpotImpressionCount)).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.TotalClicks)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(o => o.Sum(s => s.ClickCount)),
					values = grouped.Select(o => o.Sum(s => s.ClickCount)).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.TotalSessions)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(o => o.Sum(s => s.SessionCount)),
					values = grouped.Select(o => o.Sum(s => s.SessionCount)).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.TotalPageviews)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(o => o.Sum(s => s.PageviewCount)),
					values = grouped.Select(o => o.Sum(s => s.PageviewCount)).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.TotalVideoViews)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(o => o.Sum(s => s.VideoViewCount)),
					values = grouped.Select(o => o.Sum(s => s.VideoViewCount)).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.CTR)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Sum(s => s.Sum(m => m.InteractiveImpressionCount) > 0 ? (double)s.Sum(m => m.ClickCount) / (double)s.Sum(m => m.InteractiveImpressionCount) : 0),
					values = grouped.Select(s => s.Sum(m => m.InteractiveImpressionCount) > 0 ? (double)s.Sum(m => m.ClickCount) / (double)s.Sum(m => m.InteractiveImpressionCount) : 0).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.AvgTimeSpent)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? ((double)s.Sum(m => m.SessionDurationSum) / (double)s.Sum(m => m.SessionCount)) * 1000 : 0),
					values = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? ((double)s.Sum(m => m.SessionDurationSum) / (double)s.Sum(m => m.SessionCount)) * 1000 : 0).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.AvgPageviewsSession)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? (double)s.Sum(m => m.PageviewCount) / (double)s.Sum(m => m.SessionCount) : 0),
					values = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? (double)s.Sum(m => m.PageviewCount) / (double)s.Sum(m => m.SessionCount) : 0).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.AvgVideoViewsSession)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? (double)s.Sum(m => m.VideoViewCount) / (double)s.Sum(m => m.SessionCount) : 0),
					values = grouped.Select(s => s.Sum(m => m.SessionCount) > 0 ? (double)s.Sum(m => m.VideoViewCount) / (double)s.Sum(m => m.SessionCount) : 0).Cast<object>().ToArray()
				};
			}
			else if (metric == MetricEnum.AvgPageviewDuration)
			{
				returnValue = new SeriesMetricDetail
				{
					total = grouped.Select(s => s.Sum(m => m.PageviewQualifiedCount) > 0 ? ((double)s.Sum(m => m.PageviewDurationSum) / (double)s.Sum(m => m.PageviewQualifiedCount)) * 1000 : 0),
					values = grouped.Select(s => s.Sum(m => m.PageviewQualifiedCount) > 0 ? ((double)s.Sum(m => m.PageviewDurationSum) / (double)s.Sum(m => m.PageviewQualifiedCount)) * 1000 : 0).Cast<object>().ToArray()
				};
			}
			else { }

			return returnValue;
		}

		private DateTime GetAnalyticsWindowFloor()
		{
			// Data from the previous day canont be access until 6AM EST
			var tz = TimeZoneInfo.FindSystemTimeZoneById(_settings.HourFloorTimezone);
			var nowTz = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
			var limit = nowTz.Hour > _settings.HourFloor ? DateTime.UtcNow.Date.AddTicks(-1) : DateTime.UtcNow.Date.AddDays(_settings.DayDelay).AddDays(1).AddTicks(-1);
			return limit;
		}


		private bool CampaignBeginDateIsBeforeFloor(DateTime beginDate)
		{
			var limit = GetAnalyticsWindowFloor();

			if (beginDate > limit)
				return false;

			return true;
		}

		#endregion

		#region private Data Access

		private static DataAccess AddFilters(DashboardFilters filters, bool includeInterval = false)
		{
			var da = new DataAccess(DataAccessConstants.OLAP_CONNECTION_STRING);
			da.AddParameter("@campaignId", filters.Campaign.Id, DbType.Int32);
			da.AddParameter("@beginDate", filters.BeginDate, DbType.DateTime);
			da.AddParameter("@endDate", filters.EndDate, DbType.DateTime);
			if (includeInterval)
				da.AddParameter("@interval", filters.DateInterval, DbType.String);

			return da;
		}

		private List<OverviewAdSummary> GetOverviewSummaries(DashboardFilters filters)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var key = string.Format("Campaign_{0}_Analytics_Overview_{1}_{2}", filters.Campaign.Id, filters.BeginDate.ToString("yyyyMMdd"), filters.EndDate.ToString("yyyyMMdd"));
			List<OverviewAdSummary> results = null;

			if (settings.CachingEnabled)
			{
				results = IoC.Cache.Get<List<OverviewAdSummary>>(key);
				if (results != null)
				{
					return results;
				}
			}

			var da = AddFilters(filters);
			results = new List<OverviewAdSummary>();
			da.ExecuteReader<OverviewAdSummary>("[dbo].[CampaignDashboard_Overview]", (summary) => results = summary);

			if (settings.CachingEnabled)
			{
				IoC.Cache.Add(key, results, TimeSpan.FromMinutes(CACHE_DURATION));
			}
			return results;
		}

		private List<PageAdSummary> GetPageSummaries(DashboardFilters filters)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var key = string.Format("Campaign_{0}_Analytics_Page_{1}_{2}", filters.Campaign.Id, filters.BeginDate.ToString("yyyyMMdd"), filters.EndDate.ToString("yyyyMMdd"));
			List<PageAdSummary> results = null;

			if (settings.CachingEnabled)
			{
				results = IoC.Cache.Get<List<PageAdSummary>>(key);
				if (results != null)
				{
					return results;
				}
			}

			var da = AddFilters(filters);
			results = new List<PageAdSummary>();
			da.ExecuteReader<PageAdSummary>("[dbo].[CampaignDashboard_Page]", (summary) => results = summary);

			if (settings.CachingEnabled)
			{
				IoC.Cache.Add(key, results, TimeSpan.FromMinutes(CACHE_DURATION));
			}
			return results;
		}

		private List<VideoAdSummary> GetVideoSummaries(DashboardFilters filters)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var key = string.Format("Campaign_{0}_Analytics_Video_{1}_{2}", filters.Campaign.Id, filters.BeginDate.ToString("yyyyMMdd"), filters.EndDate.ToString("yyyyMMdd"));
			List<VideoAdSummary> results = null;

			if (settings.CachingEnabled)
			{
				results = IoC.Cache.Get<List<VideoAdSummary>>(key);
				if (results != null)
				{
					return results;
				}
			}

			var da = AddFilters(filters);
			results = new List<VideoAdSummary>();
			da.ExecuteReader<VideoAdSummary>("[dbo].[CampaignDashboard_Video]", (summary) => results = summary);

			if (settings.CachingEnabled)
			{
				IoC.Cache.Add(key, results, TimeSpan.FromMinutes(CACHE_DURATION));
			}
			return results;
		}

		private List<SeriesSummary> GetSeries(DashboardFilters filters)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var key = string.Format("Campaign_{0}_Analytics_Series_{1}_{2}_{3}", filters.Campaign.Id, filters.BeginDate.ToString("yyyyMMdd"), filters.EndDate.ToString("yyyyMMdd"), filters.DateInterval);
			List<SeriesSummary> results = null;

			if (settings.CachingEnabled)
			{
				results = IoC.Cache.Get<List<SeriesSummary>>(key);
				if (results != null)
				{
					return results;
				}
			}

			var da = AddFilters(filters, true);
			results = new List<SeriesSummary>();
			da.ExecuteReader<SeriesSummary>("[dbo].[CampaignDashboard_Series]", (summary) => results = summary);

			if (settings.CachingEnabled)
			{
				IoC.Cache.Add(key, results, TimeSpan.FromMinutes(CACHE_DURATION));
			}
			return results;
		}

		#endregion
	}
}
