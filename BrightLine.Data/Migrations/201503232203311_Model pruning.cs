namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modelpruning : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vertical", "Benchmark_Id", "dbo.Benchmark");
            DropForeignKey("dbo.Campaign", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Experience", "Country_Id", "dbo.Country");
            DropForeignKey("dbo.Experience", "AgeDemographic_Id", "dbo.AgeDemographic");
            DropForeignKey("dbo.Experience", "EthnicityDemographic_Id", "dbo.EthnicityDemographic");
            DropForeignKey("dbo.Experience", "GenderDemographic_Id", "dbo.GenderDemographic");
            DropForeignKey("dbo.Experience", "IncomeDemographic_Id", "dbo.IncomeDemographic");
            DropForeignKey("dbo.Experience", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Experience", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.Experience", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Experience", "Launch_Id", "dbo.Launch");
            DropForeignKey("dbo.Experience", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.Experience", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.Experience", "DistributionPoint_Id", "dbo.DistributionPoint");
            DropForeignKey("dbo.Audience", "GenderDemographic_Id", "dbo.GenderDemographic");
            DropForeignKey("dbo.Audience", "Target_Id", "dbo.Target");
            DropForeignKey("dbo.AgeDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.EthnicityDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.IncomeDemographic", "Audience_Id", "dbo.Audience");
            DropForeignKey("dbo.Ad", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.Ad", "Metadata_Id", "dbo.AdMetadata");
            DropForeignKey("dbo.AdResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.AdResult", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.AdResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.AdResult", "FeatureAdPage_Id", "dbo.FeatureAdPage");
            DropForeignKey("dbo.AdResult", "Element_Id", "dbo.Element");
            DropForeignKey("dbo.AdResult", "AdEventType_Id", "dbo.AdEventType");
            DropForeignKey("dbo.AdResult", "AdEventLabel_Id", "dbo.AdEventLabel");
            DropForeignKey("dbo.AdResult", "AdEventMethod_Id", "dbo.AdEventMethod");
            DropForeignKey("dbo.FeatureAdPage", "FeatureAd_Id", "dbo.FeatureAd");
            DropForeignKey("dbo.FeatureAdPage", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.Platform", "Benchmark_Id", "dbo.Benchmark");
            DropForeignKey("dbo.DistributionPoint", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.DistributionPoint", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.DistributionPoint", "App_Id", "dbo.App");
            DropForeignKey("dbo.DistributionPoint", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.DistributionPoint", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.ChecklistStep", "Checklist_Id", "dbo.Checklist");
            DropForeignKey("dbo.ChecklistStep", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.ChecklistStep", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.Checklist", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Checklist", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.LaunchStep", "Launch_Id", "dbo.Launch");
            DropForeignKey("dbo.LaunchStep", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.LaunchStep", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.Launch", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.Launch", "Checklist_Id", "dbo.Checklist");
            DropForeignKey("dbo.LaunchMilestone", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.ChecklistMilestone", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.Tag", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.Tag", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.RFIResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.RFIResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.RFIResult", "ImportPage_Id", "dbo.ImportPage");
            DropForeignKey("dbo.RFIResult", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.Import", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.ImportAdResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.ImportAdResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportAdResult", "Page_Id", "dbo.ImportPage");
            DropForeignKey("dbo.ImportConfig", "AdType_Id", "dbo.AdType");
            DropForeignKey("dbo.ImportConfig", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportMessageLog", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.ImportRFIResult", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.ImportRFIResult", "Import_Id", "dbo.Import");
            DropForeignKey("dbo.Element", "Page_Id", "dbo.Page");
            DropForeignKey("dbo.ExperienceDownloadHistory", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.MeasurementPlan", "Id", "dbo.Campaign");
            DropForeignKey("dbo.Metric", "MeasurementPlan_Id", "dbo.MeasurementPlan");
            DropForeignKey("dbo.CampaignObjective", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignObjective", "Objective_Id", "dbo.TargetObjective");
            DropForeignKey("dbo.CampaignObjective", "Target_Id", "dbo.Target");
            DropForeignKey("dbo.ExperienceCandidate", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.ExperienceCandidate", "DistributionPoint_Id", "dbo.DistributionPoint");
            DropForeignKey("dbo.AdBuildRequest", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.AdBuildRequest", "PublishedBy_Id", "dbo.User");
            DropForeignKey("dbo.CampaignContentModelPropertyValue", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.CampaignImport", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CampaignImport", "Experience_Id", "dbo.Experience");
            DropForeignKey("dbo.Recommendation", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.Recommendation", "TargetObjective_Id", "dbo.TargetObjective");
            DropForeignKey("dbo.AdActionType", "AdActionCategory_Id", "dbo.AdActionCategory");
            DropForeignKey("dbo.ProductLineFeatureType", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.ProductLineFeatureType", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.ExperienceTypePlatform", "ExperienceType_Id", "dbo.ExperienceType");
            DropForeignKey("dbo.ExperienceTypePlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistOwner_Id", "dbo.ChecklistOwner");
            DropForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.LaunchStepChecklistOwner", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.LaunchStepChecklistOwner", "ChecklistOwner_Id", "dbo.ChecklistOwner");
            DropForeignKey("dbo.LaunchStepResourceDoc", "LaunchStep_Id", "dbo.LaunchStep");
            DropForeignKey("dbo.LaunchStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc");
            DropForeignKey("dbo.ChecklistStepResourceDoc", "ChecklistStep_Id", "dbo.ChecklistStep");
            DropForeignKey("dbo.ChecklistStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc");
            DropForeignKey("dbo.FeatureExperience", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.FeatureExperience", "Experience_Id", "dbo.Experience");
            DropIndex("dbo.Vertical", new[] { "Benchmark_Id" });
            DropIndex("dbo.Campaign", new[] { "Audience_Id" });
            DropIndex("dbo.Experience", new[] { "Country_Id" });
            DropIndex("dbo.Experience", new[] { "AgeDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "EthnicityDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "GenderDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "IncomeDemographic_Id" });
            DropIndex("dbo.Experience", new[] { "Audience_Id" });
            DropIndex("dbo.Experience", new[] { "Campaign_Id" });
            DropIndex("dbo.Experience", new[] { "Platform_Id" });
            DropIndex("dbo.Experience", new[] { "Launch_Id" });
            DropIndex("dbo.Experience", new[] { "ExperienceType_Id" });
            DropIndex("dbo.Experience", new[] { "Publisher_Id" });
            DropIndex("dbo.Experience", new[] { "DistributionPoint_Id" });
            DropIndex("dbo.Audience", new[] { "GenderDemographic_Id" });
            DropIndex("dbo.Audience", new[] { "Target_Id" });
            DropIndex("dbo.AgeDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.EthnicityDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.IncomeDemographic", new[] { "Audience_Id" });
            DropIndex("dbo.Ad", new[] { "Experience_Id" });
            DropIndex("dbo.Ad", new[] { "Metadata_Id" });
            DropIndex("dbo.AdResult", new[] { "Ad_Id" });
            DropIndex("dbo.AdResult", new[] { "Page_Id" });
            DropIndex("dbo.AdResult", new[] { "Import_Id" });
            DropIndex("dbo.AdResult", new[] { "FeatureAdPage_Id" });
            DropIndex("dbo.AdResult", new[] { "Element_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventType_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventLabel_Id" });
            DropIndex("dbo.AdResult", new[] { "AdEventMethod_Id" });
            DropIndex("dbo.FeatureAdPage", new[] { "FeatureAd_Id" });
            DropIndex("dbo.FeatureAdPage", new[] { "Page_Id" });
            DropIndex("dbo.Platform", new[] { "Benchmark_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "ExperienceType_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Platform_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "App_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Publisher_Id" });
            DropIndex("dbo.DistributionPoint", new[] { "Network_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "Checklist_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.ChecklistStep", new[] { "ProductionStep_Id" });
            DropIndex("dbo.Checklist", new[] { "Platform_Id" });
            DropIndex("dbo.Checklist", new[] { "ExperienceType_Id" });
            DropIndex("dbo.LaunchStep", new[] { "Launch_Id" });
            DropIndex("dbo.LaunchStep", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.LaunchStep", new[] { "ProductionStep_Id" });
            DropIndex("dbo.Launch", new[] { "Experience_Id" });
            DropIndex("dbo.Launch", new[] { "Checklist_Id" });
            DropIndex("dbo.LaunchMilestone", new[] { "LaunchStep_Id" });
            DropIndex("dbo.ChecklistMilestone", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.Tag", new[] { "FeatureType_Id" });
            DropIndex("dbo.Tag", new[] { "Page_Id" });
            DropIndex("dbo.RFIResult", new[] { "Ad_Id" });
            DropIndex("dbo.RFIResult", new[] { "Import_Id" });
            DropIndex("dbo.RFIResult", new[] { "ImportPage_Id" });
            DropIndex("dbo.RFIResult", new[] { "Page_Id" });
            DropIndex("dbo.Import", new[] { "Experience_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Ad_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Import_Id" });
            DropIndex("dbo.ImportAdResult", new[] { "Page_Id" });
            DropIndex("dbo.ImportConfig", new[] { "AdType_Id" });
            DropIndex("dbo.ImportConfig", new[] { "Import_Id" });
            DropIndex("dbo.ImportMessageLog", new[] { "Import_Id" });
            DropIndex("dbo.ImportRFIResult", new[] { "Ad_Id" });
            DropIndex("dbo.ImportRFIResult", new[] { "Import_Id" });
            DropIndex("dbo.Element", new[] { "Page_Id" });
            DropIndex("dbo.ExperienceDownloadHistory", new[] { "Experience_Id" });
            DropIndex("dbo.MeasurementPlan", new[] { "Id" });
            DropIndex("dbo.Metric", new[] { "MeasurementPlan_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Campaign_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Objective_Id" });
            DropIndex("dbo.CampaignObjective", new[] { "Target_Id" });
            DropIndex("dbo.ExperienceCandidate", new[] { "Campaign_Id" });
            DropIndex("dbo.ExperienceCandidate", new[] { "DistributionPoint_Id" });
            DropIndex("dbo.AdBuildRequest", new[] { "Campaign_Id" });
            DropIndex("dbo.AdBuildRequest", new[] { "PublishedBy_Id" });
            DropIndex("dbo.CampaignContentModelPropertyValue", new[] { "Experience_Id" });
            DropIndex("dbo.CampaignImport", new[] { "Campaign_Id" });
            DropIndex("dbo.CampaignImport", new[] { "Experience_Id" });
            DropIndex("dbo.Recommendation", new[] { "ExperienceType_Id" });
            DropIndex("dbo.Recommendation", new[] { "TargetObjective_Id" });
            DropIndex("dbo.AdActionType", new[] { "AdActionCategory_Id" });
            DropIndex("dbo.ProductLineFeatureType", new[] { "ProductLine_Id" });
            DropIndex("dbo.ProductLineFeatureType", new[] { "FeatureType_Id" });
            DropIndex("dbo.ExperienceTypePlatform", new[] { "ExperienceType_Id" });
            DropIndex("dbo.ExperienceTypePlatform", new[] { "Platform_Id" });
            DropIndex("dbo.ChecklistOwnerChecklistStep", new[] { "ChecklistOwner_Id" });
            DropIndex("dbo.ChecklistOwnerChecklistStep", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.LaunchStepChecklistOwner", new[] { "LaunchStep_Id" });
            DropIndex("dbo.LaunchStepChecklistOwner", new[] { "ChecklistOwner_Id" });
            DropIndex("dbo.LaunchStepResourceDoc", new[] { "LaunchStep_Id" });
            DropIndex("dbo.LaunchStepResourceDoc", new[] { "ResourceDoc_Id" });
            DropIndex("dbo.ChecklistStepResourceDoc", new[] { "ChecklistStep_Id" });
            DropIndex("dbo.ChecklistStepResourceDoc", new[] { "ResourceDoc_Id" });
            DropIndex("dbo.FeatureExperience", new[] { "Feature_Id" });
            DropIndex("dbo.FeatureExperience", new[] { "Experience_Id" });
            CreateTable(
                "dbo.FeatureTypeProductLine",
                c => new
                    {
                        FeatureType_Id = c.Int(nullable: false),
                        ProductLine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FeatureType_Id, t.ProductLine_Id })
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductLine", t => t.ProductLine_Id, cascadeDelete: true)
                .Index(t => t.FeatureType_Id)
                .Index(t => t.ProductLine_Id);
            
            CreateTable(
                "dbo.AdFeature",
                c => new
                    {
                        Ad_Id = c.Int(nullable: false),
                        Feature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ad_Id, t.Feature_Id })
                .ForeignKey("dbo.Ad", t => t.Ad_Id, cascadeDelete: true)
                .ForeignKey("dbo.Feature", t => t.Feature_Id, cascadeDelete: true)
                .Index(t => t.Ad_Id)
                .Index(t => t.Feature_Id);
            
            DropColumn("dbo.Vertical", "Benchmark_Id");
            DropColumn("dbo.Campaign", "Audience_Id");
            DropColumn("dbo.AgeDemographic", "Audience_Id");
            DropColumn("dbo.EthnicityDemographic", "Audience_Id");
            DropColumn("dbo.IncomeDemographic", "Audience_Id");
            DropColumn("dbo.Ad", "Experience_Id");
            DropColumn("dbo.Ad", "Metadata_Id");
            DropColumn("dbo.Platform", "Benchmark_Id");
            DropColumn("dbo.Metric", "MeasurementPlan_Id");
            DropColumn("dbo.CampaignContentModelPropertyValue", "Experience_Id");
            DropTable("dbo.Benchmark");
            //DropTable("dbo.Experience");
            DropTable("dbo.Audience");
            DropTable("dbo.AdResult");
            DropTable("dbo.FeatureAdPage");
            DropTable("dbo.ExperienceType");
            DropTable("dbo.DistributionPoint");
            DropTable("dbo.ChecklistStep");
            DropTable("dbo.Checklist");
            DropTable("dbo.ChecklistOwner");
            DropTable("dbo.LaunchStep");
            DropTable("dbo.Launch");
            DropTable("dbo.LaunchMilestone");
            DropTable("dbo.ChecklistMilestone");
            DropTable("dbo.Tag");
            DropTable("dbo.RFIResult");
            DropTable("dbo.Import");
            DropTable("dbo.ImportAdResult");
            DropTable("dbo.ImportPage");
            DropTable("dbo.ImportConfig");
            DropTable("dbo.ImportMessageLog");
            DropTable("dbo.ImportRFIResult");
            DropTable("dbo.Element");
            DropTable("dbo.AdEventType");
            DropTable("dbo.AdEventLabel");
            DropTable("dbo.AdEventMethod");
            DropTable("dbo.AdMetadata");
            DropTable("dbo.ExperienceDownloadHistory");
            DropTable("dbo.MeasurementPlan");
            DropTable("dbo.CampaignObjective");
            DropTable("dbo.TargetObjective");
            DropTable("dbo.ExperienceCandidate");
            DropTable("dbo.AdBuildRequest");
            DropTable("dbo.AnalyticsSummary");
            DropTable("dbo.CampaignImport");
            DropTable("dbo.Recommendation");
            DropTable("dbo.Target");
            DropTable("dbo.AdActionCategory");
            DropTable("dbo.AdActionType");
            DropTable("dbo.ProductLineFeatureType");
            DropTable("dbo.ExperienceTypePlatform");
            DropTable("dbo.ChecklistOwnerChecklistStep");
            DropTable("dbo.LaunchStepChecklistOwner");
            DropTable("dbo.LaunchStepResourceDoc");
            DropTable("dbo.ChecklistStepResourceDoc");
            DropTable("dbo.FeatureExperience");

			Sql(@"
if exists (select 1 from sys.tables where name='MiniProfilers')
	drop table [dbo].[MiniProfilers]
if exists (select 1 from sys.tables where name='MiniProfilerClientTimings')
	drop table [dbo].[MiniProfilerClientTimings]
if exists (select 1 from sys.tables where name='MiniProfilerTimings')
	drop table [dbo].[MiniProfilerTimings]
");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FeatureExperience",
                c => new
                    {
                        Feature_Id = c.Int(nullable: false),
                        Experience_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feature_Id, t.Experience_Id });
            
            CreateTable(
                "dbo.ChecklistStepResourceDoc",
                c => new
                    {
                        ChecklistStep_Id = c.Int(nullable: false),
                        ResourceDoc_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistStep_Id, t.ResourceDoc_Id });
            
            CreateTable(
                "dbo.LaunchStepResourceDoc",
                c => new
                    {
                        LaunchStep_Id = c.Int(nullable: false),
                        ResourceDoc_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LaunchStep_Id, t.ResourceDoc_Id });
            
            CreateTable(
                "dbo.LaunchStepChecklistOwner",
                c => new
                    {
                        LaunchStep_Id = c.Int(nullable: false),
                        ChecklistOwner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LaunchStep_Id, t.ChecklistOwner_Id });
            
            CreateTable(
                "dbo.ChecklistOwnerChecklistStep",
                c => new
                    {
                        ChecklistOwner_Id = c.Int(nullable: false),
                        ChecklistStep_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChecklistOwner_Id, t.ChecklistStep_Id });
            
            CreateTable(
                "dbo.ExperienceTypePlatform",
                c => new
                    {
                        ExperienceType_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExperienceType_Id, t.Platform_Id });
            
            CreateTable(
                "dbo.ProductLineFeatureType",
                c => new
                    {
                        ProductLine_Id = c.Int(nullable: false),
                        FeatureType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductLine_Id, t.FeatureType_Id });
            
            CreateTable(
                "dbo.FeatureAd1",
                c => new
                    {
                        Feature_Id = c.Int(nullable: false),
                        Ad_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feature_Id, t.Ad_Id });
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                        Element_Id = c.Int(),
                        AdEventType_Id = c.Int(),
                        AdEventLabel_Id = c.Int(),
                        AdEventMethod_Id = c.Int(),
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
                .PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.Experience",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Link = c.String(maxLength: 255, unicode: false),
			//			OldIdentity = c.Int(),
			//			MediaSpend = c.Int(nullable: false),
			//			BandwidthSpend = c.Int(nullable: false),
			//			EngagementRateIndex = c.Double(nullable: false),
			//			LaunchDate = c.DateTime(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Country_Id = c.Int(),
			//			AgeDemographic_Id = c.Int(),
			//			EthnicityDemographic_Id = c.Int(),
			//			GenderDemographic_Id = c.Int(),
			//			IncomeDemographic_Id = c.Int(),
			//			Audience_Id = c.Int(),
			//			Campaign_Id = c.Int(),
			//			Platform_Id = c.Int(),
			//			Launch_Id = c.Int(),
			//			ExperienceType_Id = c.Int(),
			//			Publisher_Id = c.Int(),
			//			DistributionPoint_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
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
            
            AddColumn("dbo.CampaignContentModelPropertyValue", "Experience_Id", c => c.Int());
            AddColumn("dbo.Metric", "MeasurementPlan_Id", c => c.Int());
            AddColumn("dbo.Platform", "Benchmark_Id", c => c.Int());
            AddColumn("dbo.Ad", "Metadata_Id", c => c.Int());
            AddColumn("dbo.Ad", "Experience_Id", c => c.Int(nullable: false));
            AddColumn("dbo.IncomeDemographic", "Audience_Id", c => c.Int());
            AddColumn("dbo.EthnicityDemographic", "Audience_Id", c => c.Int());
            AddColumn("dbo.AgeDemographic", "Audience_Id", c => c.Int());
            AddColumn("dbo.Campaign", "Audience_Id", c => c.Int());
            AddColumn("dbo.Vertical", "Benchmark_Id", c => c.Int());
            DropIndex("dbo.AdFeature", new[] { "Feature_Id" });
            DropIndex("dbo.AdFeature", new[] { "Ad_Id" });
            DropIndex("dbo.FeatureTypeProductLine", new[] { "ProductLine_Id" });
            DropIndex("dbo.FeatureTypeProductLine", new[] { "FeatureType_Id" });
            DropForeignKey("dbo.AdFeature", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.AdFeature", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.FeatureTypeProductLine", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.FeatureTypeProductLine", "FeatureType_Id", "dbo.FeatureType");
            DropTable("dbo.AdFeature");
            DropTable("dbo.FeatureTypeProductLine");
            CreateIndex("dbo.FeatureExperience", "Experience_Id");
            CreateIndex("dbo.FeatureExperience", "Feature_Id");
            CreateIndex("dbo.ChecklistStepResourceDoc", "ResourceDoc_Id");
            CreateIndex("dbo.ChecklistStepResourceDoc", "ChecklistStep_Id");
            CreateIndex("dbo.LaunchStepResourceDoc", "ResourceDoc_Id");
            CreateIndex("dbo.LaunchStepResourceDoc", "LaunchStep_Id");
            CreateIndex("dbo.LaunchStepChecklistOwner", "ChecklistOwner_Id");
            CreateIndex("dbo.LaunchStepChecklistOwner", "LaunchStep_Id");
            CreateIndex("dbo.ChecklistOwnerChecklistStep", "ChecklistStep_Id");
            CreateIndex("dbo.ChecklistOwnerChecklistStep", "ChecklistOwner_Id");
            CreateIndex("dbo.ExperienceTypePlatform", "Platform_Id");
            CreateIndex("dbo.ExperienceTypePlatform", "ExperienceType_Id");
            CreateIndex("dbo.ProductLineFeatureType", "FeatureType_Id");
            CreateIndex("dbo.ProductLineFeatureType", "ProductLine_Id");
            CreateIndex("dbo.FeatureAd1", "Ad_Id");
            CreateIndex("dbo.FeatureAd1", "Feature_Id");
            CreateIndex("dbo.AdActionType", "AdActionCategory_Id");
            CreateIndex("dbo.Recommendation", "TargetObjective_Id");
            CreateIndex("dbo.Recommendation", "ExperienceType_Id");
            CreateIndex("dbo.CampaignImport", "Experience_Id");
            CreateIndex("dbo.CampaignImport", "Campaign_Id");
            CreateIndex("dbo.CampaignContentModelPropertyValue", "Experience_Id");
            CreateIndex("dbo.AdBuildRequest", "PublishedBy_Id");
            CreateIndex("dbo.AdBuildRequest", "Campaign_Id");
            CreateIndex("dbo.ExperienceCandidate", "DistributionPoint_Id");
            CreateIndex("dbo.ExperienceCandidate", "Campaign_Id");
            CreateIndex("dbo.CampaignObjective", "Target_Id");
            CreateIndex("dbo.CampaignObjective", "Objective_Id");
            CreateIndex("dbo.CampaignObjective", "Campaign_Id");
            CreateIndex("dbo.Metric", "MeasurementPlan_Id");
            CreateIndex("dbo.MeasurementPlan", "Id");
            CreateIndex("dbo.ExperienceDownloadHistory", "Experience_Id");
            CreateIndex("dbo.Element", "Page_Id");
            CreateIndex("dbo.ImportRFIResult", "Import_Id");
            CreateIndex("dbo.ImportRFIResult", "Ad_Id");
            CreateIndex("dbo.ImportMessageLog", "Import_Id");
            CreateIndex("dbo.ImportConfig", "Import_Id");
            CreateIndex("dbo.ImportConfig", "AdType_Id");
            CreateIndex("dbo.ImportAdResult", "Page_Id");
            CreateIndex("dbo.ImportAdResult", "Import_Id");
            CreateIndex("dbo.ImportAdResult", "Ad_Id");
            CreateIndex("dbo.Import", "Experience_Id");
            CreateIndex("dbo.RFIResult", "Page_Id");
            CreateIndex("dbo.RFIResult", "ImportPage_Id");
            CreateIndex("dbo.RFIResult", "Import_Id");
            CreateIndex("dbo.RFIResult", "Ad_Id");
            CreateIndex("dbo.Tag", "Page_Id");
            CreateIndex("dbo.Tag", "FeatureType_Id");
            CreateIndex("dbo.ChecklistMilestone", "ChecklistStep_Id");
            CreateIndex("dbo.LaunchMilestone", "LaunchStep_Id");
            CreateIndex("dbo.Launch", "Checklist_Id");
            CreateIndex("dbo.Launch", "Experience_Id");
            CreateIndex("dbo.LaunchStep", "ProductionStep_Id");
            CreateIndex("dbo.LaunchStep", "ExecutionStage_Id");
            CreateIndex("dbo.LaunchStep", "Launch_Id");
            CreateIndex("dbo.Checklist", "ExperienceType_Id");
            CreateIndex("dbo.Checklist", "Platform_Id");
            CreateIndex("dbo.ChecklistStep", "ProductionStep_Id");
            CreateIndex("dbo.ChecklistStep", "ExecutionStage_Id");
            CreateIndex("dbo.ChecklistStep", "Checklist_Id");
            CreateIndex("dbo.DistributionPoint", "Network_Id");
            CreateIndex("dbo.DistributionPoint", "Publisher_Id");
            CreateIndex("dbo.DistributionPoint", "App_Id");
            CreateIndex("dbo.DistributionPoint", "Platform_Id");
            CreateIndex("dbo.DistributionPoint", "ExperienceType_Id");
            CreateIndex("dbo.Platform", "Benchmark_Id");
            CreateIndex("dbo.FeatureAdPage", "Page_Id");
            CreateIndex("dbo.FeatureAdPage", "FeatureAd_Id");
            CreateIndex("dbo.AdResult", "AdEventMethod_Id");
            CreateIndex("dbo.AdResult", "AdEventLabel_Id");
            CreateIndex("dbo.AdResult", "AdEventType_Id");
            CreateIndex("dbo.AdResult", "Element_Id");
            CreateIndex("dbo.AdResult", "FeatureAdPage_Id");
            CreateIndex("dbo.AdResult", "Import_Id");
            CreateIndex("dbo.AdResult", "Page_Id");
            CreateIndex("dbo.AdResult", "Ad_Id");
            CreateIndex("dbo.Ad", "Metadata_Id");
            CreateIndex("dbo.Ad", "Experience_Id");
            CreateIndex("dbo.IncomeDemographic", "Audience_Id");
            CreateIndex("dbo.EthnicityDemographic", "Audience_Id");
            CreateIndex("dbo.AgeDemographic", "Audience_Id");
            CreateIndex("dbo.Audience", "Target_Id");
            CreateIndex("dbo.Audience", "GenderDemographic_Id");
            CreateIndex("dbo.Experience", "DistributionPoint_Id");
            CreateIndex("dbo.Experience", "Publisher_Id");
            CreateIndex("dbo.Experience", "ExperienceType_Id");
            CreateIndex("dbo.Experience", "Launch_Id");
            CreateIndex("dbo.Experience", "Platform_Id");
            CreateIndex("dbo.Experience", "Campaign_Id");
            CreateIndex("dbo.Experience", "Audience_Id");
            CreateIndex("dbo.Experience", "IncomeDemographic_Id");
            CreateIndex("dbo.Experience", "GenderDemographic_Id");
            CreateIndex("dbo.Experience", "EthnicityDemographic_Id");
            CreateIndex("dbo.Experience", "AgeDemographic_Id");
            CreateIndex("dbo.Experience", "Country_Id");
            CreateIndex("dbo.Campaign", "Audience_Id");
            CreateIndex("dbo.Vertical", "Benchmark_Id");
            AddForeignKey("dbo.FeatureExperience", "Experience_Id", "dbo.Experience", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeatureExperience", "Feature_Id", "dbo.Feature", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistStepResourceDoc", "ChecklistStep_Id", "dbo.ChecklistStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStepResourceDoc", "ResourceDoc_Id", "dbo.ResourceDoc", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStepResourceDoc", "LaunchStep_Id", "dbo.LaunchStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStepChecklistOwner", "ChecklistOwner_Id", "dbo.ChecklistOwner", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStepChecklistOwner", "LaunchStep_Id", "dbo.LaunchStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistStep_Id", "dbo.ChecklistStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistOwnerChecklistStep", "ChecklistOwner_Id", "dbo.ChecklistOwner", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExperienceTypePlatform", "Platform_Id", "dbo.Platform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExperienceTypePlatform", "ExperienceType_Id", "dbo.ExperienceType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductLineFeatureType", "FeatureType_Id", "dbo.FeatureType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductLineFeatureType", "ProductLine_Id", "dbo.ProductLine", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeatureAd1", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeatureAd1", "Feature_Id", "dbo.Feature", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AdActionType", "AdActionCategory_Id", "dbo.AdActionCategory", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Recommendation", "TargetObjective_Id", "dbo.TargetObjective", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Recommendation", "ExperienceType_Id", "dbo.ExperienceType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CampaignImport", "Experience_Id", "dbo.Experience", "Id");
            AddForeignKey("dbo.CampaignImport", "Campaign_Id", "dbo.Campaign", "Id");
            AddForeignKey("dbo.CampaignContentModelPropertyValue", "Experience_Id", "dbo.Experience", "Id");
            AddForeignKey("dbo.AdBuildRequest", "PublishedBy_Id", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AdBuildRequest", "Campaign_Id", "dbo.Campaign", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExperienceCandidate", "DistributionPoint_Id", "dbo.DistributionPoint", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExperienceCandidate", "Campaign_Id", "dbo.Campaign", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CampaignObjective", "Target_Id", "dbo.Target", "Id");
            AddForeignKey("dbo.CampaignObjective", "Objective_Id", "dbo.TargetObjective", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CampaignObjective", "Campaign_Id", "dbo.Campaign", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Metric", "MeasurementPlan_Id", "dbo.MeasurementPlan", "Id");
            AddForeignKey("dbo.MeasurementPlan", "Id", "dbo.Campaign", "Id");
            AddForeignKey("dbo.ExperienceDownloadHistory", "Experience_Id", "dbo.Experience", "Id");
            AddForeignKey("dbo.Element", "Page_Id", "dbo.Page", "Id");
            AddForeignKey("dbo.ImportRFIResult", "Import_Id", "dbo.Import", "Id");
            AddForeignKey("dbo.ImportRFIResult", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ImportMessageLog", "Import_Id", "dbo.Import", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ImportConfig", "Import_Id", "dbo.Import", "Id");
            AddForeignKey("dbo.ImportConfig", "AdType_Id", "dbo.AdType", "Id");
            AddForeignKey("dbo.ImportAdResult", "Page_Id", "dbo.ImportPage", "Id");
            AddForeignKey("dbo.ImportAdResult", "Import_Id", "dbo.Import", "Id");
            AddForeignKey("dbo.ImportAdResult", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Import", "Experience_Id", "dbo.Experience", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RFIResult", "Page_Id", "dbo.Page", "Id");
            AddForeignKey("dbo.RFIResult", "ImportPage_Id", "dbo.ImportPage", "Id");
            AddForeignKey("dbo.RFIResult", "Import_Id", "dbo.Import", "Id");
            AddForeignKey("dbo.RFIResult", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tag", "Page_Id", "dbo.Page", "Id");
            AddForeignKey("dbo.Tag", "FeatureType_Id", "dbo.FeatureType", "Id");
            AddForeignKey("dbo.ChecklistMilestone", "ChecklistStep_Id", "dbo.ChecklistStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchMilestone", "LaunchStep_Id", "dbo.LaunchStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Launch", "Checklist_Id", "dbo.Checklist", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Launch", "Experience_Id", "dbo.Experience", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStep", "ProductionStep_Id", "dbo.ProductionStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStep", "ExecutionStage_Id", "dbo.ExecutionStage", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LaunchStep", "Launch_Id", "dbo.Launch", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checklist", "ExperienceType_Id", "dbo.ExperienceType", "Id");
            AddForeignKey("dbo.Checklist", "Platform_Id", "dbo.Platform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistStep", "ProductionStep_Id", "dbo.ProductionStep", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistStep", "ExecutionStage_Id", "dbo.ExecutionStage", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecklistStep", "Checklist_Id", "dbo.Checklist", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DistributionPoint", "Network_Id", "dbo.Network", "Id");
            AddForeignKey("dbo.DistributionPoint", "Publisher_Id", "dbo.Publisher", "Id");
            AddForeignKey("dbo.DistributionPoint", "App_Id", "dbo.App", "Id");
            AddForeignKey("dbo.DistributionPoint", "Platform_Id", "dbo.Platform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DistributionPoint", "ExperienceType_Id", "dbo.ExperienceType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Platform", "Benchmark_Id", "dbo.Benchmark", "Id");
            AddForeignKey("dbo.FeatureAdPage", "Page_Id", "dbo.Page", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeatureAdPage", "FeatureAd_Id", "dbo.FeatureAd", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AdResult", "AdEventMethod_Id", "dbo.AdEventMethod", "Id");
            AddForeignKey("dbo.AdResult", "AdEventLabel_Id", "dbo.AdEventLabel", "Id");
            AddForeignKey("dbo.AdResult", "AdEventType_Id", "dbo.AdEventType", "Id");
            AddForeignKey("dbo.AdResult", "Element_Id", "dbo.Element", "Id");
            AddForeignKey("dbo.AdResult", "FeatureAdPage_Id", "dbo.FeatureAdPage", "Id");
            AddForeignKey("dbo.AdResult", "Import_Id", "dbo.Import", "Id");
            AddForeignKey("dbo.AdResult", "Page_Id", "dbo.Page", "Id");
            AddForeignKey("dbo.AdResult", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Ad", "Metadata_Id", "dbo.AdMetadata", "Id");
            AddForeignKey("dbo.Ad", "Experience_Id", "dbo.Experience", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IncomeDemographic", "Audience_Id", "dbo.Audience", "Id");
            AddForeignKey("dbo.EthnicityDemographic", "Audience_Id", "dbo.Audience", "Id");
            AddForeignKey("dbo.AgeDemographic", "Audience_Id", "dbo.Audience", "Id");
            AddForeignKey("dbo.Audience", "Target_Id", "dbo.Target", "Id");
            AddForeignKey("dbo.Audience", "GenderDemographic_Id", "dbo.GenderDemographic", "Id");
            AddForeignKey("dbo.Experience", "DistributionPoint_Id", "dbo.DistributionPoint", "Id");
            AddForeignKey("dbo.Experience", "Publisher_Id", "dbo.Publisher", "Id");
            AddForeignKey("dbo.Experience", "ExperienceType_Id", "dbo.ExperienceType", "Id");
            AddForeignKey("dbo.Experience", "Launch_Id", "dbo.Launch", "Id");
            AddForeignKey("dbo.Experience", "Platform_Id", "dbo.Platform", "Id");
            AddForeignKey("dbo.Experience", "Campaign_Id", "dbo.Campaign", "Id");
            AddForeignKey("dbo.Experience", "Audience_Id", "dbo.Audience", "Id");
            AddForeignKey("dbo.Experience", "IncomeDemographic_Id", "dbo.IncomeDemographic", "Id");
            AddForeignKey("dbo.Experience", "GenderDemographic_Id", "dbo.GenderDemographic", "Id");
            AddForeignKey("dbo.Experience", "EthnicityDemographic_Id", "dbo.EthnicityDemographic", "Id");
            AddForeignKey("dbo.Experience", "AgeDemographic_Id", "dbo.AgeDemographic", "Id");
            AddForeignKey("dbo.Experience", "Country_Id", "dbo.Country", "Id");
            AddForeignKey("dbo.Campaign", "Audience_Id", "dbo.Audience", "Id");
            AddForeignKey("dbo.Vertical", "Benchmark_Id", "dbo.Benchmark", "Id");
        }
    }
}
