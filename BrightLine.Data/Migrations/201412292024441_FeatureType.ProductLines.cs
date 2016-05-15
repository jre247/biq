namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureTypeProductLines : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FeatureType", "ProductLine_Id", "dbo.ProductLine");
            DropIndex("dbo.FeatureType", new[] { "ProductLine_Id" });
            CreateTable(
                "dbo.ProductLineFeatureType",
                c => new
                    {
                        ProductLine_Id = c.Int(nullable: false),
                        FeatureType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductLine_Id, t.FeatureType_Id })
                .ForeignKey("dbo.ProductLine", t => t.ProductLine_Id, cascadeDelete: true)
                .ForeignKey("dbo.FeatureType", t => t.FeatureType_Id, cascadeDelete: true)
                .Index(t => t.ProductLine_Id)
                .Index(t => t.FeatureType_Id);
            
            DropColumn("dbo.FeatureType", "ProductLine_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FeatureType", "ProductLine_Id", c => c.Int());
            DropIndex("dbo.ProductLineFeatureType", new[] { "FeatureType_Id" });
            DropIndex("dbo.ProductLineFeatureType", new[] { "ProductLine_Id" });
            DropForeignKey("dbo.ProductLineFeatureType", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.ProductLineFeatureType", "ProductLine_Id", "dbo.ProductLine");
            DropTable("dbo.ProductLineFeatureType");
            CreateIndex("dbo.FeatureType", "ProductLine_Id");
            AddForeignKey("dbo.FeatureType", "ProductLine_Id", "dbo.ProductLine", "Id");
        }
    }
}
