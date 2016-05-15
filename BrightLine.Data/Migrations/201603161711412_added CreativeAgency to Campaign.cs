namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCreativeAgencytoCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaign", "CreativeAgency_Id", c => c.Int(nullable: true));
            CreateIndex("dbo.Campaign", "CreativeAgency_Id");
            AddForeignKey("dbo.Campaign", "CreativeAgency_Id", "dbo.Agency", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaign", "CreativeAgency_Id", "dbo.Agency");
            DropIndex("dbo.Campaign", new[] { "CreativeAgency_Id" });
            DropColumn("dbo.Campaign", "CreativeAgency_Id");
        }
    }
}
