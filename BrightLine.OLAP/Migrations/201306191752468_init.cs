namespace BrightLine.OLAP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AggAppHourlyPlatformCity",
                c => new
                    {
                        DimHourId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimHourId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimCityId })
                .ForeignKey("dbo.DimHour", t => t.DimHourId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .Index(t => t.DimHourId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId);
            
            CreateTable(
                "dbo.DimHour",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimDayId = c.Int(nullable: false),
                        DimWeekId = c.Int(nullable: false),
                        DimMonthId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateString = c.String(),
                        HourOfDay = c.Byte(nullable: false),
                        DayOfWeek = c.Byte(nullable: false),
                        IsWeekend = c.Boolean(nullable: false),
                        DayOfMonth = c.Byte(nullable: false),
                        DayOfYear = c.Int(nullable: false),
                        DayOfWeekName = c.String(),
                        WeekOfYear = c.Byte(nullable: false),
                        MonthOfYear = c.Byte(nullable: false),
                        MonthOfYearName = c.String(),
                        Quarter = c.Byte(nullable: false),
                        Year = c.Int(nullable: false),
                        IsLeapYear = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: true)
                .Index(t => t.DimDayId)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimMonthId);
            
            CreateTable(
                "dbo.DimDay",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimWeekId = c.Int(nullable: false),
                        DimMonthId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateString = c.String(),
                        DayOfWeek = c.Byte(nullable: false),
                        IsWeekend = c.Boolean(nullable: false),
                        DayOfMonth = c.Byte(nullable: false),
                        DayOfYear = c.Int(nullable: false),
                        DayOfWeekName = c.String(),
                        WeekOfYear = c.Byte(nullable: false),
                        MonthOfYear = c.Byte(nullable: false),
                        MonthOfYearName = c.String(),
                        Quarter = c.Byte(nullable: false),
                        Year = c.Int(nullable: false),
                        IsLeapYear = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: false)
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: false)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimMonthId);
            
            CreateTable(
                "dbo.DimWeek",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimMonthId = c.Int(nullable: false),
                        BeginDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        DateString = c.String(),
                        WeekOfYear = c.Byte(nullable: false),
                        MonthOfYear = c.Byte(nullable: false),
                        MonthOfYearName = c.String(),
                        Quarter = c.Byte(nullable: false),
                        Year = c.Int(nullable: false),
                        IsLeapYear = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: false)
                .Index(t => t.DimMonthId);
            
            CreateTable(
                "dbo.DimMonth",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        MonthOfYear = c.Byte(nullable: false),
                        MonthOfYearName = c.String(),
                        Quarter = c.Byte(nullable: false),
                        Year = c.Int(nullable: false),
                        IsLeapYear = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AggAppDailyPageEventPlatform",
                c => new
                    {
                        DayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DayId, t.DimAppVersionId, t.DimAppVersionEventId })
                .ForeignKey("dbo.DimDay", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .Index(t => t.DayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimAppVersionEventId)
                .Index(t => t.DimAppVersionPageId);
            
            CreateTable(
                "dbo.DimAppVersion",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        Version = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimApp", t => t.DimAppId)
                .Index(t => t.DimAppId);
            
            CreateTable(
                "dbo.DimApp",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimPublisherId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimPublisher", t => t.DimPublisherId)
                .Index(t => t.DimPublisherId);
            
            CreateTable(
                "dbo.DimPublisher",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DimAppVersionEvent",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: false)
                .Index(t => t.DimAppVersionId);
            
            CreateTable(
                "dbo.DimAppVersionPage",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: false)
                .Index(t => t.DimAppVersionId);
            
            CreateTable(
                "dbo.DimAppVersionVideo",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: false)
                .Index(t => t.DimAppVersionId);																				    
            CreateTable(
                "dbo.DimPlatformVersion",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DimPlatform", t => t.DimPlatformId, cascadeDelete: true)
                .Index(t => t.DimPlatformId);
            
            CreateTable(
                "dbo.DimPlatform",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DimCity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AggAppHourlyPagePlatformCity",
                c => new
                    {
                        DimHourId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimHourId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId, t.DimCityId })
                .ForeignKey("dbo.DimHour", t => t.DimHourId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimHourId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppHourlyPageEventPlatformCity",
                c => new
                    {
                        DimHourId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimHourId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionEventId, t.DimCityId })
                .ForeignKey("dbo.DimHour", t => t.DimHourId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .Index(t => t.DimHourId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionEventId);
            
            CreateTable(
                "dbo.AggAppHourlyPageVideoPercentPlatformCity",
                c => new
                    {
                        DimHourId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionVideoId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        PercentComplete = c.Byte(nullable: false),
                        CountPlays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimHourId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionVideoId, t.DimCityId })
                .ForeignKey("dbo.DimHour", t => t.DimHourId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionVideo", t => t.DimAppVersionVideoId, cascadeDelete: true)
                .Index(t => t.DimHourId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionVideoId);
            
            CreateTable(
                "dbo.AggAppDailyPlatformCity",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimCityId })
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimCityId, cascadeDelete: true)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId);
            
            CreateTable(
                "dbo.AggAppDailyPagePlatformCity",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId, t.DimCityId })
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppDailyPageEventPlatformCity",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionEventId, t.DimCityId })
                .ForeignKey("dbo.DimHour", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionEventId);
            
            CreateTable(
                "dbo.AggAppDailyPageVideoPercentPlatformCity",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionVideoId = c.Int(nullable: false),
                        DimCityId = c.Int(nullable: false),
                        PercentComplete = c.Byte(nullable: false),
                        CountPlays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionVideoId, t.DimCityId })
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimCity", t => t.DimCityId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionVideo", t => t.DimAppVersionVideoId, cascadeDelete: true)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimCityId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionVideoId);
            
            CreateTable(
                "dbo.AggAppDailyPlatform",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId })
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId);
            
            CreateTable(
                "dbo.AggAppDailyPagePlatform",
                c => new
                    {
                        DimDayId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        CountViews = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimDayId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId })
                .ForeignKey("dbo.DimDay", t => t.DimDayId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimDayId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppWeeklyPlatform",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppVersionId, t.DimPlatformVersionId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId);
            
            CreateTable(
                "dbo.AggAppWeeklyPagePlatform",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        CountViews = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppWeeklyPageEventPlatform",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppVersionId, t.DimPlatformVersionId, t.DimAppVersionPageId, t.DimAppVersionEventId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionEventId);
            
            CreateTable(
                "dbo.AggAppWeeklyPlatformUnique",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppId, t.DimPlatformId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimApp", t => t.DimAppId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformId, cascadeDelete: true)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppId)
                .Index(t => t.DimPlatformId);
            
            CreateTable(
                "dbo.AggAppWeeklyPagePlatformUnique",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        CountViews = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppId, t.DimPlatformId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimApp", t => t.DimAppId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatform", t => t.DimPlatformId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppId)
                .Index(t => t.DimPlatformId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppWeeklyPageEventPlatformUnique",
                c => new
                    {
                        DimWeekId = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimWeekId, t.DimAppId, t.DimPlatformId, t.DimAppVersionPageId, t.DimAppVersionEventId })
                .ForeignKey("dbo.DimWeek", t => t.DimWeekId, cascadeDelete: true)
                .ForeignKey("dbo.DimApp", t => t.DimAppId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatform", t => t.DimPlatformId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .Index(t => t.DimWeekId)
                .Index(t => t.DimAppId)
                .Index(t => t.DimPlatformId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionEventId);
            
            CreateTable(
                "dbo.AggAppMonthlyPlatformUnique",
                c => new
                    {
                        DimMonthId = c.Int(nullable: false),
                        DimAppVersionId = c.Int(nullable: false),
                        DimPlatformVersionId = c.Int(nullable: false),
                        CountSessions = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimMonthId, t.DimAppVersionId, t.DimPlatformVersionId })
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersion", t => t.DimAppVersionId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatformVersion", t => t.DimPlatformVersionId, cascadeDelete: true)
                .Index(t => t.DimMonthId)
                .Index(t => t.DimAppVersionId)
                .Index(t => t.DimPlatformVersionId);
            
            CreateTable(
                "dbo.AggAppMonthlyPagePlatformUnique",
                c => new
                    {
                        DimMonthId = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionPageReferrerId = c.Int(nullable: false),
                        CountViews = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                        CountDurations = c.Int(nullable: false),
                        TotalDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimMonthId, t.DimAppId, t.DimPlatformId, t.DimAppVersionPageId, t.DimAppVersionPageReferrerId })
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: true)
                .ForeignKey("dbo.DimApp", t => t.DimAppId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatform", t => t.DimPlatformId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageReferrerId, cascadeDelete: false)
                .Index(t => t.DimMonthId)
                .Index(t => t.DimAppId)
                .Index(t => t.DimPlatformId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionPageReferrerId);
            
            CreateTable(
                "dbo.AggAppMonthlyPageEventPlatformUnique",
                c => new
                    {
                        DimMonthId = c.Int(nullable: false),
                        DimAppId = c.Int(nullable: false),
                        DimPlatformId = c.Int(nullable: false),
                        DimAppVersionPageId = c.Int(nullable: false),
                        DimAppVersionEventId = c.Int(nullable: false),
                        CountEvents = c.Int(nullable: false),
                        UniqueDevices = c.Int(nullable: false),
                        CountNewDevices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimMonthId, t.DimAppId, t.DimPlatformId, t.DimAppVersionPageId, t.DimAppVersionEventId })
                .ForeignKey("dbo.DimMonth", t => t.DimMonthId, cascadeDelete: true)
                .ForeignKey("dbo.DimApp", t => t.DimAppId, cascadeDelete: true)
                .ForeignKey("dbo.DimPlatform", t => t.DimPlatformId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionPage", t => t.DimAppVersionPageId, cascadeDelete: true)
                .ForeignKey("dbo.DimAppVersionEvent", t => t.DimAppVersionEventId, cascadeDelete: true)
                .Index(t => t.DimMonthId)
                .Index(t => t.DimAppId)
                .Index(t => t.DimPlatformId)
                .Index(t => t.DimAppVersionPageId)
                .Index(t => t.DimAppVersionEventId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AggAppMonthlyPageEventPlatformUnique", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppMonthlyPageEventPlatformUnique", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppMonthlyPageEventPlatformUnique", new[] { "DimPlatformId" });
            DropIndex("dbo.AggAppMonthlyPageEventPlatformUnique", new[] { "DimAppId" });
            DropIndex("dbo.AggAppMonthlyPageEventPlatformUnique", new[] { "DimMonthId" });
            DropIndex("dbo.AggAppMonthlyPagePlatformUnique", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppMonthlyPagePlatformUnique", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppMonthlyPagePlatformUnique", new[] { "DimPlatformId" });
            DropIndex("dbo.AggAppMonthlyPagePlatformUnique", new[] { "DimAppId" });
            DropIndex("dbo.AggAppMonthlyPagePlatformUnique", new[] { "DimMonthId" });
            DropIndex("dbo.AggAppMonthlyPlatformUnique", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppMonthlyPlatformUnique", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppMonthlyPlatformUnique", new[] { "DimMonthId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatformUnique", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatformUnique", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatformUnique", new[] { "DimPlatformId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatformUnique", new[] { "DimAppId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatformUnique", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppWeeklyPagePlatformUnique", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppWeeklyPagePlatformUnique", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppWeeklyPagePlatformUnique", new[] { "DimPlatformId" });
            DropIndex("dbo.AggAppWeeklyPagePlatformUnique", new[] { "DimAppId" });
            DropIndex("dbo.AggAppWeeklyPagePlatformUnique", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppWeeklyPlatformUnique", new[] { "DimPlatformId" });
            DropIndex("dbo.AggAppWeeklyPlatformUnique", new[] { "DimAppId" });
            DropIndex("dbo.AggAppWeeklyPlatformUnique", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatform", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatform", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppWeeklyPageEventPlatform", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppWeeklyPagePlatform", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppWeeklyPagePlatform", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppWeeklyPagePlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppWeeklyPagePlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppWeeklyPagePlatform", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppWeeklyPlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppWeeklyPlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppWeeklyPlatform", new[] { "DimWeekId" });
            DropIndex("dbo.AggAppDailyPagePlatform", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppDailyPagePlatform", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppDailyPagePlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPagePlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPagePlatform", new[] { "DimDayId" });
            DropIndex("dbo.AggAppDailyPlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPlatform", new[] { "DimDayId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimAppVersionVideoId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPageVideoPercentPlatformCity", new[] { "DimDayId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPageEventPlatformCity", new[] { "DimDayId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPagePlatformCity", new[] { "DimDayId" });
            DropIndex("dbo.AggAppDailyPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppDailyPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPlatformCity", new[] { "DimDayId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimAppVersionVideoId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppHourlyPageVideoPercentPlatformCity", new[] { "DimHourId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppHourlyPageEventPlatformCity", new[] { "DimHourId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimAppVersionPageReferrerId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppHourlyPagePlatformCity", new[] { "DimHourId" });
            DropIndex("dbo.DimPlatformVersion", new[] { "DimPlatformId" });
            DropIndex("dbo.DimAppVersionVideo", new[] { "DimAppVersionId" });
            DropIndex("dbo.DimAppVersionPage", new[] { "DimAppVersionId" });
            DropIndex("dbo.DimAppVersionEvent", new[] { "DimAppVersionId" });
            DropIndex("dbo.DimApp", new[] { "DimPublisherId" });
            DropIndex("dbo.DimAppVersion", new[] { "DimAppId" });
            DropIndex("dbo.AggAppDailyPageEventPlatform", new[] { "DimAppVersionPageId" });
            DropIndex("dbo.AggAppDailyPageEventPlatform", new[] { "DimAppVersionEventId" });
            DropIndex("dbo.AggAppDailyPageEventPlatform", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppDailyPageEventPlatform", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppDailyPageEventPlatform", new[] { "DayId" });
            DropIndex("dbo.DimWeek", new[] { "DimMonthId" });
            DropIndex("dbo.DimDay", new[] { "DimMonthId" });
            DropIndex("dbo.DimDay", new[] { "DimWeekId" });
            DropIndex("dbo.DimHour", new[] { "DimMonthId" });
            DropIndex("dbo.DimHour", new[] { "DimWeekId" });
            DropIndex("dbo.DimHour", new[] { "DimDayId" });
            DropIndex("dbo.AggAppHourlyPlatformCity", new[] { "DimCityId" });
            DropIndex("dbo.AggAppHourlyPlatformCity", new[] { "DimPlatformVersionId" });
            DropIndex("dbo.AggAppHourlyPlatformCity", new[] { "DimAppVersionId" });
            DropIndex("dbo.AggAppHourlyPlatformCity", new[] { "DimHourId" });
            DropForeignKey("dbo.AggAppMonthlyPageEventPlatformUnique", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppMonthlyPageEventPlatformUnique", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppMonthlyPageEventPlatformUnique", "DimPlatformId", "dbo.DimPlatform");
            DropForeignKey("dbo.AggAppMonthlyPageEventPlatformUnique", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppMonthlyPageEventPlatformUnique", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.AggAppMonthlyPagePlatformUnique", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppMonthlyPagePlatformUnique", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppMonthlyPagePlatformUnique", "DimPlatformId", "dbo.DimPlatform");
            DropForeignKey("dbo.AggAppMonthlyPagePlatformUnique", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppMonthlyPagePlatformUnique", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.AggAppMonthlyPlatformUnique", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppMonthlyPlatformUnique", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppMonthlyPlatformUnique", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatformUnique", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatformUnique", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatformUnique", "DimPlatformId", "dbo.DimPlatform");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatformUnique", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatformUnique", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppWeeklyPagePlatformUnique", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPagePlatformUnique", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPagePlatformUnique", "DimPlatformId", "dbo.DimPlatform");
            DropForeignKey("dbo.AggAppWeeklyPagePlatformUnique", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppWeeklyPagePlatformUnique", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppWeeklyPlatformUnique", "DimPlatformId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppWeeklyPlatformUnique", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppWeeklyPlatformUnique", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatform", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatform", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppWeeklyPageEventPlatform", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppWeeklyPagePlatform", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPagePlatform", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppWeeklyPagePlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppWeeklyPagePlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppWeeklyPagePlatform", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppWeeklyPlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppWeeklyPlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppWeeklyPlatform", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.AggAppDailyPagePlatform", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPagePlatform", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPagePlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPagePlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPagePlatform", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppDailyPlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPlatform", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimAppVersionVideoId", "dbo.DimAppVersionVideo");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPageVideoPercentPlatformCity", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPageEventPlatformCity", "DimDayId", "dbo.DimHour");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimCityId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPagePlatformCity", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppDailyPlatformCity", "DimCityId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppDailyPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPlatformCity", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimAppVersionVideoId", "dbo.DimAppVersionVideo");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppHourlyPageVideoPercentPlatformCity", "DimHourId", "dbo.DimHour");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppHourlyPageEventPlatformCity", "DimHourId", "dbo.DimHour");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimAppVersionPageReferrerId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppHourlyPagePlatformCity", "DimHourId", "dbo.DimHour");
            DropForeignKey("dbo.DimPlatformVersion", "DimPlatformId", "dbo.DimPlatform");
            DropForeignKey("dbo.DimAppVersionVideo", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.DimAppVersionPage", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.DimAppVersionEvent", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.DimApp", "DimPublisherId", "dbo.DimPublisher");
            DropForeignKey("dbo.DimAppVersion", "DimAppId", "dbo.DimApp");
            DropForeignKey("dbo.AggAppDailyPageEventPlatform", "DimAppVersionPageId", "dbo.DimAppVersionPage");
            DropForeignKey("dbo.AggAppDailyPageEventPlatform", "DimAppVersionEventId", "dbo.DimAppVersionEvent");
            DropForeignKey("dbo.AggAppDailyPageEventPlatform", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppDailyPageEventPlatform", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppDailyPageEventPlatform", "DayId", "dbo.DimDay");
            DropForeignKey("dbo.DimWeek", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.DimDay", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.DimDay", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.DimHour", "DimMonthId", "dbo.DimMonth");
            DropForeignKey("dbo.DimHour", "DimWeekId", "dbo.DimWeek");
            DropForeignKey("dbo.DimHour", "DimDayId", "dbo.DimDay");
            DropForeignKey("dbo.AggAppHourlyPlatformCity", "DimCityId", "dbo.DimCity");
            DropForeignKey("dbo.AggAppHourlyPlatformCity", "DimPlatformVersionId", "dbo.DimPlatformVersion");
            DropForeignKey("dbo.AggAppHourlyPlatformCity", "DimAppVersionId", "dbo.DimAppVersion");
            DropForeignKey("dbo.AggAppHourlyPlatformCity", "DimHourId", "dbo.DimHour");
            DropTable("dbo.AggAppMonthlyPageEventPlatformUnique");
            DropTable("dbo.AggAppMonthlyPagePlatformUnique");
            DropTable("dbo.AggAppMonthlyPlatformUnique");
            DropTable("dbo.AggAppWeeklyPageEventPlatformUnique");
            DropTable("dbo.AggAppWeeklyPagePlatformUnique");
            DropTable("dbo.AggAppWeeklyPlatformUnique");
            DropTable("dbo.AggAppWeeklyPageEventPlatform");
            DropTable("dbo.AggAppWeeklyPagePlatform");
            DropTable("dbo.AggAppWeeklyPlatform");
            DropTable("dbo.AggAppDailyPagePlatform");
            DropTable("dbo.AggAppDailyPlatform");
            DropTable("dbo.AggAppDailyPageVideoPercentPlatformCity");
            DropTable("dbo.AggAppDailyPageEventPlatformCity");
            DropTable("dbo.AggAppDailyPagePlatformCity");
            DropTable("dbo.AggAppDailyPlatformCity");
            DropTable("dbo.AggAppHourlyPageVideoPercentPlatformCity");
            DropTable("dbo.AggAppHourlyPageEventPlatformCity");
            DropTable("dbo.AggAppHourlyPagePlatformCity");
            DropTable("dbo.DimCity");
            DropTable("dbo.DimPlatform");
            DropTable("dbo.DimPlatformVersion");
            DropTable("dbo.DimAppVersionVideo");
            DropTable("dbo.DimAppVersionPage");
            DropTable("dbo.DimAppVersionEvent");
            DropTable("dbo.DimPublisher");
            DropTable("dbo.DimApp");
            DropTable("dbo.DimAppVersion");
            DropTable("dbo.AggAppDailyPageEventPlatform");
            DropTable("dbo.DimMonth");
            DropTable("dbo.DimWeek");
            DropTable("dbo.DimDay");
            DropTable("dbo.DimHour");
            DropTable("dbo.AggAppHourlyPlatformCity");
        }
    }
}
