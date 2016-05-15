namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class FeatureAdremoved : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.FeatureAd", "Feature_Id", "dbo.Feature");
			DropForeignKey("dbo.FeatureAd", "Ad_Id", "dbo.Ad");
			DropForeignKey("dbo.Page", "FeatureAd_Id", "dbo.FeatureAd");
			DropIndex("dbo.FeatureAd", new[] { "Feature_Id" });
			DropIndex("dbo.FeatureAd", new[] { "Ad_Id" });
			DropIndex("dbo.Page", new[] { "FeatureAd_Id" });
			DropColumn("dbo.Page", "FeatureAd_Id");
			Sql(@"
delete from [dbo].[AdFeature]
insert into [dbo].[AdFeature] (Ad_Id, Feature_Id)
	select Ad_Id, Feature_Id
		from [dbo].[FeatureAd]");
			DropTable("dbo.FeatureAd");
		}

		public override void Down()
		{
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
				.PrimaryKey(t => t.Id);

			AddColumn("dbo.Page", "FeatureAd_Id", c => c.Int());
			CreateIndex("dbo.Page", "FeatureAd_Id");
			CreateIndex("dbo.FeatureAd", "Ad_Id");
			CreateIndex("dbo.FeatureAd", "Feature_Id");
			AddForeignKey("dbo.Page", "FeatureAd_Id", "dbo.FeatureAd", "Id");
			AddForeignKey("dbo.FeatureAd", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
			AddForeignKey("dbo.FeatureAd", "Feature_Id", "dbo.Feature", "Id", cascadeDelete: true);
		}
	}
}
