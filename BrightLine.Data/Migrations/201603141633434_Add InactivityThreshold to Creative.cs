namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInactivityThresholdtoCreative : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creative", "InactivityThreshold", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Creative", "InactivityThreshold");
        }
    }
}
