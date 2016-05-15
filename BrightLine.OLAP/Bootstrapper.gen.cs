
using BrightLine.Common.Models.OLAP.Aggregations.Ad;
using BrightLine.Common.Models.OLAP.Aggregations.App;
using BrightLine.Common.Models.OLAP.Aggregations.Destination;
using BrightLine.Common.Models.OLAP.Dimensions;
using BrightLine.Common.Data;
using SimpleInjector;

namespace BrightLine.OLAP
{
    public static partial class Bootstrapper
    {
        public static void InitializeContainer(Container container)
        {
			#region CrudRepository Registrations
			container.Register<IAggregationRepository<AggAd>, AggregationRepository<AggAd>>();
			container.Register<IAggregationRepository<AggAppDailyPageEventPlatform>, AggregationRepository<AggAppDailyPageEventPlatform>>();
			container.Register<IAggregationRepository<AggAppDailyPageEventPlatformCity>, AggregationRepository<AggAppDailyPageEventPlatformCity>>();
			container.Register<IAggregationRepository<AggAppDailyPagePlatform>, AggregationRepository<AggAppDailyPagePlatform>>();
			container.Register<IAggregationRepository<AggAppDailyPagePlatformCity>, AggregationRepository<AggAppDailyPagePlatformCity>>();
			container.Register<IAggregationRepository<AggAppDailyPageVideoPercentPlatformCity>, AggregationRepository<AggAppDailyPageVideoPercentPlatformCity>>();
			container.Register<IAggregationRepository<AggAppDailyPlatform>, AggregationRepository<AggAppDailyPlatform>>();
			container.Register<IAggregationRepository<AggAppDailyPlatformCity>, AggregationRepository<AggAppDailyPlatformCity>>();
			container.Register<IAggregationRepository<AggAppHourlyPageEventPlatformCity>, AggregationRepository<AggAppHourlyPageEventPlatformCity>>();
			container.Register<IAggregationRepository<AggAppHourlyPagePlatformCity>, AggregationRepository<AggAppHourlyPagePlatformCity>>();
			container.Register<IAggregationRepository<AggAppHourlyPageVideoPercentPlatformCity>, AggregationRepository<AggAppHourlyPageVideoPercentPlatformCity>>();
			container.Register<IAggregationRepository<AggAppHourlyPlatformCity>, AggregationRepository<AggAppHourlyPlatformCity>>();
			container.Register<IAggregationRepository<AggAppMonthlyPageEventPlatformUnique>, AggregationRepository<AggAppMonthlyPageEventPlatformUnique>>();
			container.Register<IAggregationRepository<AggAppMonthlyPagePlatformUnique>, AggregationRepository<AggAppMonthlyPagePlatformUnique>>();
			container.Register<IAggregationRepository<AggAppMonthlyPlatformUnique>, AggregationRepository<AggAppMonthlyPlatformUnique>>();
			container.Register<IAggregationRepository<AggAppWeeklyPageEventPlatform>, AggregationRepository<AggAppWeeklyPageEventPlatform>>();
			container.Register<IAggregationRepository<AggAppWeeklyPageEventPlatformUnique>, AggregationRepository<AggAppWeeklyPageEventPlatformUnique>>();
			container.Register<IAggregationRepository<AggAppWeeklyPagePlatform>, AggregationRepository<AggAppWeeklyPagePlatform>>();
			container.Register<IAggregationRepository<AggAppWeeklyPagePlatformUnique>, AggregationRepository<AggAppWeeklyPagePlatformUnique>>();
			container.Register<IAggregationRepository<AggAppWeeklyPlatform>, AggregationRepository<AggAppWeeklyPlatform>>();
			container.Register<IAggregationRepository<AggAppWeeklyPlatformUnique>, AggregationRepository<AggAppWeeklyPlatformUnique>>();
			container.Register<IAggregationRepository<AggDestination>, AggregationRepository<AggDestination>>();
			#endregion
          
        }
    }
}