namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Clear2 : DbMigration
    {
        public override void Up()
        {
			//AddColumn("dbo.FeatureCategory", "FeatureType_Id", c => c.Int());
			//CreateIndex("dbo.FeatureCategory", "FeatureType_Id");
			//AddForeignKey("dbo.FeatureCategory", "FeatureType_Id", "dbo.FeatureType", "Id");
        }
        
        public override void Down()
        {
			//DropForeignKey("dbo.FeatureCategory", "FeatureType_Id", "dbo.FeatureType");
			//DropIndex("dbo.FeatureCategory", new[] { "FeatureType_Id" });
			//DropColumn("dbo.FeatureCategory", "FeatureType_Id");
        }
    }
}
