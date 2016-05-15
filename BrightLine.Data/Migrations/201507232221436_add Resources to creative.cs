namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResourcestocreative : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creative", "Resource_Id", "dbo.Resource");
            DropIndex("dbo.Creative", new[] { "Resource_Id" });
            AddColumn("dbo.Resource", "Creative_Id", c => c.Int());
            CreateIndex("dbo.Resource", "Creative_Id");
            AddForeignKey("dbo.Resource", "Creative_Id", "dbo.Creative", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resource", "Creative_Id", "dbo.Creative");
            DropIndex("dbo.Resource", new[] { "Creative_Id" });
            DropColumn("dbo.Resource", "Creative_Id");
            CreateIndex("dbo.Creative", "Resource_Id");
            AddForeignKey("dbo.Creative", "Resource_Id", "dbo.Resource", "Id");
        }
    }
}
