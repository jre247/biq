using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BrightLine.Common.Models.OLAP.Aggregations.App;

namespace BrightLine.OLAP
{
	public partial class OLAPContext : DbContext
	{
		[ThreadStatic]
		private static OLAPContext context;

		public OLAPContext()
			: base(ConfigurationManager.ConnectionStrings["OLAP"].ConnectionString)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<OLAPContext, Migrations.Configuration>());
		}

		public static OLAPContext Current()
		{
			if (context == null)
				context = new OLAPContext();
			return context;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Entity<AggAppDailyPageEventPlatform>()
						.HasKey(o => new { o.DayId, o.DimAppVersionId, o.DimAppVersionEventId });
			modelBuilder.Entity<AggAppHourlyPlatformCity>()
						.HasKey(o => new { o.DimHourId, o.DimAppVersionId, o.DimPlatformVersionId, o.DimCityId });
			modelBuilder.Entity<AggAppHourlyPagePlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimHourId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionPageReferrerId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppHourlyPageEventPlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimHourId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionEventId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppHourlyPageVideoPercentPlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimHourId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionVideoId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppDailyPlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimCityId,
								});
			modelBuilder.Entity<AggAppDailyPagePlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionPageReferrerId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppDailyPageEventPlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionEventId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppDailyPageVideoPercentPlatformCity>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionVideoId,
									o.DimCityId,
								});

			modelBuilder.Entity<AggAppDailyPlatform>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
								});

			modelBuilder.Entity<AggAppDailyPagePlatform>()
						.HasKey(
							o =>
							new
								{
									o.DimDayId,
									o.DimAppVersionId,
									o.DimPlatformVersionId,
									o.DimAppVersionPageId,
									o.DimAppVersionPageReferrerId,
								});

			modelBuilder.Entity<AggAppWeeklyPlatform>()
						.HasKey(
							o =>
							new
								{
									o.DimWeekId,
									o.DimAppVersionId,
									o.DimPlatformVersionId
								});

			modelBuilder.Entity<AggAppWeeklyPagePlatform>()
						.HasKey(
							o =>
							new
							{
								o.DimWeekId,
								o.DimAppVersionId,
								o.DimPlatformVersionId,
								o.DimAppVersionPageId,
								o.DimAppVersionPageReferrerId
							});

			modelBuilder.Entity<AggAppWeeklyPageEventPlatform>()
						.HasKey(
							o =>
							new
							{
								o.DimWeekId,
								o.DimAppVersionId,
								o.DimPlatformVersionId,
								o.DimAppVersionPageId,
								o.DimAppVersionEventId
							});

			modelBuilder.Entity<AggAppWeeklyPlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimWeekId,
								o.DimAppId,
								o.DimPlatformId
							});

			modelBuilder.Entity<AggAppWeeklyPagePlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimWeekId,
								o.DimAppId,
								o.DimPlatformId,
								o.DimAppVersionPageId,
								o.DimAppVersionPageReferrerId
							});

			modelBuilder.Entity<AggAppWeeklyPageEventPlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimWeekId,
								o.DimAppId,
								o.DimPlatformId,
								o.DimAppVersionPageId,
								o.DimAppVersionEventId
							});


			modelBuilder.Entity<AggAppMonthlyPlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimMonthId,
								o.DimAppVersionId,
								o.DimPlatformVersionId,
							});

			modelBuilder.Entity<AggAppMonthlyPagePlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimMonthId,
								o.DimAppId,
								o.DimPlatformId,
								o.DimAppVersionPageId,
								o.DimAppVersionPageReferrerId
							});

			modelBuilder.Entity<AggAppMonthlyPageEventPlatformUnique>()
						.HasKey(
							o =>
							new
							{
								o.DimMonthId,
								o.DimAppId,
								o.DimPlatformId,
								o.DimAppVersionPageId,
								o.DimAppVersionEventId
							});

			base.OnModelCreating(modelBuilder);
		}
	}
}
