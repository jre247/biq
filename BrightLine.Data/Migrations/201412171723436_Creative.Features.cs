namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreativeFeatures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creative", "Blueprint_Id", "dbo.Blueprint");
            DropIndex("dbo.Creative", new[] { "Blueprint_Id" });
            AddColumn("dbo.Feature", "Creative_Id", c => c.Int());
            AddForeignKey("dbo.Feature", "Creative_Id", "dbo.Creative", "Id");
            CreateIndex("dbo.Feature", "Creative_Id");
            DropColumn("dbo.Creative", "Blueprint_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Creative", "Blueprint_Id", c => c.Int());
            DropIndex("dbo.Feature", new[] { "Creative_Id" });
            DropForeignKey("dbo.Feature", "Creative_Id", "dbo.Creative");
            DropColumn("dbo.Feature", "Creative_Id");
            CreateIndex("dbo.Creative", "Blueprint_Id");
            AddForeignKey("dbo.Creative", "Blueprint_Id", "dbo.Blueprint", "Id");
        }
    }
}
