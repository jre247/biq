namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rebuildall : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInvitation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        SecondaryToken = c.Guid(nullable: false),
                        DateIssued = c.DateTime(nullable: false),
                        DateExpired = c.DateTime(nullable: false),
                        DateActivated = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        User_Id = c.Int(),
                        InvitedUser_Id = c.Int(),
                        CreatingUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .ForeignKey("dbo.User", t => t.InvitedUser_Id)
                .ForeignKey("dbo.User", t => t.CreatingUser_Id)
                .Index(t => t.User_Id)
                .Index(t => t.InvitedUser_Id)
                .Index(t => t.CreatingUser_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 255, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        PasswordFormat = c.Byte(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastActivityDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        LastPasswordChangedDate = c.DateTime(),
                        FailedPasswordAttemptCount = c.Byte(nullable: false),
                        FailedPasswordAttemptWindowStart = c.DateTime(),
                        LockOutWindowStart = c.DateTime(),
                        TimeZoneId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Advertiser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertiser", t => t.Advertiser_Id)
                .Index(t => t.Advertiser_Id);
            
            CreateTable(
                "dbo.Advertiser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Advertiser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertiser", t => t.Advertiser_Id)
                .Index(t => t.Advertiser_Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        OldIdentity = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Brand_Id = c.Int(nullable: false),
                        SubSegment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brand", t => t.Brand_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubSegment", t => t.SubSegment_Id)
                .Index(t => t.Brand_Id)
                .Index(t => t.SubSegment_Id);
            
            CreateTable(
                "dbo.SubSegment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Segment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Segment", t => t.Segment_Id, cascadeDelete: true)
                .Index(t => t.Segment_Id);
            
            CreateTable(
                "dbo.Segment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Vertical_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vertical", t => t.Vertical_Id)
                .Index(t => t.Vertical_Id);
            
            CreateTable(
                "dbo.Vertical",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Benchmark_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benchmark", t => t.Benchmark_Id)
                .Index(t => t.Benchmark_Id);
            
            CreateTable(
                "dbo.Benchmark",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Impressions = c.Long(nullable: false),
                        PercentDVRProofImpressions = c.Double(),
                        CTR = c.Double(),
                        Visits = c.Int(),
                        UniqueVisits = c.Int(),
                        EngagementRate = c.Double(),
                        AverageDuration = c.Int(),
                        CPV = c.Double(),
                        MediaCPV = c.Double(),
                        NumberOfCampaigns = c.Byte(),
                        EngagementRateIndex = c.Double(),
                        EngagementRateStandardDeviation = c.Double(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        SalesForceId = c.String(maxLength: 255, unicode: false),
                        LaunchDate = c.DateTime(),
                        Description = c.String(maxLength: 1000, unicode: false),
                        CmsKey = c.String(),
                        Spend = c.Int(nullable: false),
                        OldIdentity = c.Int(),
                        EngagementRateIndex = c.Double(nullable: false),
                        MediaSpend = c.Int(nullable: false),
                        BandwidthSpend = c.Int(nullable: false),
                        UxtvType = c.Int(nullable: false),
                        SettingsLastUpdated = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Product_Id = c.Int(nullable: false),
                        Audience_Id = c.Int(),
                        Thumbnail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Audience", t => t.Audience_Id)
                .ForeignKey("dbo.Resource", t => t.Thumbnail_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Audience_Id)
                .Index(t => t.Thumbnail_Id);
            
            CreateTable(
                "dbo.Experience",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(maxLength: 255, unicode: false),
                        OldIdentity = c.Int(),
                        MediaSpend = c.Int(nullable: false),
                        BandwidthSpend = c.Int(nullable: false),
                        EngagementRateIndex = c.Double(nullable: false),
                        LaunchDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Country_Id = c.Int(),
                        AgeDemographic_Id = c.Int(),
                        EthnicityDemographic_Id = c.Int(),
                        GenderDemographic_Id = c.Int(),
                        IncomeDemographic_Id = c.Int(),
                        Audience_Id = c.Int(),
                        Campaign_Id = c.Int(),
                        Platform_Id = c.Int(),
                        Launch_Id = c.Int(),
                        ExperienceType_Id = c.Int(),
                        Publisher_Id = c.Int(),
                        DistributionPoint_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.Country_Id)
                .ForeignKey("dbo.AgeDemographic", t => t.AgeDemographic_Id)
                .ForeignKey("dbo.EthnicityDemographic", t => t.EthnicityDemographic_Id)
                .ForeignKey("dbo.GenderDemographic", t => t.GenderDemographic_Id)
                .ForeignKey("dbo.IncomeDemographic", t => t.IncomeDemographic_Id)
                .ForeignKey("dbo.Audience", t => t.Audience_Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id)
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .ForeignKey("dbo.Launch", t => t.Launch_Id)
                .ForeignKey("dbo.ExperienceType", t => t.ExperienceType_Id)
                .ForeignKey("dbo.Publisher", t => t.Publisher_Id)
                .ForeignKey("dbo.DistributionPoint", t => t.DistributionPoint_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.AgeDemographic_Id)
                .Index(t => t.EthnicityDemographic_Id)
                .Index(t => t.GenderDemographic_Id)
                .Index(t => t.IncomeDemographic_Id)
                .Index(t => t.Audience_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.Launch_Id)
                .Index(t => t.ExperienceType_Id)
                .Index(t => t.Publisher_Id)
                .Index(t => t.DistributionPoint_Id);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Audience",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        GenderDemographic_Id = c.Int(),
                        Target_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GenderDemographic", t => t.GenderDemographic_Id)
                .ForeignKey("dbo.Target", t => t.Target_Id)
                .Index(t => t.GenderDemographic_Id)
                .Index(t => t.Target_Id);
            
            CreateTable(
                "dbo.AgeDemographic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LowerLimit = c.Byte(nullable: false),
                        UpperLimit = c.Byte(),
                        Display = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        ShortDisplay = c.String(),
                        Audience_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audience", t => t.Audience_Id)
                .Index(t => t.Audience_Id);
            
            CreateTable(
                "dbo.EthnicityDemographic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Audience_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audience", t => t.Audience_Id)
                .Index(t => t.Audience_Id);
            
            CreateTable(
                "dbo.GenderDemographic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomeDemographic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Audience_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audience", t => t.Audience_Id)
                .Index(t => t.Audience_Id);
            
            CreateTable(
                "dbo.Ad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AdType_Id = c.Int(nullable: false),
                        Experience_Id = c.Int(nullable: false),
                        Flight_Id = c.Int(),
                        Creative_Id = c.Int(),
                        AppPlatformPlacement_Id = c.Int(),
                        PlatformPlacement_Id = c.Int(),
                        NetworkPlacement_Id = c.Int(),
                        Metadata_Id = c.Int(),
                        AdGroup_Id = c.Int(),
                        AdTypeGroup_Id = c.Int(),
                        AdFunction_Id = c.Int(),
                        AdFormat_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdType", t => t.AdType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Experience", t => t.Experience_Id, cascadeDelete: true)
                .ForeignKey("dbo.Flight", t => t.Flight_Id)
                .ForeignKey("dbo.Creative", t => t.Creative_Id)
                .ForeignKey("dbo.AppPlatformPlacement", t => t.AppPlatformPlacement_Id)
                .ForeignKey("dbo.PlatformPlacement", t => t.PlatformPlacement_Id)
                .ForeignKey("dbo.NetworkPlatformPlacement", t => t.NetworkPlacement_Id)
                .ForeignKey("dbo.AdMetadata", t => t.Metadata_Id)
                .ForeignKey("dbo.AdGroup", t => t.AdGroup_Id)
                .ForeignKey("dbo.AdTypeGroup", t => t.AdTypeGroup_Id)
                .ForeignKey("dbo.AdFunction", t => t.AdFunction_Id)
                .ForeignKey("dbo.AdFormat", t => t.AdFormat_Id)
                .Index(t => t.AdType_Id)
                .Index(t => t.Experience_Id)
                .Index(t => t.Flight_Id)
                .Index(t => t.Creative_Id)
                .Index(t => t.AppPlatformPlacement_Id)
                .Index(t => t.PlatformPlacement_Id)
                .Index(t => t.NetworkPlacement_Id)
                .Index(t => t.Metadata_Id)
                .Index(t => t.AdGroup_Id)
                .Index(t => t.AdTypeGroup_Id)
                .Index(t => t.AdFunction_Id)
                .Index(t => t.AdFormat_Id);
            
            CreateTable(
                "dbo.AdType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        ManifestName = c.String(),
                        IsPromo = c.Boolean(nullable: false),
                        IsDVRProof = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdTypeGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        WeekNumber = c.Byte(nullable: false),
                        HourOfDay = c.Byte(),
                        Daypart = c.String(),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Duration = c.Int(),
                        Visits = c.Int(),
                        NonInteractiveVisits = c.Int(),
                        InteractiveVisits = c.Int(),
                        UniqueVisits = c.Int(),
                        UniqueInteractiveVisits = c.Int(),
                        UniqueNonInteractiveVisits = c.Int(),
                        AverageDuration = c.Int(),
                        ProjectedImpressions = c.Int(),
                        Impressions = c.Int(),
                        UniqueImpressions = c.Int(),
                        Clicks = c.Int(),
                        UniqueClicks = c.Int(),
                        ClickThrough = c.Double(),
                        NetworkGroup = c.String(),
                        Network = c.String(),
                        Program = c.String(),
                        ProgramRank = c.Int(),
                        RecipeViewCount = c.Int(),
                        PageViews = c.Int(),
                        UniquePageViews = c.Int(),
                        TargetImpressions = c.Int(),
                        ProjectedTargetImpressions = c.Int(),
                        VideoViews = c.Int(),
                        UniqueVideoViews = c.Int(),
                        VideoRunTime = c.Int(),
                        VideoCompletionRate = c.Int(),
                        VideoViewDuration = c.Int(),
                        GamePlays = c.Int(),
                        UniqueGamePlays = c.Int(),
                        ProductViews = c.Int(),
                        Rfi = c.Int(),
                        UniqueRfi = c.Int(),
                        Bounces = c.Int(),
                        UniqueBounces = c.Int(),
                        AppDownloads = c.Int(),
                        ClicksToLaunchCrossPlatformExperience = c.Int(),
                        Channel = c.String(),
                        Resolution = c.String(),
                        DataSource = c.String(),
                        TotalEvents = c.Int(),
                        UniqueEvents = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(nullable: false),
                        Page_Id = c.Int(),
                        Import_Id = c.Int(),
                        FeatureAdPage_Id = c.Int(),
                        Flight_Id = c.Int(),
                        Element_Id = c.Int(),
                        AdEventType_Id = c.Int(),
                        AdEventLabel_Id = c.Int(),
                        AdEventMethod_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .ForeignKey("dbo.Page", t => t.Page_Id)
                .ForeignKey("dbo.Import", t => t.Import_Id)
                .ForeignKey("dbo.FeatureAdPage", t => t.FeatureAdPage_Id)
                .ForeignKey("dbo.Flight", t => t.Flight_Id)
                .ForeignKey("dbo.Element", t => t.Element_Id)
                .ForeignKey("dbo.AdEventType", t => t.AdEventType_Id)
                .ForeignKey("dbo.AdEventLabel", t => t.AdEventLabel_Id)
                .ForeignKey("dbo.AdEventMethod", t => t.AdEventMethod_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.Page_Id)
                .Index(t => t.Import_Id)
                .Index(t => t.FeatureAdPage_Id)
                .Index(t => t.Flight_Id)
                .Index(t => t.Element_Id)
                .Index(t => t.AdEventType_Id)
                .Index(t => t.AdEventLabel_Id)
                .Index(t => t.AdEventMethod_Id);
            
            CreateTable(
                "dbo.FeatureAdPage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        FeatureAd_Id = c.Int(nullable: false),
                        Page_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureAd", t => t.FeatureAd_Id, cascadeDelete: true)
                .ForeignKey("dbo.Page", t => t.Page_Id, cascadeDelete: true)
                .Index(t => t.FeatureAd_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.FeatureAd",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Feature_Id = c.Int(nullable: false),
                        Ad_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feature", t => t.Feature_Id, cascadeDelete: true)
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .Index(t => t.Feature_Id)
                .Index(t => t.Ad_Id);
            
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EngagementRate = c.Double(),
                        OrderIndex = c.Int(nullable: false),
                        ButtonName = c.String(),
                        DateAdded = c.DateTime(),
                        DateArchived = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(nullable: false),
                        FeatureType_Id = c.Int(nullable: false),
                        FeatureCategory_Id = c.Int(),
                        Blueprint_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id, cascadeDelete: true)
                .ForeignKey("dbo.FeatureCategory", t => t.FeatureCategory_Id)
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.FeatureType_Id)
                .Index(t => t.FeatureCategory_Id)
                .Index(t => t.Blueprint_Id);
            
            CreateTable(
                "dbo.FeatureType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        FeatureTypeGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureTypeGroup", t => t.FeatureTypeGroup_Id)
                .Index(t => t.FeatureTypeGroup_Id);
            
            CreateTable(
                "dbo.FeatureTypeGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeatureCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blueprint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManifestName = c.String(),
                        GroupId = c.Guid(nullable: false),
                        MajorVersion = c.Int(nullable: false),
                        MinorVersion = c.Int(nullable: false),
                        Patch = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        FeatureType_Id = c.Int(),
                        Preview_Id = c.Int(),
                        ConnectedTVCreative_Id = c.Int(),
                        ConnectedTVSupport_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id)
                .ForeignKey("dbo.Resource", t => t.Preview_Id)
                .ForeignKey("dbo.Resource", t => t.ConnectedTVCreative_Id)
                .ForeignKey("dbo.Resource", t => t.ConnectedTVSupport_Id)
                .Index(t => t.FeatureType_Id)
                .Index(t => t.Preview_Id)
                .Index(t => t.ConnectedTVCreative_Id)
                .Index(t => t.ConnectedTVSupport_Id);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Filename = c.String(),
                        Extension = c.String(),
                        Source = c.String(),
                        Url = c.String(),
                        Size = c.Int(),
                        Height = c.Int(),
                        Width = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ExecutionStage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SortOrder = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductionStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsFiltered = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Platform",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Reach = c.Int(),
                        ManifestName = c.String(maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Benchmark_Id = c.Int(),
                        PlatformGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benchmark", t => t.Benchmark_Id)
                .ForeignKey("dbo.PlatformGroup", t => t.PlatformGroup_Id, cascadeDelete: true)
                .Index(t => t.Benchmark_Id)
                .Index(t => t.PlatformGroup_Id);
            
            CreateTable(
                "dbo.PlatformGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Parent_Id = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publisher", t => t.Parent_Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExperienceType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ManifestName = c.String(maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DistributionPoint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsRecommended = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        ExperienceType_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                        App_Id = c.Int(),
                        Publisher_Id = c.Int(),
                        Network_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExperienceType", t => t.ExperienceType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .ForeignKey("dbo.App", t => t.App_Id)
                .ForeignKey("dbo.Publisher", t => t.Publisher_Id)
                .ForeignKey("dbo.Network", t => t.Network_Id)
                .Index(t => t.ExperienceType_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.App_Id)
                .Index(t => t.Publisher_Id)
                .Index(t => t.Network_Id);
            
            CreateTable(
                "dbo.App",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppId = c.Guid(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Publisher_Id = c.Int(),
                        Advertiser_Id = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publisher", t => t.Publisher_Id)
                .ForeignKey("dbo.Advertiser", t => t.Advertiser_Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Publisher_Id)
                .Index(t => t.Advertiser_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Network",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManifestName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChecklistStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WeeksFromStart = c.Int(nullable: false),
                        Notes = c.String(),
                        Order = c.Int(nullable: false),
                        IsExternalFacing = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Checklist_Id = c.Int(nullable: false),
                        ExecutionStage_Id = c.Int(nullable: false),
                        ProductionStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Checklist", t => t.Checklist_Id, cascadeDelete: true)
                .ForeignKey("dbo.ExecutionStage", t => t.ExecutionStage_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductionStep", t => t.ProductionStep_Id, cascadeDelete: true)
                .Index(t => t.Checklist_Id)
                .Index(t => t.ExecutionStage_Id)
                .Index(t => t.ProductionStep_Id);
            
            CreateTable(
                "dbo.Checklist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UxtvType = c.Int(nullable: false),
                        ExecutionType = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        IsLive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Platform_Id = c.Int(nullable: false),
                        ExperienceType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .ForeignKey("dbo.ExperienceType", t => t.ExperienceType_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.ExperienceType_Id);
            
            CreateTable(
                "dbo.ChecklistOwner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LaunchStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Notes = c.String(),
                        Order = c.Int(nullable: false),
                        WeeksFromStart = c.Int(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        ActualEndDate = c.DateTime(),
                        IsExternalFacing = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Launch_Id = c.Int(nullable: false),
                        ExecutionStage_Id = c.Int(nullable: false),
                        ProductionStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Launch", t => t.Launch_Id, cascadeDelete: true)
                .ForeignKey("dbo.ExecutionStage", t => t.ExecutionStage_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductionStep", t => t.ProductionStep_Id, cascadeDelete: true)
                .Index(t => t.Launch_Id)
                .Index(t => t.ExecutionStage_Id)
                .Index(t => t.ProductionStep_Id);
            
            CreateTable(
                "dbo.Launch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        ActualStartDate = c.DateTime(),
                        LaunchDate = c.DateTime(nullable: false),
                        ActualLaunchDate = c.DateTime(),
                        LaunchDuration = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Experience_Id = c.Int(nullable: false),
                        Checklist_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id, cascadeDelete: true)
                .ForeignKey("dbo.Checklist", t => t.Checklist_Id, cascadeDelete: true)
                .Index(t => t.Experience_Id)
                .Index(t => t.Checklist_Id);
            
            CreateTable(
                "dbo.LaunchMilestone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        LaunchStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LaunchStep", t => t.LaunchStep_Id, cascadeDelete: true)
                .Index(t => t.LaunchStep_Id);
            
            CreateTable(
                "dbo.ChecklistMilestone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        ChecklistStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChecklistStep", t => t.ChecklistStep_Id, cascadeDelete: true)
                .Index(t => t.ChecklistStep_Id);
            
            CreateTable(
                "dbo.BlueprintPlatform",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Blueprint_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                        Creative_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .ForeignKey("dbo.Resource", t => t.Creative_Id)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.Creative_Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Property = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        FeatureType_Id = c.Int(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id)
                .ForeignKey("dbo.Page", t => t.Page_Id)
                .Index(t => t.FeatureType_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.Page",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RelativeUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        FeatureAd_Id = c.Int(),
                        Feature_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureAd", t => t.FeatureAd_Id)
                .ForeignKey("dbo.Feature", t => t.Feature_Id)
                .Index(t => t.FeatureAd_Id)
                .Index(t => t.Feature_Id);
            
            CreateTable(
                "dbo.RFIResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResultType = c.Int(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        UniqueRFIs = c.Int(),
                        TotalRFIs = c.Int(),
                        EmailRFIs = c.Int(),
                        SweepstakeRFIs = c.Int(),
                        CouponRFIs = c.Int(),
                        SampleRFIs = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(nullable: false),
                        Import_Id = c.Int(),
                        ImportPage_Id = c.Int(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .ForeignKey("dbo.Import", t => t.Import_Id)
                .ForeignKey("dbo.ImportPage", t => t.ImportPage_Id)
                .ForeignKey("dbo.Page", t => t.Page_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.Import_Id)
                .Index(t => t.ImportPage_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.Import",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportType = c.Int(nullable: false),
                        ImportStatus = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        FileName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Experience_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id, cascadeDelete: true)
                .Index(t => t.Experience_Id);
            
            CreateTable(
                "dbo.ImportAdResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        WeekNumber = c.Byte(nullable: false),
                        Daypart = c.String(),
                        HourOfDay = c.Byte(),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Duration = c.Int(),
                        Visits = c.Int(),
                        NonInteractiveVisits = c.Int(),
                        InteractiveVisits = c.Int(),
                        UniqueVisits = c.Int(),
                        UniqueInteractiveVisits = c.Int(),
                        UniqueNonInteractiveVisits = c.Int(),
                        AverageDuration = c.Int(),
                        ProjectedImpressions = c.Int(),
                        Impressions = c.Int(),
                        UniqueImpressions = c.Int(),
                        Clicks = c.Int(),
                        UniqueClicks = c.Int(),
                        ClickThrough = c.Double(),
                        NetworkGroup = c.String(),
                        Network = c.String(),
                        Program = c.String(),
                        ProgramRank = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(nullable: false),
                        Import_Id = c.Int(),
                        Page_Id = c.Int(),
                        Flight_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .ForeignKey("dbo.Import", t => t.Import_Id)
                .ForeignKey("dbo.ImportPage", t => t.Page_Id)
                .ForeignKey("dbo.Flight", t => t.Flight_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.Import_Id)
                .Index(t => t.Page_Id)
                .Index(t => t.Flight_Id);
            
            CreateTable(
                "dbo.ImportPage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportPageCategoryName = c.String(),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Flight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Experience_Id = c.Int(),
                        ProductLine_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id)
                .ForeignKey("dbo.ProductLine", t => t.ProductLine_Id)
                .Index(t => t.Experience_Id)
                .Index(t => t.ProductLine_Id);
            
            CreateTable(
                "dbo.ProductLine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImportConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                        AdResultType = c.Int(nullable: false),
                        Value = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AdType_Id = c.Int(),
                        Import_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdType", t => t.AdType_Id)
                .ForeignKey("dbo.Import", t => t.Import_Id)
                .Index(t => t.AdType_Id)
                .Index(t => t.Import_Id);
            
            CreateTable(
                "dbo.ImportMessageLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageType = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                        FeatureType = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Import_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Import", t => t.Import_Id, cascadeDelete: true)
                .Index(t => t.Import_Id);
            
            CreateTable(
                "dbo.ImportRFIResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResultType = c.Int(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        UniqueRFIs = c.Int(),
                        TotalRFIs = c.Int(),
                        EmailRFIs = c.Int(),
                        SweepstakeRFIs = c.Int(),
                        CouponRFIs = c.Int(),
                        SampleRFIs = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(nullable: false),
                        Import_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .ForeignKey("dbo.Import", t => t.Import_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.Import_Id);
            
            CreateTable(
                "dbo.Element",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Type = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Page", t => t.Page_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.AdEventType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdEventLabel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdEventMethod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Creative",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Blueprint_Id = c.Int(),
                        Resource_Id = c.Int(),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id)
                .ForeignKey("dbo.Resource", t => t.Resource_Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.Resource_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.AppPlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AppPlatform_Id = c.Int(nullable: false),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppPlatform", t => t.AppPlatform_Id, cascadeDelete: true)
                .ForeignKey("dbo.Placement", t => t.Placement_Id, cascadeDelete: true)
                .Index(t => t.AppPlatform_Id)
                .Index(t => t.Placement_Id);
            
            CreateTable(
                "dbo.AppPlatform",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        App_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.App", t => t.App_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.App_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Placement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Height = c.Int(),
                        Width = c.Int(),
                        LocationDetails = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AdTypeGroup_Id = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdTypeGroup", t => t.AdTypeGroup_Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.AdTypeGroup_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.PlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Platform_Id = c.Int(nullable: false),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .ForeignKey("dbo.Placement", t => t.Placement_Id, cascadeDelete: true)
                .Index(t => t.Platform_Id)
                .Index(t => t.Placement_Id);
            
            CreateTable(
                "dbo.NetworkPlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Network_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Network", t => t.Network_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .ForeignKey("dbo.Placement", t => t.Placement_Id, cascadeDelete: true)
                .Index(t => t.Network_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.Placement_Id);
            
            CreateTable(
                "dbo.AdMetadata",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Notes = c.String(),
                        Platform = c.String(),
                        Placement = c.String(),
                        AdDetailSetup = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdFunction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdFormat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExperienceDownloadHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Downloads = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Experience_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id)
                .Index(t => t.Experience_Id);
            
            CreateTable(
                "dbo.CampaignContentSchema",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(maxLength: 50),
                        DataSchema = c.String(),
                        DataModelsRaw = c.String(),
                        DataModelsPublished = c.String(),
                        IsPublished = c.Boolean(nullable: false),
                        LastPublishEnvironment = c.String(),
                        ChangeComment = c.String(),
                        LastPublishedDate = c.DateTime(),
                        LastPublishedModelTypeList = c.String(),
                        TotalModels = c.Int(nullable: false),
                        TotalLookups = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.CampaignContentModelInstance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        ModelName = c.String(),
                        InstanceData = c.String(),
                        InstanceDataJson = c.String(),
                        IsPublishable = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Schema_Id = c.Int(nullable: false),
                        Model_Id = c.Int(),
                        Format_Id = c.Int(),
                        Type_Id = c.Int(),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignContentSchema", t => t.Schema_Id, cascadeDelete: true)
                .ForeignKey("dbo.CampaignContentModel", t => t.Model_Id)
                .ForeignKey("dbo.CampaignContentModelBaseType", t => t.Format_Id)
                .ForeignKey("dbo.CampaignContentModelType", t => t.Type_Id)
                .ForeignKey("dbo.CampaignContentModelInstance", t => t.Parent_Id)
                .Index(t => t.Schema_Id)
                .Index(t => t.Model_Id)
                .Index(t => t.Format_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Parent_Id);
            
            CreateTable(
                "dbo.CampaignContentModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelName = c.String(),
                        InstancesData = c.String(),
                        InstancesDataJson = c.String(),
                        SqlTemplateQuery = c.String(),
                        SqlTemplateFields = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        BaseType_Id = c.Int(),
                        Schema_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignContentModelBaseType", t => t.BaseType_Id)
                .ForeignKey("dbo.CampaignContentSchema", t => t.Schema_Id, cascadeDelete: true)
                .Index(t => t.BaseType_Id)
                .Index(t => t.Schema_Id);
            
            CreateTable(
                "dbo.CampaignContentModelBaseType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CampaignContentModelType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeasurementPlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Metric",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        MeasurementPlan_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MeasurementPlan", t => t.MeasurementPlan_Id)
                .Index(t => t.MeasurementPlan_Id);
            
            CreateTable(
                "dbo.CampaignObjective",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(nullable: false),
                        Objective_Id = c.Int(nullable: false),
                        Target_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.TargetObjective", t => t.Objective_Id, cascadeDelete: true)
                .ForeignKey("dbo.Target", t => t.Target_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.Objective_Id)
                .Index(t => t.Target_Id);
            
            CreateTable(
                "dbo.TargetObjective",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExperienceCandidate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LaunchDate = c.DateTime(),
                        MediaSpend = c.Int(nullable: false),
                        BandwidthSpend = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Campaign_Id = c.Int(nullable: false),
                        DistributionPoint_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.DistributionPoint", t => t.DistributionPoint_Id, cascadeDelete: true)
                .Index(t => t.Campaign_Id)
                .Index(t => t.DistributionPoint_Id);
            
            CreateTable(
                "dbo.AdBuildRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManifestId = c.Guid(nullable: false),
                        Manifest = c.String(nullable: false),
                        PublishedOn = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        RequestType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(nullable: false),
                        PublishedBy_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.PublishedBy_Id, cascadeDelete: true)
                .Index(t => t.Campaign_Id)
                .Index(t => t.PublishedBy_Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        DisplayName = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PasswordHashHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PasswordHash = c.String(nullable: false, maxLength: 1024),
                        DateChanged = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AccountRetrievalRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(nullable: false),
                        SecondaryToken = c.Guid(nullable: false),
                        Salt = c.String(nullable: false),
                        DateIssued = c.DateTime(nullable: false),
                        DateExpired = c.DateTime(nullable: false),
                        DateRetrieved = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AdTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(),
                        Placement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id)
                .ForeignKey("dbo.Placement", t => t.Placement_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.Placement_Id);
            
            CreateTable(
                "dbo.AnalyticsSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(maxLength: 50),
                        ActionName = c.String(maxLength: 50),
                        ActionDate = c.DateTime(nullable: false),
                        User = c.String(maxLength: 50),
                        IPAddress = c.String(maxLength: 50),
                        Source = c.String(maxLength: 50),
                        RequestUrl = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CampaignContentModelProperty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        Metatype = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Model_Id = c.Int(),
                        PropertyType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignContentModel", t => t.Model_Id)
                .ForeignKey("dbo.CampaignContentModelPropertyType", t => t.PropertyType_Id)
                .Index(t => t.Model_Id)
                .Index(t => t.PropertyType_Id);
            
            CreateTable(
                "dbo.CampaignContentModelPropertyType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CampaignContentModelPropertyValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberValue = c.Double(),
                        StringValue = c.String(),
                        BoolValue = c.Boolean(),
                        DateValue = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(),
                        Experience_Id = c.Int(),
                        Model_Id = c.Int(),
                        Instance_Id = c.Int(),
                        Property_Id = c.Int(),
                        PropertyType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id)
                .ForeignKey("dbo.CampaignContentModel", t => t.Model_Id)
                .ForeignKey("dbo.CampaignContentModelInstance", t => t.Instance_Id)
                .ForeignKey("dbo.CampaignContentModelProperty", t => t.Property_Id)
                .ForeignKey("dbo.CampaignContentModelPropertyType", t => t.PropertyType_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.Experience_Id)
                .Index(t => t.Model_Id)
                .Index(t => t.Instance_Id)
                .Index(t => t.Property_Id)
                .Index(t => t.PropertyType_Id);
            
            CreateTable(
                "dbo.CampaignContentSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(maxLength: 100),
                        Content = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Schema_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignContentSchema", t => t.Schema_Id, cascadeDelete: true)
                .Index(t => t.Schema_Id);
            
            CreateTable(
                "dbo.CampaignImport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfWeeks = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Metadata = c.String(),
                        ImportVersion = c.String(maxLength: 10),
                        ImportFileName = c.String(maxLength: 200),
                        ImportedBy = c.String(maxLength: 100),
                        ImportDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(),
                        Experience_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id)
                .ForeignKey("dbo.Experience", t => t.Experience_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.Experience_Id);
            
            CreateTable(
                "dbo.ConfigSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        Key = c.String(),
                        ValueType = c.String(),
                        Value = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Extension = c.String(),
                        LastWriteTime = c.DateTime(nullable: false),
                        FullNameRaw = c.String(),
                        Length = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Logger = c.String(),
                        Message = c.String(),
                        Level = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductLineBlueprint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderIndex = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Blueprint_Id = c.Int(nullable: false),
                        ProductLine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductLine", t => t.ProductLine_Id, cascadeDelete: true)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.ProductLine_Id);
            
            CreateTable(
                "dbo.Recommendation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        ExperienceType_Id = c.Int(nullable: false),
                        TargetObjective_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExperienceType", t => t.ExperienceType_Id, cascadeDelete: true)
                .ForeignKey("dbo.TargetObjective", t => t.TargetObjective_Id, cascadeDelete: true)
                .Index(t => t.ExperienceType_Id)
                .Index(t => t.TargetObjective_Id);
            
            CreateTable(
                "dbo.Target",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdActionCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdActionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AdActionCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdActionCategory", t => t.AdActionCategory_Id, cascadeDelete: true)
                .Index(t => t.AdActionCategory_Id);
            
            CreateTable(
                "dbo.AdTypeGroupAdType",
                c => new
                    {
                        AdTypeGroup_Id = c.Int(nullable: false),
                        AdType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdTypeGroup_Id, t.AdType_Id })
                .ForeignKey("dbo.AdTypeGroup", t => t.AdTypeGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdType", t => t.AdType_Id, cascadeDelete: true)
                .Index(t => t.AdTypeGroup_Id)
                .Index(t => t.AdType_Id);
            
            CreateTable(
                "dbo.FeatureCategoryFeatureType",
                c => new
                    {
                        FeatureCategory_Id = c.Int(nullable: false),
                        FeatureType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FeatureCategory_Id, t.FeatureType_Id })
                .ForeignKey("dbo.FeatureCategory", t => t.FeatureCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id, cascadeDelete: true)
                .Index(t => t.FeatureCategory_Id)
                .Index(t => t.FeatureType_Id);
            
            CreateTable(
                "dbo.BlueprintFeatureCategory",
                c => new
                    {
                        Blueprint_Id = c.Int(nullable: false),
                        FeatureCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Blueprint_Id, t.FeatureCategory_Id })
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id, cascadeDelete: true)
                .ForeignKey("dbo.FeatureCategory", t => t.FeatureCategory_Id, cascadeDelete: true)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.FeatureCategory_Id);
            
            CreateTable(
                "dbo.PublisherPlatform",
                c => new
                    {
                        Publisher_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Publisher_Id, t.Platform_Id })
                .ForeignKey("dbo.Publisher", t => t.Publisher_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .Index(t => t.Publisher_Id)
                .Index(t => t.Platform_Id);
            
            CreateTable(
                "dbo.ExperienceTypePlatform",
                c => new
                    {
                        ExperienceType_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExperienceType_Id, t.Platform_Id })
                .ForeignKey("dbo.ExperienceType", t => t.ExperienceType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .Index(t => t.ExperienceType_Id)
                .Index(t => t.Platform_Id);
            
            CreateTable(
                "dbo.ChecklistOwnerChecklistStep",
                c => new
                    {
                        ChecklistOwner_Id = c.Int(nullable: false),
                        ChecklistStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistOwner_Id, t.ChecklistStep_Id })
                .ForeignKey("dbo.ChecklistOwner", t => t.ChecklistOwner_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChecklistStep", t => t.ChecklistStep_Id, cascadeDelete: true)
                .Index(t => t.ChecklistOwner_Id)
                .Index(t => t.ChecklistStep_Id);
            
            CreateTable(
                "dbo.LaunchStepChecklistOwner",
                c => new
                    {
                        LaunchStep_Id = c.Int(nullable: false),
                        ChecklistOwner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LaunchStep_Id, t.ChecklistOwner_Id })
                .ForeignKey("dbo.LaunchStep", t => t.LaunchStep_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChecklistOwner", t => t.ChecklistOwner_Id, cascadeDelete: true)
                .Index(t => t.LaunchStep_Id)
                .Index(t => t.ChecklistOwner_Id);
            
            CreateTable(
                "dbo.LaunchStepResourceDoc",
                c => new
                    {
                        LaunchStep_Id = c.Int(nullable: false),
                        ResourceDoc_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LaunchStep_Id, t.ResourceDoc_Id })
                .ForeignKey("dbo.LaunchStep", t => t.LaunchStep_Id, cascadeDelete: true)
                .ForeignKey("dbo.ResourceDoc", t => t.ResourceDoc_Id, cascadeDelete: true)
                .Index(t => t.LaunchStep_Id)
                .Index(t => t.ResourceDoc_Id);
            
            CreateTable(
                "dbo.ChecklistStepResourceDoc",
                c => new
                    {
                        ChecklistStep_Id = c.Int(nullable: false),
                        ResourceDoc_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistStep_Id, t.ResourceDoc_Id })
                .ForeignKey("dbo.ChecklistStep", t => t.ChecklistStep_Id, cascadeDelete: true)
                .ForeignKey("dbo.ResourceDoc", t => t.ResourceDoc_Id, cascadeDelete: true)
                .Index(t => t.ChecklistStep_Id)
                .Index(t => t.ResourceDoc_Id);
            
            CreateTable(
                "dbo.FeatureExperience",
                c => new
                    {
                        Feature_Id = c.Int(nullable: false),
                        Experience_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feature_Id, t.Experience_Id })
                .ForeignKey("dbo.Feature", t => t.Feature_Id, cascadeDelete: true)
                .ForeignKey("dbo.Experience", t => t.Experience_Id, cascadeDelete: true)
                .Index(t => t.Feature_Id)
                .Index(t => t.Experience_Id);
            
            CreateTable(
                "dbo.CampaignUser",
                c => new
                    {
                        Campaign_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Campaign_Id, t.User_Id })
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Campaign_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.RoleUser",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Role", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ResourceDoc",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExecutionStage_Id = c.Int(),
                        ProductionStep_Id = c.Int(),
                        Platform_Id = c.Int(),
                        ResourceType_Id = c.Int(),
                        IsAllPlatforms = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resource", t => t.Id)
                .ForeignKey("dbo.ExecutionStage", t => t.ExecutionStage_Id)
                .ForeignKey("dbo.ProductionStep", t => t.ProductionStep_Id)
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .ForeignKey("dbo.ResourceType", t => t.ResourceType_Id)
                .Index(t => t.Id)
                .Index(t => t.ExecutionStage_Id)
                .Index(t => t.ProductionStep_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.ResourceType_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ResourceDoc", new[] { "ResourceType_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "Platform_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "ProductionStep_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "Id" });
            DropIndex("dbo.RoleUser", new[] { "User_Id" });
            DropIndex("dbo.RoleUser", new[] { "Role_Id" });
            DropIndex("dbo.CampaignUser", new[] { "User_Id" });
            DropIndex("dbo.CampaignUser", new[] { "Campaign_Id" });
            DropIndex("dbo.FeatureExperience", new[] { "Experience_Id" });
            DropIndex("dbo.FeatureExperience", new[] { "Feature_Id" });
            DropIndex("dbo.ChecklistStepResourceDoc", new[] { "ResourceDoc_Id" });
            DropIndex("dbo.ChecklistStepResourceDoc", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.LaunchStepResourceDoc", new[] { "ResourceDoc_Id" });
            DropIndex("dbo.LaunchStepResourceDoc", new[] { "LaunchStep_Id" });
            DropIndex("dbo.LaunchStepChecklistOwner", new[] { "ChecklistOwner_Id" });
            DropIndex("dbo.LaunchStepChecklistOwner", new[] { "LaunchStep_Id" });
            DropIndex("dbo.ChecklistOwnerChecklistStep", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.ChecklistOwnerChecklistStep", new[] { "ChecklistOwner_Id" });
            DropIndex("dbo.ExperienceTypePlatform", new[] { "Platform_Id" });
            DropIndex("dbo.ExperienceTypePlatform", new[] { "ExperienceType_Id" });
            DropIndex("dbo.PublisherPlatform", new[] { "Platform_Id" });
            DropIndex("dbo.PublisherPlatform", new[] { "Publisher_Id" });
            DropIndex("dbo.BlueprintFeatureCategory", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.BlueprintFeatureCategory", new[] { "Blueprint_Id" });
            DropIndex("dbo.FeatureCategoryFeatureType", new[] { "FeatureType_Id" });
            DropIndex("dbo.FeatureCategoryFeatureType", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.AdTypeGroupAdType", new[] { "AdType_Id" });
            DropIndex("dbo.AdTypeGroupAdType", new[] { "AdTypeGroup_Id" });
            DropIndex("dbo.AdActionType", new[] { "AdActionCategory_Id" });
            DropIndex("dbo.Recommendation", new[] { "TargetObjective_Id" });
            DropIndex("dbo.Recommendation", new[] { "ExperienceType_Id" });
            DropIndex("dbo.ProductLineBlueprint", new[] { "ProductLine_Id" });
            DropIndex("dbo.ProductLineBlueprint", new[] { "Blueprint_Id" });
            DropIndex("dbo.CampaignImport", new[] { "Experience_Id" });
            DropIndex("dbo.CampaignImport", new[] { "Campaign_Id" });
            DropIndex("dbo.CampaignContentSettings", new[] { "Schema_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "PropertyType_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Property_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Instance_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Model_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Experience_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Campaign_Id" });
            DropIndex("dbo.CampaignContentModelProperty", new[] { "PropertyType_Id" });
            DropIndex("dbo.CampaignContentModelProperty", new[] { "Model_Id" });
            DropIndex("dbo.AdTag", new[] { "Placement_Id" });
            DropIndex("dbo.AdTag", new[] { "Ad_Id" });
            DropIndex("dbo.AccountRetrievalRequest", new[] { "User_Id" });
            DropIndex("dbo.PasswordHashHistory", new[] { "User_Id" });
            DropIndex("dbo.AdBuildRequest", new[] { "PublishedBy_Id" });
            DropIndex("dbo.AdBuildRequest", new[] { "Campaign_Id" });
            DropIndex("dbo.ExperienceCandidate", new[] { "DistributionPoint_Id" });
            DropIndex("dbo.ExperienceCandidate", new[] { "Campaign_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Target_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Objective_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Campaign_Id" });
            DropIndex("dbo.Metric", new[] { "MeasurementPlan_Id" });
            DropIndex("dbo.MeasurementPlan", new[] { "Id" });
            DropIndex("dbo.CampaignContentModel", new[] { "Schema_Id" });
            DropIndex("dbo.CampaignContentModel", new[] { "BaseType_Id" });
            DropIndex("dbo.CampaignContentModelInstance", new[] { "Parent_Id" });
            DropIndex("dbo.CampaignContentModelInstance", new[] { "Type_Id" });
            DropIndex("dbo.CampaignContentModelInstance", new[] { "Format_Id" });
            DropIndex("dbo.CampaignContentModelInstance", new[] { "Model_Id" });
            DropIndex("dbo.CampaignContentModelInstance", new[] { "Schema_Id" });
            DropIndex("dbo.CampaignContentSchema", new[] { "Campaign_Id" });
            DropIndex("dbo.ExperienceDownloadHistory", new[] { "Experience_Id" });
            DropIndex("dbo.NetworkPlatformPlacement", new[] { "Placement_Id" });
            DropIndex("dbo.NetworkPlatformPlacement", new[] { "Platform_Id" });
            DropIndex("dbo.NetworkPlatformPlacement", new[] { "Network_Id" });
            DropIndex("dbo.PlatformPlacement", new[] { "Placement_Id" });
            DropIndex("dbo.PlatformPlacement", new[] { "Platform_Id" });
            DropIndex("dbo.Placement", new[] { "Category_Id" });
            DropIndex("dbo.Placement", new[] { "AdTypeGroup_Id" });
            DropIndex("dbo.AppPlatform", new[] { "Category_Id" });
            DropIndex("dbo.AppPlatform", new[] { "Platform_Id" });
            DropIndex("dbo.AppPlatform", new[] { "App_Id" });
            DropIndex("dbo.AppPlatformPlacement", new[] { "Placement_Id" });
            DropIndex("dbo.AppPlatformPlacement", new[] { "AppPlatform_Id" });
            DropIndex("dbo.Creative", new[] { "Campaign_Id" });
            DropIndex("dbo.Creative", new[] { "Resource_Id" });
            DropIndex("dbo.Creative", new[] { "Blueprint_Id" });
            DropIndex("dbo.Element", new[] { "Page_Id" });
            DropIndex("dbo.ImportRFIResult", new[] { "Import_Id" });
            DropIndex("dbo.ImportRFIResult", new[] { "Ad_Id" });
            DropIndex("dbo.ImportMessageLog", new[] { "Import_Id" });
            DropIndex("dbo.ImportConfig", new[] { "Import_Id" });
            DropIndex("dbo.ImportConfig", new[] { "AdType_Id" });
            DropIndex("dbo.Flight", new[] { "ProductLine_Id" });
            DropIndex("dbo.Flight", new[] { "Experience_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Flight_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Page_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Import_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Ad_Id" });
            DropIndex("dbo.Import", new[] { "Experience_Id" });
            DropIndex("dbo.RFIResult", new[] { "Page_Id" });
            DropIndex("dbo.RFIResult", new[] { "ImportPage_Id" });
            DropIndex("dbo.RFIResult", new[] { "Import_Id" });
            DropIndex("dbo.RFIResult", new[] { "Ad_Id" });
            DropIndex("dbo.Page", new[] { "Feature_Id" });
            DropIndex("dbo.Page", new[] { "FeatureAd_Id" });
            DropIndex("dbo.Tag", new[] { "Page_Id" });
            DropIndex("dbo.Tag", new[] { "FeatureType_Id" });
            DropIndex("dbo.BlueprintPlatform", new[] { "Creative_Id" });
            DropIndex("dbo.BlueprintPlatform", new[] { "Platform_Id" });
            DropIndex("dbo.BlueprintPlatform", new[] { "Blueprint_Id" });
            DropIndex("dbo.ChecklistMilestone", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.LaunchMilestone", new[] { "LaunchStep_Id" });
            DropIndex("dbo.Launch", new[] { "Checklist_Id" });
            DropIndex("dbo.Launch", new[] { "Experience_Id" });
            DropIndex("dbo.LaunchStep", new[] { "ProductionStep_Id" });
            DropIndex("dbo.LaunchStep", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.LaunchStep", new[] { "Launch_Id" });
            DropIndex("dbo.Checklist", new[] { "ExperienceType_Id" });
            DropIndex("dbo.Checklist", new[] { "Platform_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "ProductionStep_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "Checklist_Id" });
            DropIndex("dbo.App", new[] { "Category_Id" });
            DropIndex("dbo.App", new[] { "Advertiser_Id" });
            DropIndex("dbo.App", new[] { "Publisher_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Network_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Publisher_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "App_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Platform_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "ExperienceType_Id" });
            DropIndex("dbo.Publisher", new[] { "Category_Id" });
            DropIndex("dbo.Publisher", new[] { "Parent_Id" });
            DropIndex("dbo.Platform", new[] { "PlatformGroup_Id" });
            DropIndex("dbo.Platform", new[] { "Benchmark_Id" });
            DropIndex("dbo.Resource", new[] { "User_Id" });
            DropIndex("dbo.Blueprint", new[] { "ConnectedTVSupport_Id" });
            DropIndex("dbo.Blueprint", new[] { "ConnectedTVCreative_Id" });
            DropIndex("dbo.Blueprint", new[] { "Preview_Id" });
            DropIndex("dbo.Blueprint", new[] { "FeatureType_Id" });
            DropIndex("dbo.FeatureType", new[] { "FeatureTypeGroup_Id" });
            DropIndex("dbo.Feature", new[] { "Blueprint_Id" });
            DropIndex("dbo.Feature", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.Feature", new[] { "FeatureType_Id" });
            DropIndex("dbo.Feature", new[] { "Campaign_Id" });
            DropIndex("dbo.FeatureAd", new[] { "Ad_Id" });
            DropIndex("dbo.FeatureAd", new[] { "Feature_Id" });
            DropIndex("dbo.FeatureAdPage", new[] { "Page_Id" });
            DropIndex("dbo.FeatureAdPage", new[] { "FeatureAd_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventMethod_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventLabel_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventType_Id" });
            DropIndex("dbo.AdResult", new[] { "Element_Id" });
            DropIndex("dbo.AdResult", new[] { "Flight_Id" });
            DropIndex("dbo.AdResult", new[] { "FeatureAdPage_Id" });
            DropIndex("dbo.AdResult", new[] { "Import_Id" });
            DropIndex("dbo.AdResult", new[] { "Page_Id" });
            DropIndex("dbo.AdResult", new[] { "Ad_Id" });
            DropIndex("dbo.Ad", new[] { "AdFormat_Id" });
            DropIndex("dbo.Ad", new[] { "AdFunction_Id" });
            DropIndex("dbo.Ad", new[] { "AdTypeGroup_Id" });
            DropIndex("dbo.Ad", new[] { "AdGroup_Id" });
            DropIndex("dbo.Ad", new[] { "Metadata_Id" });
            DropIndex("dbo.Ad", new[] { "NetworkPlacement_Id" });
            DropIndex("dbo.Ad", new[] { "PlatformPlacement_Id" });
            DropIndex("dbo.Ad", new[] { "AppPlatformPlacement_Id" });
            DropIndex("dbo.Ad", new[] { "Creative_Id" });
            DropIndex("dbo.Ad", new[] { "Flight_Id" });
            DropIndex("dbo.Ad", new[] { "Experience_Id" });
            DropIndex("dbo.Ad", new[] { "AdType_Id" });
            DropIndex("dbo.IncomeDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.EthnicityDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.AgeDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.Audience", new[] { "Target_Id" });
            DropIndex("dbo.Audience", new[] { "GenderDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "DistributionPoint_Id" });
            DropIndex("dbo.Experience", new[] { "Publisher_Id" });
            DropIndex("dbo.Experience", new[] { "ExperienceType_Id" });
            DropIndex("dbo.Experience", new[] { "Launch_Id" });
            DropIndex("dbo.Experience", new[] { "Platform_Id" });
            DropIndex("dbo.Experience", new[] { "Campaign_Id" });
            DropIndex("dbo.Experience", new[] { "Audience_Id" });
            DropIndex("dbo.Experience", new[] { "IncomeDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "GenderDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "EthnicityDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "AgeDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "Country_Id" });
            DropIndex("dbo.Campaign", new[] { "Thumbnail_Id" });
            DropIndex("dbo.Campaign", new[] { "Audience_Id" });
            DropIndex("dbo.Campaign", new[] { "Product_Id" });
            DropIndex("dbo.Vertical", new[] { "Benchmark_Id" });
            DropIndex("dbo.Segment", new[] { "Vertical_Id" });
            DropIndex("dbo.SubSegment", new[] { "Segment_Id" });
            DropIndex("dbo.Product", new[] { "SubSegment_Id" });
            DropIndex("dbo.Product", new[] { "Brand_Id" });
            DropIndex("dbo.Brand", new[] { "Advertiser_Id" });
            DropIndex("dbo.User", new[] { "Advertiser_Id" });
            DropIndex("dbo.AccountInvitation", new[] { "CreatingUser_Id" });
            DropIndex("dbo.AccountInvitation", new[] { "InvitedUser_Id" });
            DropIndex("dbo.AccountInvitation", new[] { "User_Id" });
            DropForeignKey("dbo.ResourceDoc", "ResourceType_Id", "dbo.ResourceType");
            DropForeignKey("dbo.ResourceDoc", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.ResourceDoc", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.ResourceDoc", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.ResourceDoc", "Id", "dbo.Resource");
            DropForeignKey("dbo.RoleUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.RoleUser", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.CampaignUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.CampaignUser", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.FeatureExperience", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.FeatureExperience", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.ChecklistStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc");
            DropForeignKey("dbo.ChecklistStepResourceDoc", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.LaunchStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc");
            DropForeignKey("dbo.LaunchStepResourceDoc", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.LaunchStepChecklistOwner", "ChecklistOwner_Id", "dbo.ChecklistOwner");
            DropForeignKey("dbo.LaunchStepChecklistOwner", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistOwner_Id", "dbo.ChecklistOwner");
            DropForeignKey("dbo.ExperienceTypePlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.ExperienceTypePlatform", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.PublisherPlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.PublisherPlatform", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.BlueprintFeatureCategory", "FeatureCategory_Id", "dbo.FeatureCategory");
            DropForeignKey("dbo.BlueprintFeatureCategory", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.FeatureCategoryFeatureType", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.FeatureCategoryFeatureType", "FeatureCategory_Id", "dbo.FeatureCategory");
            DropForeignKey("dbo.AdTypeGroupAdType", "AdType_Id", "dbo.AdType");
            DropForeignKey("dbo.AdTypeGroupAdType", "AdTypeGroup_Id", "dbo.AdTypeGroup");
            DropForeignKey("dbo.AdActionType", "AdActionCategory_Id", "dbo.AdActionCategory");
            DropForeignKey("dbo.Recommendation", "TargetObjective_Id", "dbo.TargetObjective");
            DropForeignKey("dbo.Recommendation", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.ProductLineBlueprint", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.ProductLineBlueprint", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.CampaignImport", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.CampaignImport", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignContentSettings", "Schema_Id", "dbo.CampaignContentSchema");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "PropertyType_Id", "dbo.CampaignContentModelPropertyType");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Property_Id", "dbo.CampaignContentModelProperty");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Instance_Id", "dbo.CampaignContentModelInstance");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Model_Id", "dbo.CampaignContentModel");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignContentModelProperty", "PropertyType_Id", "dbo.CampaignContentModelPropertyType");
            DropForeignKey("dbo.CampaignContentModelProperty", "Model_Id", "dbo.CampaignContentModel");
            DropForeignKey("dbo.AdTag", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.AccountRetrievalRequest", "User_Id", "dbo.User");
            DropForeignKey("dbo.PasswordHashHistory", "User_Id", "dbo.User");
            DropForeignKey("dbo.AdBuildRequest", "PublishedBy_Id", "dbo.User");
            DropForeignKey("dbo.AdBuildRequest", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.ExperienceCandidate", "DistributionPoint_Id", "dbo.DistributionPoint");
            DropForeignKey("dbo.ExperienceCandidate", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignObjective", "Target_Id", "dbo.Target");
            DropForeignKey("dbo.CampaignObjective", "Objective_Id", "dbo.TargetObjective");
            DropForeignKey("dbo.CampaignObjective", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.Metric", "MeasurementPlan_Id", "dbo.MeasurementPlan");
            DropForeignKey("dbo.MeasurementPlan", "Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignContentModel", "Schema_Id", "dbo.CampaignContentSchema");
            DropForeignKey("dbo.CampaignContentModel", "BaseType_Id", "dbo.CampaignContentModelBaseType");
            DropForeignKey("dbo.CampaignContentModelInstance", "Parent_Id", "dbo.CampaignContentModelInstance");
            DropForeignKey("dbo.CampaignContentModelInstance", "Type_Id", "dbo.CampaignContentModelType");
            DropForeignKey("dbo.CampaignContentModelInstance", "Format_Id", "dbo.CampaignContentModelBaseType");
            DropForeignKey("dbo.CampaignContentModelInstance", "Model_Id", "dbo.CampaignContentModel");
            DropForeignKey("dbo.CampaignContentModelInstance", "Schema_Id", "dbo.CampaignContentSchema");
            DropForeignKey("dbo.CampaignContentSchema", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.ExperienceDownloadHistory", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.PlatformPlacement", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.PlatformPlacement", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Placement", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Placement", "AdTypeGroup_Id", "dbo.AdTypeGroup");
            DropForeignKey("dbo.AppPlatform", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.AppPlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.AppPlatform", "App_Id", "dbo.App");
            DropForeignKey("dbo.AppPlatformPlacement", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.AppPlatformPlacement", "AppPlatform_Id", "dbo.AppPlatform");
            DropForeignKey("dbo.Creative", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.Creative", "Resource_Id", "dbo.Resource");
            DropForeignKey("dbo.Creative", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.Element", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.ImportRFIResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportRFIResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.ImportMessageLog", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportConfig", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportConfig", "AdType_Id", "dbo.AdType");
            DropForeignKey("dbo.Flight", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.Flight", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.ImportAdResult", "Flight_Id", "dbo.Flight");
            DropForeignKey("dbo.ImportAdResult", "Page_Id", "dbo.ImportPage");
            DropForeignKey("dbo.ImportAdResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportAdResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.Import", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.RFIResult", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.RFIResult", "ImportPage_Id", "dbo.ImportPage");
            DropForeignKey("dbo.RFIResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.RFIResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.Page", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.Page", "FeatureAd_Id", "dbo.FeatureAd");
            DropForeignKey("dbo.Tag", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.Tag", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.BlueprintPlatform", "Creative_Id", "dbo.Resource");
            DropForeignKey("dbo.BlueprintPlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.BlueprintPlatform", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.ChecklistMilestone", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.LaunchMilestone", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.Launch", "Checklist_Id", "dbo.Checklist");
            DropForeignKey("dbo.Launch", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.LaunchStep", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.LaunchStep", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.LaunchStep", "Launch_Id", "dbo.Launch");
            DropForeignKey("dbo.Checklist", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.Checklist", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.ChecklistStep", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.ChecklistStep", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.ChecklistStep", "Checklist_Id", "dbo.Checklist");
            DropForeignKey("dbo.App", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.App", "Advertiser_Id", "dbo.Advertiser");
            DropForeignKey("dbo.App", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.DistributionPoint", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.DistributionPoint", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.DistributionPoint", "App_Id", "dbo.App");
            DropForeignKey("dbo.DistributionPoint", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.DistributionPoint", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.Publisher", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Publisher", "Parent_Id", "dbo.Publisher");
            DropForeignKey("dbo.Platform", "PlatformGroup_Id", "dbo.PlatformGroup");
            DropForeignKey("dbo.Platform", "Benchmark_Id", "dbo.Benchmark");
            DropForeignKey("dbo.Resource", "User_Id", "dbo.User");
            DropForeignKey("dbo.Blueprint", "ConnectedTVSupport_Id", "dbo.Resource");
            DropForeignKey("dbo.Blueprint", "ConnectedTVCreative_Id", "dbo.Resource");
            DropForeignKey("dbo.Blueprint", "Preview_Id", "dbo.Resource");
            DropForeignKey("dbo.Blueprint", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.FeatureType", "FeatureTypeGroup_Id", "dbo.FeatureTypeGroup");
            DropForeignKey("dbo.Feature", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.Feature", "FeatureCategory_Id", "dbo.FeatureCategory");
            DropForeignKey("dbo.Feature", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.Feature", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.FeatureAd", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.FeatureAd", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.FeatureAdPage", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.FeatureAdPage", "FeatureAd_Id", "dbo.FeatureAd");
            DropForeignKey("dbo.AdResult", "AdEventMethod_Id", "dbo.AdEventMethod");
            DropForeignKey("dbo.AdResult", "AdEventLabel_Id", "dbo.AdEventLabel");
            DropForeignKey("dbo.AdResult", "AdEventType_Id", "dbo.AdEventType");
            DropForeignKey("dbo.AdResult", "Element_Id", "dbo.Element");
            DropForeignKey("dbo.AdResult", "Flight_Id", "dbo.Flight");
            DropForeignKey("dbo.AdResult", "FeatureAdPage_Id", "dbo.FeatureAdPage");
            DropForeignKey("dbo.AdResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.AdResult", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.AdResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.Ad", "AdFormat_Id", "dbo.AdFormat");
            DropForeignKey("dbo.Ad", "AdFunction_Id", "dbo.AdFunction");
            DropForeignKey("dbo.Ad", "AdTypeGroup_Id", "dbo.AdTypeGroup");
            DropForeignKey("dbo.Ad", "AdGroup_Id", "dbo.AdGroup");
            DropForeignKey("dbo.Ad", "Metadata_Id", "dbo.AdMetadata");
            DropForeignKey("dbo.Ad", "NetworkPlacement_Id", "dbo.NetworkPlatformPlacement");
            DropForeignKey("dbo.Ad", "PlatformPlacement_Id", "dbo.PlatformPlacement");
            DropForeignKey("dbo.Ad", "AppPlatformPlacement_Id", "dbo.AppPlatformPlacement");
            DropForeignKey("dbo.Ad", "Creative_Id", "dbo.Creative");
            DropForeignKey("dbo.Ad", "Flight_Id", "dbo.Flight");
            DropForeignKey("dbo.Ad", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.Ad", "AdType_Id", "dbo.AdType");
            DropForeignKey("dbo.IncomeDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.EthnicityDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.AgeDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Audience", "Target_Id", "dbo.Target");
            DropForeignKey("dbo.Audience", "GenderDemographic_Id", "dbo.GenderDemographic");
            DropForeignKey("dbo.Experience", "DistributionPoint_Id", "dbo.DistributionPoint");
            DropForeignKey("dbo.Experience", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.Experience", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.Experience", "Launch_Id", "dbo.Launch");
            DropForeignKey("dbo.Experience", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Experience", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.Experience", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Experience", "IncomeDemographic_Id", "dbo.IncomeDemographic");
            DropForeignKey("dbo.Experience", "GenderDemographic_Id", "dbo.GenderDemographic");
            DropForeignKey("dbo.Experience", "EthnicityDemographic_Id", "dbo.EthnicityDemographic");
            DropForeignKey("dbo.Experience", "AgeDemographic_Id", "dbo.AgeDemographic");
            DropForeignKey("dbo.Experience", "Country_Id", "dbo.Country");
            DropForeignKey("dbo.Campaign", "Thumbnail_Id", "dbo.Resource");
            DropForeignKey("dbo.Campaign", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Campaign", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.Vertical", "Benchmark_Id", "dbo.Benchmark");
            DropForeignKey("dbo.Segment", "Vertical_Id", "dbo.Vertical");
            DropForeignKey("dbo.SubSegment", "Segment_Id", "dbo.Segment");
            DropForeignKey("dbo.Product", "SubSegment_Id", "dbo.SubSegment");
            DropForeignKey("dbo.Product", "Brand_Id", "dbo.Brand");
            DropForeignKey("dbo.Brand", "Advertiser_Id", "dbo.Advertiser");
            DropForeignKey("dbo.User", "Advertiser_Id", "dbo.Advertiser");
            DropForeignKey("dbo.AccountInvitation", "CreatingUser_Id", "dbo.User");
            DropForeignKey("dbo.AccountInvitation", "InvitedUser_Id", "dbo.User");
            DropForeignKey("dbo.AccountInvitation", "User_Id", "dbo.User");
            DropTable("dbo.ResourceDoc");
            DropTable("dbo.RoleUser");
            DropTable("dbo.CampaignUser");
            DropTable("dbo.FeatureExperience");
            DropTable("dbo.ChecklistStepResourceDoc");
            DropTable("dbo.LaunchStepResourceDoc");
            DropTable("dbo.LaunchStepChecklistOwner");
            DropTable("dbo.ChecklistOwnerChecklistStep");
            DropTable("dbo.ExperienceTypePlatform");
            DropTable("dbo.PublisherPlatform");
            DropTable("dbo.BlueprintFeatureCategory");
            DropTable("dbo.FeatureCategoryFeatureType");
            DropTable("dbo.AdTypeGroupAdType");
            DropTable("dbo.AdActionType");
            DropTable("dbo.AdActionCategory");
            DropTable("dbo.Target");
            DropTable("dbo.Recommendation");
            DropTable("dbo.ProductLineBlueprint");
            DropTable("dbo.LogEntry");
            DropTable("dbo.FileItem");
            DropTable("dbo.ConfigSetting");
            DropTable("dbo.CampaignImport");
            DropTable("dbo.CampaignContentSettings");
            DropTable("dbo.CampaignContentModelPropertyValue");
            DropTable("dbo.CampaignContentModelPropertyType");
            DropTable("dbo.CampaignContentModelProperty");
            DropTable("dbo.AuditEvent");
            DropTable("dbo.AnalyticsSummary");
            DropTable("dbo.AdTag");
            DropTable("dbo.AccountRetrievalRequest");
            DropTable("dbo.PasswordHashHistory");
            DropTable("dbo.Role");
            DropTable("dbo.AdBuildRequest");
            DropTable("dbo.ExperienceCandidate");
            DropTable("dbo.TargetObjective");
            DropTable("dbo.CampaignObjective");
            DropTable("dbo.Metric");
            DropTable("dbo.MeasurementPlan");
            DropTable("dbo.CampaignContentModelType");
            DropTable("dbo.CampaignContentModelBaseType");
            DropTable("dbo.CampaignContentModel");
            DropTable("dbo.CampaignContentModelInstance");
            DropTable("dbo.CampaignContentSchema");
            DropTable("dbo.ExperienceDownloadHistory");
            DropTable("dbo.AdFormat");
            DropTable("dbo.AdFunction");
            DropTable("dbo.AdGroup");
            DropTable("dbo.AdMetadata");
            DropTable("dbo.NetworkPlatformPlacement");
            DropTable("dbo.PlatformPlacement");
            DropTable("dbo.Placement");
            DropTable("dbo.AppPlatform");
            DropTable("dbo.AppPlatformPlacement");
            DropTable("dbo.Creative");
            DropTable("dbo.AdEventMethod");
            DropTable("dbo.AdEventLabel");
            DropTable("dbo.AdEventType");
            DropTable("dbo.Element");
            DropTable("dbo.ImportRFIResult");
            DropTable("dbo.ImportMessageLog");
            DropTable("dbo.ImportConfig");
            DropTable("dbo.ProductLine");
            DropTable("dbo.Flight");
            DropTable("dbo.ImportPage");
            DropTable("dbo.ImportAdResult");
            DropTable("dbo.Import");
            DropTable("dbo.RFIResult");
            DropTable("dbo.Page");
            DropTable("dbo.Tag");
            DropTable("dbo.BlueprintPlatform");
            DropTable("dbo.ChecklistMilestone");
            DropTable("dbo.LaunchMilestone");
            DropTable("dbo.Launch");
            DropTable("dbo.LaunchStep");
            DropTable("dbo.ChecklistOwner");
            DropTable("dbo.Checklist");
            DropTable("dbo.ChecklistStep");
            DropTable("dbo.ResourceType");
            DropTable("dbo.Network");
            DropTable("dbo.App");
            DropTable("dbo.DistributionPoint");
            DropTable("dbo.ExperienceType");
            DropTable("dbo.Category");
            DropTable("dbo.Publisher");
            DropTable("dbo.PlatformGroup");
            DropTable("dbo.Platform");
            DropTable("dbo.ProductionStep");
            DropTable("dbo.ExecutionStage");
            DropTable("dbo.Resource");
            DropTable("dbo.Blueprint");
            DropTable("dbo.FeatureCategory");
            DropTable("dbo.FeatureTypeGroup");
            DropTable("dbo.FeatureType");
            DropTable("dbo.Feature");
            DropTable("dbo.FeatureAd");
            DropTable("dbo.FeatureAdPage");
            DropTable("dbo.AdResult");
            DropTable("dbo.AdTypeGroup");
            DropTable("dbo.AdType");
            DropTable("dbo.Ad");
            DropTable("dbo.IncomeDemographic");
            DropTable("dbo.GenderDemographic");
            DropTable("dbo.EthnicityDemographic");
            DropTable("dbo.AgeDemographic");
            DropTable("dbo.Audience");
            DropTable("dbo.Country");
            DropTable("dbo.Experience");
            DropTable("dbo.Campaign");
            DropTable("dbo.Benchmark");
            DropTable("dbo.Vertical");
            DropTable("dbo.Segment");
            DropTable("dbo.SubSegment");
            DropTable("dbo.Product");
            DropTable("dbo.Brand");
            DropTable("dbo.Advertiser");
            DropTable("dbo.User");
            DropTable("dbo.AccountInvitation");
        }
    }
}
