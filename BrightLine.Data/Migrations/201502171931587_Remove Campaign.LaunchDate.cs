namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCampaignLaunchDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaign", "LaunchDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaign", "LaunchDate", c => c.DateTime());
        }
    }
}
