namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Camapign_RemoveProperties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaign", "MediaSpend");
            DropColumn("dbo.Campaign", "BandwidthSpend");
            DropColumn("dbo.Campaign", "SettingsLastUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaign", "SettingsLastUpdated", c => c.DateTime());
            AddColumn("dbo.Campaign", "BandwidthSpend", c => c.Int(nullable: false));
            AddColumn("dbo.Campaign", "MediaSpend", c => c.Int(nullable: false));
        }
    }
}
