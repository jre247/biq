namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameUxtvTypetoCampaignType : DbMigration
    {
        public override void Up()
		{
			RenameColumn("dbo.Campaign", "UxtvType", "CampaignType");
        }
        
        public override void Down()
		{
			RenameColumn("dbo.Campaign", "CampaignType", "UxtvType");
        }
    }
}
