namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pageupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Page", "PageDefinition_Id", c => c.Int());
            AddForeignKey("dbo.Page", "PageDefinition_Id", "dbo.PageDefinition", "Id");
            CreateIndex("dbo.Page", "PageDefinition_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Page", new[] { "PageDefinition_Id" });
            DropForeignKey("dbo.Page", "PageDefinition_Id", "dbo.PageDefinition");
            DropColumn("dbo.Page", "PageDefinition_Id");
        }
    }
}
