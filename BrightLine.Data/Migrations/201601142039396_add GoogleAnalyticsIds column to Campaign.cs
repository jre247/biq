namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoogleAnalyticsIdscolumntoCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaign", "GoogleAnalyticsIds", c => c.String(maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaign", "GoogleAnalyticsIds");
        }
    }
}
