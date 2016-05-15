namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricHexColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Metric", "HexColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Metric", "HexColor");
        }
    }
}
