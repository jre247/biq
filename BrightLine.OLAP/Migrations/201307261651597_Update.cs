namespace BrightLine.OLAP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AggAppHourlyPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppHourlyPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppHourlyPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPageEventPlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPageEventPlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPageEventPlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppHourlyPagePlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppHourlyPagePlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppHourlyPagePlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppHourlyPageEventPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppHourlyPageEventPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppHourlyPageEventPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPagePlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPagePlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPagePlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPageEventPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPageEventPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPageEventPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppDailyPagePlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppDailyPagePlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppDailyPagePlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPagePlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPagePlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPagePlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPageEventPlatform", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPageEventPlatform", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPageEventPlatform", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPlatformUnique", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPagePlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPagePlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPagePlatformUnique", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppMonthlyPlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppMonthlyPlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppMonthlyPlatformUnique", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppMonthlyPagePlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppMonthlyPagePlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppMonthlyPagePlatformUnique", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppMonthlyPageEventPlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppMonthlyPagePlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppMonthlyPagePlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppMonthlyPagePlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppMonthlyPlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppMonthlyPlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppMonthlyPlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPageEventPlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPagePlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPagePlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPagePlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPlatformUnique", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPlatformUnique", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPlatformUnique", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPageEventPlatform", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPageEventPlatform", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPageEventPlatform", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPagePlatform", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPagePlatform", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPagePlatform", "IsDeleted");
            DropColumn("dbo.AggAppWeeklyPlatform", "DateDeleted");
            DropColumn("dbo.AggAppWeeklyPlatform", "DateCreated");
            DropColumn("dbo.AggAppWeeklyPlatform", "IsDeleted");
            DropColumn("dbo.AggAppDailyPagePlatform", "DateDeleted");
            DropColumn("dbo.AggAppDailyPagePlatform", "DateCreated");
            DropColumn("dbo.AggAppDailyPagePlatform", "IsDeleted");
            DropColumn("dbo.AggAppDailyPlatform", "DateDeleted");
            DropColumn("dbo.AggAppDailyPlatform", "DateCreated");
            DropColumn("dbo.AggAppDailyPlatform", "IsDeleted");
            DropColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppDailyPageVideoPercentPlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppDailyPageEventPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppDailyPageEventPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppDailyPageEventPlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppDailyPagePlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppDailyPagePlatformCity", "DateCreated");
            DropColumn("dbo.AggAppDailyPagePlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppDailyPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppDailyPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppDailyPlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppHourlyPageVideoPercentPlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppHourlyPageEventPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppHourlyPageEventPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppHourlyPageEventPlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppHourlyPagePlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppHourlyPagePlatformCity", "DateCreated");
            DropColumn("dbo.AggAppHourlyPagePlatformCity", "IsDeleted");
            DropColumn("dbo.AggAppDailyPageEventPlatform", "DateDeleted");
            DropColumn("dbo.AggAppDailyPageEventPlatform", "DateCreated");
            DropColumn("dbo.AggAppDailyPageEventPlatform", "IsDeleted");
            DropColumn("dbo.AggAppHourlyPlatformCity", "DateDeleted");
            DropColumn("dbo.AggAppHourlyPlatformCity", "DateCreated");
            DropColumn("dbo.AggAppHourlyPlatformCity", "IsDeleted");
        }
    }
}
