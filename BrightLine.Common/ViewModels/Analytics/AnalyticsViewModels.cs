using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public enum MetricEnum
	{
		TotalClicks	= 2,
		CTR	= 3,
		TotalSessions = 4,
		AvgTimeSpent = 5,
		TotalBounces = 6,
		UniqueUsers = 7,
		ReturningUsers = 8,
		TotalVideoViews	=9,
		AvgVideoViewsSession = 10,
		TotalPageviews = 11,
		AvgPageviewsSession = 12,
		AvgVideoDuration = 13,
		VideoAvg = 14,
		AvgPageviewDuration	= 15,
		InteractiveImpressions = 16,
		DurationSum	= 17,
		Frequency = 18,
		UniqueImpressions = 19,
		QualifiedVideoViews = 20,
		SpotImpressions = 21
	}

	public class DashboardFilters
	{

		#region Properties
		
		public Campaign Campaign { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateIntervalEnum DateInterval { get; set; }
		public Groups Groups { get; set; }
		public MetricEnum PrimaryMetric { get; set; }
		public MetricEnum SecondaryMetric { get; set; }
		public Dictionary<string, DimensionFilter> Filters { get; set; }

		#endregion

		#region Initialization/Finalization

		public DashboardFilters(Campaign campaign, DateTime begin, DateTime end, string dims, string filters, DateIntervalEnum dateInterval, MetricEnum primaryMetric, MetricEnum secondaryMetric)
		{
			Campaign = campaign;
			BeginDate = begin;
			EndDate = end;
			DateInterval = dateInterval;
			PrimaryMetric = primaryMetric;
			SecondaryMetric = secondaryMetric;
			SetDimensionsAndFilters(dims, filters);

			Validate();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Given a string of dimensions and filters, parse each dimension and filter and return a GroupAndFilter object
		/// </summary>
		/// <param name="dimStrings"></param>
		/// <returns></returns>
		private void SetDimensionsAndFilters(string dimStrings, string filterString)
		{
			var dimLookUp = new Dictionary<string, DimensionFilter>
			{
				{ "campaign", new DimensionFilter { Name = "Campaign", Key = "CampaignKey", Selector = summary => summary.CampaignKey } },
				{ "ad", new DimensionFilter { Name = "Ad", Key = "AdKey", Selector = summary => summary.AdKey } },
				
				{ "mediapartner", new DimensionFilter { Name = "MediaPartner", Key = "MediaPartnerKey", Selector = summary => summary.MediaPartnerKey } },
				{ "platform", new DimensionFilter { Name = "Platform", Key = "PlatformKey", Selector = summary => summary.PlatformKey } },
				{ "deliverygroup", new DimensionFilter { Name = "DeliveryGroup", Key = "DeliveryGroupKey", Selector = summary => summary.DeliveryGroupKey.HasValue ? summary.DeliveryGroupKey.Value : 0 } },
				{ "app", new DimensionFilter { Name = "App", Key = "AppKey", Selector = summary => summary.AppKey.HasValue ? summary.AppKey.Value : 0 } },
				{ "creative", new DimensionFilter { Name = "Creative", Key = "CreativeKey", Selector = summary => summary.CreativeKey } },
				{ "placement", new DimensionFilter { Name = "Placement", Key = "PlacementKey", Selector = summary => summary.PlacementKey } },
				{ "category", new DimensionFilter { Name = "Category", Key = "CategoryKey", Selector = summary => summary.CategoryKey } },


				{ "adtype", new DimensionFilter { Name = "AdType", Key = "AdTypeKey", Selector = summary => summary.AdTypeKey } },
				{ "adtypegroup", new DimensionFilter { Name = "AdTypeGroup", Key = "AdTypeGroupKey", Selector = summary => summary.AdTypeGroupKey } },
			};

			SetFilters(filterString, dimLookUp);
			SetDimensions(dimStrings, dimLookUp);
		}

		private void SetDimensions(string dims, Dictionary<string, DimensionFilter> dimLookUp)
		{
			try
			{
				this.Groups = new Groups();
				var groupList = new Dictionary<string, DimensionFilter>();

				// Parse dimensions string
				// Example: dims=platform;mediapartner;deliverygroup
				if (string.IsNullOrEmpty(dims)) return;

				var dimensions = dims.ToLower().Split(';');

				foreach (var key in dimensions)
				{
					if (string.IsNullOrEmpty(key)) continue;

					// If the dimension is unknown, ignore it
					if (!dimLookUp.Keys.Contains(key)) continue;

					// If the dimension has already been added, ignore it
					if (groupList.ContainsKey(key)) continue;

					groupList.Add(key, dimLookUp[key]);

					var groupKey = string.Format("new ({0})", string.Join(", ", Array.ConvertAll(groupList.Select(o => o.Value.Key).ToArray(), i => i.ToString())));

					var node = new GroupNode
					{
						GroupKey = groupKey,
						DimName = dimLookUp[key].Name
					};

					if (this.Groups.GroupNode != null)
					{
						var parent = this.Groups.GroupNode;
						node.ParentNode = parent;
						node.ParentGroupKey = parent.GroupKey;
						this.Groups.GroupNode = node;
					}
					else
					{
						this.Groups.GroupNode = node;
					}
				}
			}
			catch (Exception ex)
			{
				//TODO: Log exception
				//IoC.Log.Error(ex);
				throw;
			}
		}

		private void SetFilters(string filterString, Dictionary<string, DimensionFilter> dimLookUp)
		{
			try
			{
				this.Filters = new Dictionary<string, DimensionFilter>();
				// Parse filters string
				// Example: filters=platform:21,34;mediapartner:12,13;deliverygroup:10002
				if (string.IsNullOrEmpty(filterString)) return;

				var filters = filterString.Split(';');

				foreach (var filter in filters)
				{
					if (string.IsNullOrEmpty(filter)) continue;

					var parts = filter.Split(':');
					var key = parts[0].Trim().ToLower(); //name of dimensions to filter

					// If the dimension is unknown, ignore it
					if (!dimLookUp.Keys.Contains(key)) continue;

					// If the filter has already been added, ignore it
					if (this.Filters.ContainsKey(key)) continue;

					// Check if the dimension being filtered
					string filterIds = null;
					if (parts.Length == 2 && !string.IsNullOrEmpty(parts[1]))
					{
						filterIds = parts[1];

						// If there are ids to filter one, parse them and add the filter record 
						if (filterIds != null)
						{
							var values = new List<int>();

							var filterVals = filterIds.Split(',');

							foreach (var filterVal in filterVals)
							{
								int filterAsInt;
								if (!Int32.TryParse(filterVal, out filterAsInt)) continue;

								if (!values.Contains(filterAsInt))
									values.Add(filterAsInt);
							}

							if (values.Any())
							{
								this.Filters.Add(key, dimLookUp[key]);
								this.Filters[key].FilterIds = values;
							}
						}

						//TODO: security for dimensions/role
					}

				}
			}
			catch (Exception ex)
			{
				//TODO: Log exception
				//IoC.Log.Error(ex);
				throw;
			}
		}

		private void Validate()
		{
			
		}

		#endregion		

	}

	public class GroupNode
	{
		public GroupNode ParentNode { get; set; }
		public string ParentGroupKey { get; set; }
		/// <summary>
		/// String used for Dynamic grouping
		/// </summary>
		public string GroupKey { get; set; }
		public string DimName { get; set; }
	}

	public class Groups
	{
		public string Grouping { get; set; }
		public GroupNode GroupNode { get; set; }

		/// <summary>
		/// Returns list of GroupKeys from a hierarchical structure of GroupNodes
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static List<GroupNode> GetGroupings(GroupNode node)
		{
			var values = new List<GroupNode>();
			values = GetGroupings(node, values);
			return values; 
		}

		private static List<GroupNode> GetGroupings(GroupNode node, List<GroupNode> values)
		{

			values.Add(node);
			

			if (node.ParentNode == null)
				return values;

			return GetGroupings(node.ParentNode, values);
		}
	}

	public class MetricDetail
	{
		public object total { get; set; }
	}

	public class SeriesMetricDetail : MetricDetail
	{
		public object[] values { get; set; }
	}

	public class Grouping
	{
		public string id { get; set; }
		public Dictionary<string, MetricDetail> metric { get; set; }
		public DimensionResult grouping { get; set;}
	}

	public class SeriesGrouping
	{
		public Dictionary<string, SeriesMetricDetail> metric { get; set; }
	}

	public class ChartViewModel : SeriesGrouping
	{
	}

	public class VideoSummary : Grouping
	{
		public int? totalRunTime { get; set; }
	}

	public class FeatureSummary : Grouping
	{
		public Dictionary<string, Grouping> page { get; set; }
	}

	public class ThemeSummary : Grouping
	{
		public Dictionary<string, VideoSummary> video { get; set; }
	}

	public class DimensionFilter
	{
		public string Name { get; set; }
		public string Key { get; set; }
		public List<int> FilterIds { get; set; }
		public Func<BaseAdSummary, int> Selector { get; set; }
	}

	public class SeriesSummary : OverviewAdSummary
	{
		public int DateTimeKey { get; set; }
	}

	public class BaseAdSummary
	{
		public int CampaignKey { get; set; }
		public int AdKey { get; set; }
		public int? DeliveryGroupKey { get; set; }
		public int PlatformKey { get; set; }
		public int MediaPartnerKey { get; set; }
		public int? AppKey { get; set; }
		public int CreativeKey { get; set; }
		public int PlacementKey { get; set; }
		public int CategoryKey { get; set; }
		public int AdTypeKey { get; set; }
		public int AdTypeGroupKey { get; set; }

		/// <summary>
		/// For each dimension, filter an IEnumerable of BaseAdSummary by a list of Ids (integers)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="val"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static IEnumerable<T> ApplyFilters<T>(IEnumerable<T> val, DashboardFilters filters) where T : BaseAdSummary
		{
			foreach (var filter in filters.Filters)
			{
				if (filter.Value.FilterIds != null && filter.Value.FilterIds.Any())
				{

					Func<T, bool> predicate = s => filter.Value.FilterIds.Contains(filter.Value.Selector(s));
					val = val.Where(predicate);
				}
			}

			return val;
		}
	}

	public class OverviewAdSummary : BaseAdSummary
	{
		public int InteractiveImpressionCount { get; set; }
		public int SpotImpressionCount { get; set; }
		public int ClickCount { get; set; }
		public int SessionCount { get; set; }
		public int BounceSessionCount { get; set; }
		public int SessionDurationSum { get; set; }
		public int UniqueDeviceCount { get; set; }
		public int PageviewCount { get; set; }
		public int PageviewQualifiedCount { get; set; }
		public int PageviewDurationSum { get; set; }
		public int VideoViewCount { get; set; }
		public int QualifiedVideoViewCount { get; set; }
		public int CompletedVideoViewCount { get; set; }
	}

	public class PageAdSummary : OverviewAdSummary
	{
		public int PageKey { get; set; }
		public int FeatureKey { get; set; }
	}

	public class VideoAdSummary : OverviewAdSummary
	{
		public int VideoKey { get; set; }
		public string Name { get; set; }
		public string Theme { get; set; }
		public int DecileComplete { get; set; }
		public int VideoViewCount { get; set; }
	}

	public class Dimension
	{
		public string name { get; set; }
	}

	public class DimensionResult
	{
		public string id { get; set; }
		public Dimension dimension { get; set; }
		public Dictionary<string, MetricDetail> metric { get; set; }
		public List<DimensionResult> values { get; set; }
	}

	public class VideoViewModel
	{
		public int VideoKey { get; set; }
		public string Name { get; set; }
		public int Length { get; set; }
	}
}
