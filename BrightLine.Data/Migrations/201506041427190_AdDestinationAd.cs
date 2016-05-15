namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdDestinationAd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "DestinationAd_Id", c => c.Int());
            AddForeignKey("dbo.Ad", "DestinationAd_Id", "dbo.Ad", "Id");
            CreateIndex("dbo.Ad", "DestinationAd_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ad", new[] { "DestinationAd_Id" });
            DropForeignKey("dbo.Ad", "DestinationAd_Id", "dbo.Ad");
            DropColumn("dbo.Ad", "DestinationAd_Id");
        }
    }
}
