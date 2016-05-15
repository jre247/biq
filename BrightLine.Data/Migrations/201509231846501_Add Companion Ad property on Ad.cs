namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanionAdpropertyonAd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "CompanionAdId", c => c.Int());
            CreateIndex("dbo.Ad", "CompanionAdId");
            AddForeignKey("dbo.Ad", "CompanionAdId", "dbo.Ad", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ad", "CompanionAdId", "dbo.Ad");
            DropIndex("dbo.Ad", new[] { "CompanionAdId" });
            DropColumn("dbo.Ad", "CompanionAdId");
        }
    }
}
