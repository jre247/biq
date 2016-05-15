 
using System.Data.Entity;
using BrightLine.Common.Models;
using BrightLine.Common.Models.OLAP;
using BrightLine.Common.Models.OLAP.Dimensions;
using BrightLine.Common.Models.OLAP.Aggregations.Ad;
using BrightLine.Common.Models.OLAP.Aggregations.App;
using BrightLine.Common.Models.OLAP.Aggregations.Destination;

namespace BrightLine.OLAP
{
    public partial class OLAPContext
    {
        
		#region DbSets
		public DbSet<DimApp> DimApps { get; set; }
		public DbSet<DimAppVersion> DimAppVersions { get; set; }
		public DbSet<DimAppVersionEvent> DimAppVersionEvents { get; set; }
		public DbSet<DimAppVersionPage> DimAppVersionPages { get; set; }
		public DbSet<DimAppVersionVideo> DimAppVersionVideos { get; set; }
		public DbSet<DimCity> DimCitys { get; set; }
		public DbSet<DimDay> DimDays { get; set; }
		public DbSet<DimHour> DimHours { get; set; }
		public DbSet<DimMonth> DimMonths { get; set; }
		public DbSet<DimPlatform> DimPlatforms { get; set; }
		public DbSet<DimPlatformVersion> DimPlatformVersions { get; set; }
		public DbSet<DimPublisher> DimPublishers { get; set; }
		public DbSet<DimWeek> DimWeeks { get; set; }
		public DbSet<AggAd> AggAds { get; set; }
		public DbSet<AggAppDailyPageEventPlatform> AggAppDailyPageEventPlatforms { get; set; }
		public DbSet<AggAppDailyPageEventPlatformCity> AggAppDailyPageEventPlatformCitys { get; set; }
		public DbSet<AggAppDailyPagePlatform> AggAppDailyPagePlatforms { get; set; }
		public DbSet<AggAppDailyPagePlatformCity> AggAppDailyPagePlatformCitys { get; set; }
		public DbSet<AggAppDailyPageVideoPercentPlatformCity> AggAppDailyPageVideoPercentPlatformCitys { get; set; }
		public DbSet<AggAppDailyPlatform> AggAppDailyPlatforms { get; set; }
		public DbSet<AggAppDailyPlatformCity> AggAppDailyPlatformCitys { get; set; }
		public DbSet<AggAppHourlyPageEventPlatformCity> AggAppHourlyPageEventPlatformCitys { get; set; }
		public DbSet<AggAppHourlyPagePlatformCity> AggAppHourlyPagePlatformCitys { get; set; }
		public DbSet<AggAppHourlyPageVideoPercentPlatformCity> AggAppHourlyPageVideoPercentPlatformCitys { get; set; }
		public DbSet<AggAppHourlyPlatformCity> AggAppHourlyPlatformCitys { get; set; }
		public DbSet<AggAppMonthlyPageEventPlatformUnique> AggAppMonthlyPageEventPlatformUniques { get; set; }
		public DbSet<AggAppMonthlyPagePlatformUnique> AggAppMonthlyPagePlatformUniques { get; set; }
		public DbSet<AggAppMonthlyPlatformUnique> AggAppMonthlyPlatformUniques { get; set; }
		public DbSet<AggAppWeeklyPageEventPlatform> AggAppWeeklyPageEventPlatforms { get; set; }
		public DbSet<AggAppWeeklyPageEventPlatformUnique> AggAppWeeklyPageEventPlatformUniques { get; set; }
		public DbSet<AggAppWeeklyPagePlatform> AggAppWeeklyPagePlatforms { get; set; }
		public DbSet<AggAppWeeklyPagePlatformUnique> AggAppWeeklyPagePlatformUniques { get; set; }
		public DbSet<AggAppWeeklyPlatform> AggAppWeeklyPlatforms { get; set; }
		public DbSet<AggAppWeeklyPlatformUnique> AggAppWeeklyPlatformUniques { get; set; }
		public DbSet<AggDestination> AggDestinations { get; set; }
		#endregion
    }
}