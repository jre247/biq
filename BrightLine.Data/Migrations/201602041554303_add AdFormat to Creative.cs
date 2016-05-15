namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdFormattoCreative : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creative", "AdFormat_Id", c => c.Int());
            CreateIndex("dbo.Creative", "AdFormat_Id");
            AddForeignKey("dbo.Creative", "AdFormat_Id", "dbo.AdFormat", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creative", "AdFormat_Id", "dbo.AdFormat");
            DropIndex("dbo.Creative", new[] { "AdFormat_Id" });
            DropColumn("dbo.Creative", "AdFormat_Id");
        }
    }
}
