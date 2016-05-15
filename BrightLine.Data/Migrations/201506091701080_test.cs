namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AdFeature", newName: "FeatureAd");
            RenameTable(name: "dbo.FeatureTypeProductLine", newName: "ProductLineFeatureType");
            RenameTable(name: "dbo.FeatureCategoryFeatureType", newName: "FeatureTypeFeatureCategory");
            RenameTable(name: "dbo.BlueprintFeatureCategory", newName: "FeatureCategoryBlueprint");
            DropIndex("dbo.FeatureTypeProductLine", new[] { "FeatureType_Id" });
            DropIndex("dbo.FeatureTypeProductLine", new[] { "ProductLine_Id" });
            DropIndex("dbo.FeatureCategoryFeatureType", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.FeatureCategoryFeatureType", new[] { "FeatureType_Id" });
            DropIndex("dbo.BlueprintFeatureCategory", new[] { "Blueprint_Id" });
            DropIndex("dbo.BlueprintFeatureCategory", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.AdFeature", new[] { "Ad_Id" });
            DropIndex("dbo.AdFeature", new[] { "Feature_Id" });
            DropPrimaryKey("dbo.FeatureAd");
            DropPrimaryKey("dbo.ProductLineFeatureType");
            DropPrimaryKey("dbo.FeatureTypeFeatureCategory");
            DropPrimaryKey("dbo.FeatureCategoryBlueprint");
            AddPrimaryKey("dbo.FeatureAd", new[] { "Feature_Id", "Ad_Id" });
            AddPrimaryKey("dbo.ProductLineFeatureType", new[] { "ProductLine_Id", "FeatureType_Id" });
            AddPrimaryKey("dbo.FeatureTypeFeatureCategory", new[] { "FeatureType_Id", "FeatureCategory_Id" });
            AddPrimaryKey("dbo.FeatureCategoryBlueprint", new[] { "FeatureCategory_Id", "Blueprint_Id" });
            CreateIndex("dbo.FeatureAd", "Feature_Id");
            CreateIndex("dbo.FeatureAd", "Ad_Id");
            CreateIndex("dbo.FeatureCategoryBlueprint", "FeatureCategory_Id");
            CreateIndex("dbo.FeatureCategoryBlueprint", "Blueprint_Id");
            CreateIndex("dbo.FeatureTypeFeatureCategory", "FeatureType_Id");
            CreateIndex("dbo.FeatureTypeFeatureCategory", "FeatureCategory_Id");
            CreateIndex("dbo.ProductLineFeatureType", "ProductLine_Id");
            CreateIndex("dbo.ProductLineFeatureType", "FeatureType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductLineFeatureType", new[] { "FeatureType_Id" });
            DropIndex("dbo.ProductLineFeatureType", new[] { "ProductLine_Id" });
            DropIndex("dbo.FeatureTypeFeatureCategory", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.FeatureTypeFeatureCategory", new[] { "FeatureType_Id" });
            DropIndex("dbo.FeatureCategoryBlueprint", new[] { "Blueprint_Id" });
            DropIndex("dbo.FeatureCategoryBlueprint", new[] { "FeatureCategory_Id" });
            DropIndex("dbo.FeatureAd", new[] { "Ad_Id" });
            DropIndex("dbo.FeatureAd", new[] { "Feature_Id" });
            DropPrimaryKey("dbo.FeatureCategoryBlueprint");
            DropPrimaryKey("dbo.FeatureTypeFeatureCategory");
            DropPrimaryKey("dbo.ProductLineFeatureType");
            DropPrimaryKey("dbo.FeatureAd");
            AddPrimaryKey("dbo.FeatureCategoryBlueprint", new[] { "Blueprint_Id", "FeatureCategory_Id" });
            AddPrimaryKey("dbo.FeatureTypeFeatureCategory", new[] { "FeatureCategory_Id", "FeatureType_Id" });
            AddPrimaryKey("dbo.ProductLineFeatureType", new[] { "FeatureType_Id", "ProductLine_Id" });
            AddPrimaryKey("dbo.FeatureAd", new[] { "Ad_Id", "Feature_Id" });
            CreateIndex("dbo.AdFeature", "Feature_Id");
            CreateIndex("dbo.AdFeature", "Ad_Id");
            CreateIndex("dbo.BlueprintFeatureCategory", "FeatureCategory_Id");
            CreateIndex("dbo.BlueprintFeatureCategory", "Blueprint_Id");
            CreateIndex("dbo.FeatureCategoryFeatureType", "FeatureType_Id");
            CreateIndex("dbo.FeatureCategoryFeatureType", "FeatureCategory_Id");
            CreateIndex("dbo.FeatureTypeProductLine", "ProductLine_Id");
            CreateIndex("dbo.FeatureTypeProductLine", "FeatureType_Id");
            RenameTable(name: "dbo.FeatureCategoryBlueprint", newName: "BlueprintFeatureCategory");
            RenameTable(name: "dbo.FeatureTypeFeatureCategory", newName: "FeatureCategoryFeatureType");
            RenameTable(name: "dbo.ProductLineFeatureType", newName: "FeatureTypeProductLine");
            RenameTable(name: "dbo.FeatureAd", newName: "AdFeature");
        }
    }
}
