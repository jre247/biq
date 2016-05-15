namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flight", "ExpectedBeginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Flight", "ExpectedEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Flight", "BeginDate", c => c.DateTime());
            AlterColumn("dbo.Flight", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Flight", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Flight", "BeginDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Flight", "ExpectedEndDate");
            DropColumn("dbo.Flight", "ExpectedBeginDate");
        }
    }
}
