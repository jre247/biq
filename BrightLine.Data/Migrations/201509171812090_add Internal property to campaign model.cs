namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInternalpropertytocampaignmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaign", "Internal", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaign", "Internal");
        }
    }
}
