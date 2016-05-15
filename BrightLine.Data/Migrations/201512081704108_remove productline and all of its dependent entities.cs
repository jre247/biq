namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeproductlineandallofitsdependententities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductLine", "Thumbnail_Id", "dbo.Resource");
            DropForeignKey("dbo.FeatureTypeProductLine", "FeatureType_Id", "dbo.FeatureType");
            DropForeignKey("dbo.FeatureTypeProductLine", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.Creative", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.ProductLineBlueprint", "Blueprint_Id", "dbo.Blueprint");
            DropForeignKey("dbo.ProductLineBlueprint", "ProductLine_Id", "dbo.ProductLine");
            DropForeignKey("dbo.Role", "SecurableAction_Id", "dbo.SecurableAction");
            DropForeignKey("dbo.SecurableAction", "Target_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.SecurableArea", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.Role", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.User", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.User", "SecurableAction_Id", "dbo.SecurableAction");
            DropIndex("dbo.User", new[] { "SecurableArea_Id" });
            DropIndex("dbo.User", new[] { "SecurableAction_Id" });
            DropIndex("dbo.Creative", new[] { "ProductLine_Id" });
            DropIndex("dbo.ProductLine", new[] { "Thumbnail_Id" });
            DropIndex("dbo.Role", new[] { "SecurableAction_Id" });
            DropIndex("dbo.Role", new[] { "SecurableArea_Id" });
            DropIndex("dbo.ProductLineBlueprint", new[] { "Blueprint_Id" });
            DropIndex("dbo.ProductLineBlueprint", new[] { "ProductLine_Id" });
            DropIndex("dbo.SecurableAction", new[] { "Target_Id" });
            DropIndex("dbo.SecurableArea", new[] { "SecurableArea_Id" });
            DropIndex("dbo.FeatureTypeProductLine", new[] { "FeatureType_Id" });
            DropIndex("dbo.FeatureTypeProductLine", new[] { "ProductLine_Id" });
            DropColumn("dbo.User", "SecurableArea_Id");
            DropColumn("dbo.User", "SecurableAction_Id");
            DropColumn("dbo.Creative", "ProductLine_Id");
            DropColumn("dbo.Role", "SecurableAction_Id");
            DropColumn("dbo.Role", "SecurableArea_Id");
            DropTable("dbo.ProductLine");
            DropTable("dbo.ProductLineBlueprint");
            DropTable("dbo.SecurableAction");
            DropTable("dbo.SecurableArea");
            DropTable("dbo.FeatureTypeProductLine");

			Sql(@"
				IF OBJECT_ID('dbo.PlatformPlacement', 'U') IS NOT NULL
					DROP TABLE dbo.PlatformPlacement; 

				IF OBJECT_ID('dbo.NetworkPlatformPlacement', 'U') IS NOT NULL
					DROP TABLE dbo.NetworkPlatformPlacement; 

				IF OBJECT_ID('dbo.BlueprintFeatureCategory', 'U') IS NOT NULL
					DROP TABLE dbo.BlueprintFeatureCategory; 

				IF OBJECT_ID('dbo.AppPlatformPlacement', 'U') IS NOT NULL
					DROP TABLE dbo.AppPlatformPlacement; 
			");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FeatureTypeProductLine",
                c => new
                    {
                        FeatureType_Id = c.Int(nullable: false),
                        ProductLine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FeatureType_Id, t.ProductLine_Id });
            
            CreateTable(
                "dbo.SecurableArea",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Route = c.String(),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        SecurableArea_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SecurableAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Operation = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Target_Id = c.Int(),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductLine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        HexBackgroundColor = c.String(maxLength: 255),
                        Abbreviation = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Thumbnail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Role", "SecurableArea_Id", c => c.Int());
            AddColumn("dbo.Role", "SecurableAction_Id", c => c.Int());
            AddColumn("dbo.Creative", "ProductLine_Id", c => c.Int());
            AddColumn("dbo.User", "SecurableAction_Id", c => c.Int());
            AddColumn("dbo.User", "SecurableArea_Id", c => c.Int());
            CreateIndex("dbo.FeatureTypeProductLine", "ProductLine_Id");
            CreateIndex("dbo.FeatureTypeProductLine", "FeatureType_Id");
            CreateIndex("dbo.SecurableArea", "SecurableArea_Id");
            CreateIndex("dbo.SecurableAction", "Target_Id");
            CreateIndex("dbo.ProductLineBlueprint", "ProductLine_Id");
            CreateIndex("dbo.ProductLineBlueprint", "Blueprint_Id");
            CreateIndex("dbo.Role", "SecurableArea_Id");
            CreateIndex("dbo.Role", "SecurableAction_Id");
            CreateIndex("dbo.ProductLine", "Thumbnail_Id");
            CreateIndex("dbo.Creative", "ProductLine_Id");
            CreateIndex("dbo.User", "SecurableAction_Id");
            CreateIndex("dbo.User", "SecurableArea_Id");
            AddForeignKey("dbo.User", "SecurableAction_Id", "dbo.SecurableAction", "Id");
            AddForeignKey("dbo.User", "SecurableArea_Id", "dbo.SecurableArea", "Id");
            AddForeignKey("dbo.Role", "SecurableArea_Id", "dbo.SecurableArea", "Id");
            AddForeignKey("dbo.SecurableArea", "SecurableArea_Id", "dbo.SecurableArea", "Id");
            AddForeignKey("dbo.SecurableAction", "Target_Id", "dbo.SecurableArea", "Id");
            AddForeignKey("dbo.Role", "SecurableAction_Id", "dbo.SecurableAction", "Id");
            AddForeignKey("dbo.ProductLineBlueprint", "ProductLine_Id", "dbo.ProductLine", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductLineBlueprint", "Blueprint_Id", "dbo.Blueprint", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Creative", "ProductLine_Id", "dbo.ProductLine", "Id");
            AddForeignKey("dbo.FeatureTypeProductLine", "ProductLine_Id", "dbo.ProductLine", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeatureTypeProductLine", "FeatureType_Id", "dbo.FeatureType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductLine", "Thumbnail_Id", "dbo.Resource", "Id");
        }
    }
}
