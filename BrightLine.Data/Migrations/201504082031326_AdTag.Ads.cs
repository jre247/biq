namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdTagAds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad");
            //DropIndex("dbo.AdTag", new[] { "Ad_Id" });
            DropColumn("dbo.AdTag", "Ad_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdTag", "Ad_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AdTag", "Ad_Id");
            AddForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
        }
    }
}
