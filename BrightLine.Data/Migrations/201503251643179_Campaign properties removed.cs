namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campaignpropertiesremoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaign", "Spend");
            DropColumn("dbo.Campaign", "OldIdentity");
            DropColumn("dbo.Campaign", "EngagementRateIndex");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaign", "EngagementRateIndex", c => c.Double(nullable: false));
            AddColumn("dbo.Campaign", "OldIdentity", c => c.Int());
            AddColumn("dbo.Campaign", "Spend", c => c.Int(nullable: false));
        }
    }
}
