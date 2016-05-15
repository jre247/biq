namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class adduniqueconstrainttolookuptables : DbMigration
	{
		public override void Up()
		{
			Sql("ALTER TABLE dbo.Product ADD CONSTRAINT UC_Product_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Vertical ADD CONSTRAINT UC_Vertical_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.AdFormat ADD CONSTRAINT UC_AdFormat_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.AdFunction ADD CONSTRAINT UC_AdFunction_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.AdType ADD CONSTRAINT UC_AdType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.AdTypeGroup ADD CONSTRAINT UC_AdTypeGroup_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Advertiser ADD CONSTRAINT UC_Advertiser_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Brand ADD CONSTRAINT UC_Brand_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Category ADD CONSTRAINT UC_Category_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.CmsPublishStatus ADD CONSTRAINT UC_CmsPublishStatus_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.FeatureCategory ADD CONSTRAINT UC_FeatureCategory_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.FeatureType ADD CONSTRAINT UC_FeatureType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.FeatureTypeGroup ADD CONSTRAINT UC_FeatureTypeGroup_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Platform ADD CONSTRAINT UC_Platform_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.PlatformGroup ADD CONSTRAINT UC_PlatformGroup_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.RateType ADD CONSTRAINT UC_RateType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.ResourceType ADD CONSTRAINT UC_ResourceType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Role ADD CONSTRAINT UC_Role_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Segment ADD CONSTRAINT UC_Segment_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.StorageSource ADD CONSTRAINT UC_StorageSource_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.SubSegment ADD CONSTRAINT UC_SubSegment_Name UNIQUE (Name)");
		}

		public override void Down()
		{
			Sql("ALTER TABLE dbo.Product DROP CONSTRAINT UC_Product_Name");
			Sql("ALTER TABLE dbo.Vertical DROP CONSTRAINT UC_Vertical_Name");
			Sql("ALTER TABLE dbo.AdFormat DROP CONSTRAINT UC_AdFormat_Name");
			Sql("ALTER TABLE dbo.AdFunction DROP CONSTRAINT UC_AdFunction_Name");
			Sql("ALTER TABLE dbo.AdType DROP CONSTRAINT UC_AdType_Name");
			Sql("ALTER TABLE dbo.AdTypeGroup DROP CONSTRAINT UC_AdTypeGroup_Name");
			Sql("ALTER TABLE dbo.Advertiser DROP CONSTRAINT UC_Advertiser_Name");
			Sql("ALTER TABLE dbo.Brand DROP CONSTRAINT UC_Brand_Name");
			Sql("ALTER TABLE dbo.Category DROP CONSTRAINT UC_Category_Name");
			Sql("ALTER TABLE dbo.CmsPublishStatus DROP CONSTRAINT UC_CmsPublishStatus_Name");
			Sql("ALTER TABLE dbo.FeatureCategory DROP CONSTRAINT UC_FeatureCategory_Name");
			Sql("ALTER TABLE dbo.FeatureType DROP CONSTRAINT UC_FeatureType_Name");
			Sql("ALTER TABLE dbo.FeatureTypeGroup DROP CONSTRAINT UC_FeatureTypeGroup_Name");
			Sql("ALTER TABLE dbo.Platform DROP CONSTRAINT UC_Platform_Name");
			Sql("ALTER TABLE dbo.PlatformGroup DROP CONSTRAINT UC_PlatformGroup_Name");
			Sql("ALTER TABLE dbo.RateType DROP CONSTRAINT UC_RateType_Name");
			Sql("ALTER TABLE dbo.ResourceType DROP CONSTRAINT UC_ResourceType_Name");
			Sql("ALTER TABLE dbo.Role DROP CONSTRAINT UC_Role_Name");
			Sql("ALTER TABLE dbo.Segment DROP CONSTRAINT UC_Segment_Name");
			Sql("ALTER TABLE dbo.StorageSource DROP CONSTRAINT UC_StorageSource_Name");
			Sql("ALTER TABLE dbo.SubSegment DROP CONSTRAINT UC_SubSegment_Name");
		}
	}
}
