namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductLineFeatureTypesThumbnailDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeatureType", "ProductLine_Id", c => c.Int());
            AddColumn("dbo.ProductLine", "Description", c => c.String(nullable: false));
            AddColumn("dbo.ProductLine", "Thumbnail_Id", c => c.Int());
            AddForeignKey("dbo.FeatureType", "ProductLine_Id", "dbo.ProductLine", "Id");
            AddForeignKey("dbo.ProductLine", "Thumbnail_Id", "dbo.Resource", "Id");
            CreateIndex("dbo.FeatureType", "ProductLine_Id");
            CreateIndex("dbo.ProductLine", "Thumbnail_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductLine", new[] { "Thumbnail_Id" });
            DropIndex("dbo.FeatureType", new[] { "ProductLine_Id" });
            DropForeignKey("dbo.ProductLine", "Thumbnail_Id", "dbo.Resource");
            DropForeignKey("dbo.FeatureType", "ProductLine_Id", "dbo.ProductLine");
            DropColumn("dbo.ProductLine", "Thumbnail_Id");
            DropColumn("dbo.ProductLine", "Description");
            DropColumn("dbo.FeatureType", "ProductLine_Id");
        }
    }
}
